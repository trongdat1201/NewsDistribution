using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// Orchestrates AI providers with RAG context injection, tool calling,
    /// conversation memory management, and few-shot examples.
    ///
    /// Providers are constructed lazily — only the ones required by the
    /// configured <see cref="AIProviderMode"/> are instantiated.
    /// </summary>
    public class HybridAIProvider : IAIProvider
    {
        private readonly ToolRegistry _toolRegistry;
        private readonly RagEngine _ragEngine;
        private readonly AIProviderMode _mode;

        // Lazy provider singletons — avoid paying the cost (network probe, model load)
        // for providers we won't use this session.
        private readonly Lazy<GeminiProvider> _lazyGemini;
        private readonly Lazy<OllamaProvider> _lazyOllama;
        private readonly Lazy<ClaudeProvider> _lazyClaude;

        public string ProviderName
        {
            get
            {
                return _mode switch
                {
                    AIProviderMode.GeminiOnly => "Gemini",
                    AIProviderMode.OllamaOnly => "Ollama (qwen3.6)",
                    AIProviderMode.Hybrid => "Hybrid (Gemini + Claude + Ollama)",
                    AIProviderMode.ClaudeOnly => "Claude",
                    _ => "Gemini"
                };
            }
        }

        public bool IsAvailable => _mode switch
        {
            AIProviderMode.GeminiOnly => GetGemini().IsAvailable,
            AIProviderMode.OllamaOnly => GetOllama().IsAvailable,
            AIProviderMode.ClaudeOnly => GetClaude().IsAvailable,
            AIProviderMode.Hybrid => (_lazyGemini.IsValueCreated && GetGemini().IsAvailable)
                                  || (_lazyClaude.IsValueCreated && GetClaude().IsAvailable)
                                  || (_lazyOllama.IsValueCreated && GetOllama().IsAvailable),
            _ => false
        };

        /// <inheritdoc />
        public event Action<string, string> OnLog;

        public HybridAIProvider() : this(new ToolRegistry(), new RagEngine()) { }

        public HybridAIProvider(ToolRegistry toolRegistry, RagEngine ragEngine)
        {
            _toolRegistry = toolRegistry ?? new ToolRegistry();
            _ragEngine = ragEngine ?? new RagEngine();

            _lazyGemini = new Lazy<GeminiProvider>(() => new GeminiProvider());
            _lazyOllama = new Lazy<OllamaProvider>(() => new OllamaProvider());
            _lazyClaude = new Lazy<ClaudeProvider>(() => new ClaudeProvider());

            _mode = AiConfig.ProviderMode;

            // Pre-warm providers required by the active mode so IsAvailable reflects reality
            // without forcing the first user message to bear the cold-start cost.
            switch (_mode)
            {
                case AIProviderMode.GeminiOnly:
                    _ = GetGemini().IsAvailable;
                    break;
                case AIProviderMode.OllamaOnly:
                    _ = GetOllama().IsAvailable;
                    break;
                case AIProviderMode.ClaudeOnly:
                    _ = GetClaude().IsAvailable;
                    break;
                case AIProviderMode.Hybrid:
                    _ = GetGemini().IsAvailable;
                    _ = GetClaude().IsAvailable;
                    _ = GetOllama().IsAvailable;
                    break;
            }
        }

        private GeminiProvider GetGemini() => _lazyGemini.Value;
        private OllamaProvider GetOllama() => _lazyOllama.Value;
        private ClaudeProvider GetClaude() => _lazyClaude.Value;

        public async Task<AIResponse> SendMessageAsync(
            string userMessage,
            ConversationHistory history,
            CancellationToken cancellationToken = default)
        {
            return await SendMessageWithSystemAsync(userMessage, history, null, cancellationToken);
        }

        public async Task<AIResponse> SendMessageWithSystemAsync(
            string userMessage,
            ConversationHistory history,
            string systemPrompt,
            CancellationToken cancellationToken = default)
        {
            var mode = AiConfig.ProviderMode;
            return mode switch
            {
                AIProviderMode.GeminiOnly => await SendWithToolsAsync(GetGemini(), userMessage, history, systemPrompt, cancellationToken),
                AIProviderMode.OllamaOnly => await SendWithToolsAsync(GetOllama(), userMessage, history, systemPrompt, cancellationToken),
                AIProviderMode.ClaudeOnly => await SendWithToolsAsync(GetClaude(), userMessage, history, systemPrompt, cancellationToken),
                AIProviderMode.Hybrid => await SendWithHybridAsync(userMessage, history, systemPrompt, cancellationToken),
                _ => new AIResponse { Success = false, Error = "Chế độ provider không xác định.", Provider = "Hybrid" }
            };
        }

        private async Task<AIResponse> SendWithHybridAsync(
            string userMessage,
            ConversationHistory history,
            string systemPrompt,
            CancellationToken cancellationToken)
        {
            OnLog?.Invoke("info", "[Hybrid] Bắt đầu luồng: Gemini → Claude → Ollama");

            // Use lazy factories so we don't pay the cost for a provider we end up
            // not using (e.g. if Gemini succeeds first, Ollama is never instantiated).
            IAIProvider[] cascade = { GetGemini(), GetClaude(), GetOllama() };
            string[] names = { "Gemini", "Claude", "Ollama" };

            string lastError = string.Empty;
            for (int i = 0; i < cascade.Length; i++)
            {
                var provider = cascade[i];
                var name = names[i];

                if (!provider.IsAvailable)
                {
                    OnLog?.Invoke("info", $"[Hybrid] → {name} không khả dụng");
                    continue;
                }

                OnLog?.Invoke("info", $"[Hybrid] → Thử {name}...");
                var result = await SendWithToolsAsync(provider, userMessage, history, systemPrompt, cancellationToken);

                if (result.Success)
                {
                    OnLog?.Invoke("info", $"[Hybrid] ✓ {name} thành công ({result.LatencyMs}ms)");
                    if (name == "Ollama")
                        result.Text = $"[Ollama fallback]\n{result.Text}";
                    return result;
                }

                lastError = result.Error;
                OnLog?.Invoke("info", $"[Hybrid] ✗ {name} thất bại: {result.Error}");
            }

            OnLog?.Invoke("error", "[Hybrid] ✗ Tất cả providers đều thất bại");
            return new AIResponse
            {
                Success = false,
                Error = string.IsNullOrEmpty(lastError)
                    ? "Tất cả các nhà cung cấp AI đều không khả dụng."
                    : lastError,
                Provider = "Hybrid"
            };
        }

        private async Task<AIResponse> SendWithToolsAsync(
            IAIProvider provider,
            string userMessage,
            ConversationHistory history,
            string callerSystemPrompt,
            CancellationToken ct)
        {
            if (!provider.IsAvailable)
                return new AIResponse { Success = false, Error = $"{provider.ProviderName} không khả dụng.", Provider = provider.ProviderName };

            var workingHistory = CloneHistory(history);
            int initialMessageCount = workingHistory.Messages.Count;

            var allDefinitions = _toolRegistry.GetAllDefinitions();

            string ragContext = string.Empty;
            try
            {
                ragContext = await _ragEngine.BuildContextAsync(userMessage, ct);
            }
            catch
            {
                ragContext = string.Empty;
            }

            string toolPrompt = BuildSystemPrompt(allDefinitions, ragContext);
            string systemPrompt = string.IsNullOrWhiteSpace(callerSystemPrompt) ? toolPrompt : callerSystemPrompt;

            // Retry logic cho Ollama (dễ parse sai JSON)
            const int maxRetries = 2;
            AIResponse response = null;

            for (int retryRound = 0; retryRound <= maxRetries; retryRound++)
            {
                if (retryRound > 0)
                {
                    OnLog?.Invoke("info", $"[Hybrid] Retry {retryRound}/{maxRetries} với prompt cải thiện...");
                    workingHistory.Messages.Add(new ConversationMessage
                    {
                        Role = "user",
                        Content = "Lưu ý: Hãy chỉ trả về JSON thuần túy theo format {\"tool_call\":{\"tool\":\"tên_tool\",\"params\":{...}}} mà không có giải thích thêm."
                    });
                }

                response = await provider.SendMessageWithSystemAsync(userMessage, workingHistory, systemPrompt, ct);

                if (!response.Success)
                    return FinalizeResponse(FallbackResponse(response, provider.ProviderName), history, workingHistory, initialMessageCount);

                response.Text = StripTechnicalArtifacts(response.Text);

                if (response.WantsToolCall && response.ToolCall != null)
                    break;

                if (retryRound >= maxRetries)
                    break;
            }

            ToolCallRequest previousToolCall = null;

            for (int round = 0; round < 5; round++)
            {
                if (!response.WantsToolCall || response.ToolCall == null)
                    break;

                var toolCall = response.ToolCall;

                if (previousToolCall != null
                    && string.Equals(previousToolCall.ToolName, toolCall.ToolName, StringComparison.OrdinalIgnoreCase)
                    && ParametersEqual(previousToolCall.Parameters, toolCall.Parameters))
                {
                    OnLog?.Invoke("warn",
                        $"[Hybrid] AI lặp lại tool '{toolCall.ToolName}' với cùng tham số — thoát vòng lặp để tránh kẹt.");
                    break;
                }

                workingHistory.Messages.Add(new ConversationMessage
                {
                    Role = "model",
                    Content = FormatToolCallForHistory(toolCall)
                });

                var toolResult = await _toolRegistry.ExecuteAsync(toolCall, ct);

                workingHistory.Messages.Add(new ConversationMessage
                {
                    Role = "user",
                    Content = FormatToolResultForHistory(toolCall.ToolName, toolResult)
                });

                previousToolCall = toolCall;

                response = await provider.SendMessageWithSystemAsync(string.Empty, workingHistory, systemPrompt, ct);

                if (!response.Success)
                    return FinalizeResponse(FallbackResponse(response, provider.ProviderName), history, workingHistory, initialMessageCount);

                response.Text = StripTechnicalArtifacts(response.Text);
            }

            return FinalizeResponse(response, history, workingHistory, initialMessageCount);
        }

        /// <summary>
        /// Appends any messages added to <paramref name="workingHistory"/> during the tool loop
        /// back into the original <paramref name="history"/> so the caller's view of the
        /// conversation stays in sync (no context amnesia on the next turn).
        /// </summary>
        private static AIResponse FinalizeResponse(
            AIResponse response,
            ConversationHistory history,
            ConversationHistory workingHistory,
            int initialMessageCount)
        {
            if (history != null && history.Messages != null && workingHistory != null && workingHistory.Messages != null
                && workingHistory.Messages.Count > initialMessageCount)
            {
                if (history.Messages == null)
                    history.Messages = new List<ConversationMessage>();

                for (int i = initialMessageCount; i < workingHistory.Messages.Count; i++)
                {
                    history.Messages.Add(workingHistory.Messages[i]);
                }
            }

            return response;
        }

        /// <summary>
        /// Order-insensitive shallow parameter comparison for tool-call loop detection.
        /// </summary>
        private static bool ParametersEqual(
            Dictionary<string, object> a,
            Dictionary<string, object> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null) return false;
            if (a.Count != b.Count) return false;

            foreach (var kv in a)
            {
                if (!b.TryGetValue(kv.Key, out var otherValue)) return false;
                var aStr = kv.Value?.ToString() ?? string.Empty;
                var bStr = otherValue?.ToString() ?? string.Empty;
                if (!string.Equals(aStr, bStr, StringComparison.Ordinal)) return false;
            }
            return true;
        }

        private AIResponse FallbackResponse(AIResponse failed, string providerName)
        {
            string fallbackHint = failed.Text;

            if (fallbackHint.Contains("không tìm thấy") || fallbackHint.Contains("no result")
                || fallbackHint.Contains("không có dữ liệu"))
            {
                return new AIResponse
                {
                    Success = true,
                    Text = $"Tôi đã tra cứu nhưng không tìm thấy dữ liệu phù hợp với yêu cầu của bạn.\n\n" +
                           $"Gợi ý: Bạn có thể thử tìm kiếm với từ khóa khác, hoặc kiểm tra lại dữ liệu trong hệ thống.",
                    Provider = providerName,
                    LatencyMs = failed.LatencyMs
                };
            }

            if (failed.Error.Contains("unavailable") || failed.Error.Contains("timeout")
                || failed.Error.Contains("không khả dụng") || failed.Error.Contains("timeout"))
            {
                return new AIResponse
                {
                    Success = true,
                    Text = $"Dịch vụ AI tạm thời gặp sự cố. Vui lòng thử lại sau.",
                    Provider = providerName,
                    LatencyMs = failed.LatencyMs
                };
            }

            return new AIResponse
            {
                Success = false,
                Error = $"Lỗi từ {providerName}: {failed.Error}",
                Provider = providerName,
                LatencyMs = failed.LatencyMs
            };
        }

        private static ConversationHistory CloneHistory(ConversationHistory original)
        {
            var clone = new ConversationHistory();
            if (original?.Messages == null) return clone;
            foreach (var msg in original.Messages)
            {
                clone.Messages.Add(new ConversationMessage { Role = msg.Role, Content = msg.Content });
            }
            return clone;
        }

        private static string BuildSystemPrompt(List<ToolDefinition> definitions, string ragContext)
        {
            var sb = new StringBuilder();

            // Role definition
            sb.AppendLine("Bạn là trợ lý AI của hệ thống quản lý phân phối báo/tạp chí DATNWF.");
            sb.AppendLine("Trả lời bằng tiếng Việt, ngắn gọn, chính xác và thân thiện.");
            sb.AppendLine();

            // Business schema - dùng single source từ BusinessContextProvider
            sb.AppendLine("NGỮ CẢNH NGHIỆP VỤ:");
            sb.AppendLine(BusinessContextProvider.GetSchemaDescription());
            sb.AppendLine();

            // Giải thích rõ ràng về P_KT và P_PH
            sb.AppendLine("GIẢI THÍCH LOẠI KHÁCH HÀNG:");
            sb.AppendLine("- P_PH = 1: Khách hàng loại Phát hành (đại lý báo)");
            sb.AppendLine("- P_KT = 1: Khách hàng loại Kỹ thuật (kỹ thuật)");
            sb.AppendLine("- P_PH = 1 VÀ P_KT = 1: Khách hàng thuộc cả hai loại");
            sb.AppendLine("→ Khi user hỏi 'số lượng khách hàng P_KT', dùng SearchCustomers với loaiKh='P_KT'.");
            sb.AppendLine("→ Khi user hỏi 'khách hàng loại P_PH', dùng SearchCustomers với loaiKh='P_PH'.");
            sb.AppendLine("→ Khi user hỏi 'khách hàng phát hành/kỹ thuật', hỏi rõ user muốn loại nào.");
            sb.AppendLine();

            // RAG context (if any)
            if (!string.IsNullOrWhiteSpace(ragContext))
            {
                sb.AppendLine(ragContext);
                sb.AppendLine();
            }

            // Tool definitions
            sb.AppendLine("CÔNG CỤ TRUY VẤN DỮ LIỆU:");
            sb.AppendLine("(Chỉ dùng khi người dùng hỏi về dữ liệu cụ thể)");
            sb.AppendLine();

            foreach (var def in definitions)
            {
                sb.AppendLine($"TOOL: {def.Name}");
                sb.AppendLine($"Mô tả: {def.Description}");
                if (def.Parameters.Count > 0)
                {
                    sb.AppendLine("Tham số (JSON):");
                    foreach (var param in def.Parameters)
                    {
                        sb.AppendLine($"  - {param.Key}: {param.Value.Type} — {param.Value.Description}");
                    }
                }
                sb.AppendLine();
            }

            // Few-shot examples
            sb.AppendLine("VÍ DỤ (FEW-SHOT):");
            sb.AppendLine();
            sb.AppendLine("Ví dụ 1 — Tìm kiếm khách hàng:");
            sb.AppendLine("  User: Tìm khách hàng tên Nguyễn Văn A");
            sb.AppendLine("  Assistant: {\"tool_call\":{\"tool\":\"SearchCustomers\",\"params\":{\"keyword\":\"Nguyễn Văn A\"}}}");
            sb.AppendLine();
            sb.AppendLine("Ví dụ 2 — Đếm theo loại P_KT:");
            sb.AppendLine("  User: Khách hàng loại P_KT có bao nhiêu?");
            sb.AppendLine("  Assistant: {\"tool_call\":{\"tool\":\"SearchCustomers\",\"params\":{\"loaiKh\":\"P_KT\"}}}");
            sb.AppendLine();
            sb.AppendLine("Ví dụ 3 — Xem tồn kho:");
            sb.AppendLine("  User: Xem tồn kho báo Thanh Niên");
            sb.AppendLine("  Assistant: {\"tool_call\":{\"tool\":\"GetInventory\",\"params\":{\"tenBao\":\"Thanh Niên\"}}}");
            sb.AppendLine();
            sb.AppendLine("Ví dụ 4 — Không tìm thấy:");
            sb.AppendLine("  Tool trả: Không tìm thấy khách hàng nào.");
            sb.AppendLine("  Assistant: Hiện tại hệ thống không có khách hàng nào phù hợp với từ khóa bạn tìm kiếm. Bạn có thể thử tìm với tên khác hoặc kiểm tra danh sách khách hàng trong mục 'Khách hàng'.");
            sb.AppendLine();
            sb.AppendLine("Ví dụ 5 — Câu hỏi ngoài nghiệp vụ:");
            sb.AppendLine("  User: Trời hôm nay mưa không?");
            sb.AppendLine("  Assistant: Xin lỗi, tôi chỉ có thể hỗ trợ các câu hỏi liên quan đến hệ thống quản lý phân phối báo/tạp chí DATNWF. Bạn cần tôi giúp gì về khách hàng, báo, hóa đơn, tồn kho hay thống kê không?");
            sb.AppendLine();

            // Rules
            sb.AppendLine("QUY TẮC QUAN TRỌNG:");
            sb.AppendLine("1. KHI NGƯỜI DÙNG HỎI VỀ SỐ LƯỢNG/ĐẾM: Phải gọi tool ngay. KHÔNG ĐƯỢC tự tính/ước lượng số.");
            sb.AppendLine("2. KHÔNG BAO GIỜ trả lời bằng JSON cho người dùng. Luôn diễn giải kết quả tool thành câu tự nhiên.");
            sb.AppendLine("3. Nếu không biết câu trả lời, nói thẳng: 'Tôi không biết' hoặc 'Tôi không có thông tin này'. Không bịa đặt.");
            sb.AppendLine("4. Số tiền luôn kèm đơn vị 'đ' (VD: 1.500.000đ).");
            sb.AppendLine("5. Nếu câu hỏi ngoài nghiệp vụ, trả lời lịch sự và hướng về hệ thống DATNWF.");
            sb.AppendLine("6. KHÔNG BAO GIỜ dùng định dạng markdown: KHÔNG **bold**, KHÔNG *italic*, KHÔNG # heading, KHÔNG - bullet. Chỉ dùng văn bản thuần túy.");

            return sb.ToString();
        }

        private static string FormatToolCallForHistory(ToolCallRequest call)
        {
            var paramJson = Newtonsoft.Json.JsonConvert.SerializeObject(call.Parameters);
            return $"[TOOL_CALL] Gọi công cụ: {call.ToolName} với tham số: {paramJson}";
        }

        private static string FormatToolResultForHistory(string toolName, ToolResult result)
        {
            if (result.Success)
                return $"[TOOL_RESULT] Kết quả từ {toolName}:\n{StripTechnicalArtifacts(result.Output)}";
            return $"[TOOL_RESULT] Lỗi khi gọi {toolName}: {result.Error}";
        }

        private static string StripTechnicalArtifacts(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            // Strip HTML tags like <span style="...">...</span>
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<[^>]+>", "");

            // Strip CSS style attributes like style="color:red; ..."
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"style\s*=\s*[""'][^""']*[""']", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Strip triple-backtick code blocks (markdown code fences)
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"```[\s\S]*?```", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Strip single-line code blocks (backtick-wrapped)
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"`[^`]+`", m => m.Value.Trim('`'));

            // Strip markdown bold: **text** or __text__
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"\*\*(.+?)\*\*", "$1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"__(.+?)__", "$1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Strip markdown italic: *text* or _text_
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"\*(.+?)\*", "$1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"_(.+?)_", "$1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Strip markdown headers: # Header
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"^#{1,6}\s+", "", System.Text.RegularExpressions.RegexOptions.Multiline);

            // Strip markdown bullet points: - item or * item
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"^[\-\*]\s+", "", System.Text.RegularExpressions.RegexOptions.Multiline);

            // Strip markdown numbered lists: 1. item
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"^\d+\.\s+", "", System.Text.RegularExpressions.RegexOptions.Multiline);

            // Strip remaining JSON-like fragments (when AI accidentally includes tool call syntax in text)
            text = System.Text.RegularExpressions.Regex.Replace(text,
                @"\{[^}]{0,20}tool[^}]{0,50}\}", " ");

            // Normalize excessive newlines (more than 2) into max 2
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\n{3,}", "\n\n");

            // Normalize excessive spaces
            text = System.Text.RegularExpressions.Regex.Replace(text, @"  +", " ");

            return text.Trim();
        }

        public async Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
        {
            return _mode switch
            {
                AIProviderMode.GeminiOnly => await GetGemini().HealthCheckAsync(cancellationToken),
                AIProviderMode.OllamaOnly => await GetOllama().HealthCheckAsync(cancellationToken),
                AIProviderMode.ClaudeOnly => await GetClaude().HealthCheckAsync(cancellationToken),
                AIProviderMode.Hybrid => await GetGemini().HealthCheckAsync(cancellationToken)
                                      || await GetClaude().HealthCheckAsync(cancellationToken)
                                      || await GetOllama().HealthCheckAsync(cancellationToken),
                _ => false
            };
        }
    }
}
