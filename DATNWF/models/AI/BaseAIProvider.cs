using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DATNWF.Models.AI
{
    /// <summary>
    /// Shared helpers for AI providers: parse tool-call JSON out of model output.
    /// All 3 providers (Gemini, Claude, Ollama) use the same {tool_call:{tool,params}} format.
    /// </summary>
    public abstract class BaseAIProvider
    {
        private static readonly Regex HtmlTagRegex = new Regex(
            @"<[^>]+>",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex StripColorStyleRegex = new Regex(
            @"style\s*=\s*[""'][^""']*[""']",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Try to extract a {tool_call:{tool,params}} structure from the model output.
        /// Handles: raw JSON, HTML-wrapped JSON, AI text explanations surrounding JSON.
        /// Returns null if the model produced a normal reply (no tool call wanted).
        /// </summary>
        protected static ToolCallRequest TryParseToolCall(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            string cleaned = StripHtmlAndStyles(text.Trim());
            if (string.IsNullOrWhiteSpace(cleaned))
                return null;

            // Strategy 1: Try direct parse first (fastest)
            var direct = TryParseDirect(cleaned);
            if (direct != null) return direct;

            // Strategy 2: Look for JSON block inside text (AI may explain before/after the JSON)
            var inBlock = TryParseJsonBlock(cleaned);
            if (inBlock != null) return inBlock;

            return null;
        }

        private static string StripHtmlAndStyles(string text)
        {
            text = HtmlTagRegex.Replace(text, "");
            text = StripColorStyleRegex.Replace(text, "");
            return text.Trim();
        }

        /// <summary>
        /// Try parsing the string directly as a JSON object.
        /// </summary>
        private static ToolCallRequest TryParseDirect(string text)
        {
            try
            {
                var obj = JObject.Parse(text);
                return ExtractToolCall(obj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Look for a JSON object containing "tool_call" anywhere inside the text.
        /// Handles cases like: "Here is the result: {"tool_call":{...}} - do you need anything else?"
        /// </summary>
        private static ToolCallRequest TryParseJsonBlock(string text)
        {
            // Try to find a JSON object anywhere in the string using brace matching
            int jsonStart = -1;
            int braceDepth = 0;
            bool inString = false;
            bool escaped = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    escaped = true;
                    continue;
                }

                if (c == '"' && !escaped)
                {
                    inString = !inString;
                    continue;
                }

                if (inString) continue;

                if (c == '{')
                {
                    if (braceDepth == 0) jsonStart = i;
                    braceDepth++;
                }
                else if (c == '}')
                {
                    braceDepth--;
                    if (braceDepth == 0 && jsonStart >= 0)
                    {
                        string jsonSubstr = text.Substring(jsonStart, i - jsonStart + 1);
                        try
                        {
                            var obj = JObject.Parse(jsonSubstr);
                            var result = ExtractToolCall(obj);
                            if (result != null) return result;
                        }
                        catch
                        {
                            // Not valid JSON, keep searching
                        }
                        jsonStart = -1;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Extract ToolCallRequest from a JObject, handling nested structures.
        /// </summary>
        private static ToolCallRequest ExtractToolCall(JObject obj)
        {
            JToken toolCallToken = null;

            // Handle: {"tool_call": {"tool": "X", "params": {...}}}
            if (obj.TryGetValue("tool_call", StringComparison.OrdinalIgnoreCase, out var tcDirect))
            {
                toolCallToken = tcDirect;
            }
            // Handle: {"tool": "X", "params": {...}} (direct)
            else if (obj.TryGetValue("tool", StringComparison.OrdinalIgnoreCase, out _))
            {
                toolCallToken = obj;
            }

            if (toolCallToken == null || toolCallToken.Type != JTokenType.Object)
                return null;

            var tcObj = (JObject)toolCallToken;
            string toolName = tcObj["tool"]?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(toolName))
                return null;

            var parameters = new System.Collections.Generic.Dictionary<string, object>();

            // Handle: {"tool_call": {"tool": "X", "params": {"a": "b"}}}
            if (tcObj["params"] is JObject paramsObj)
            {
                foreach (var prop in paramsObj.Properties())
                {
                    parameters[prop.Name] = prop.Value.ToString();
                }
            }
            // Handle: {"tool": "X", "a": "b", "c": 1} (flat)
            else
            {
                foreach (var prop in tcObj.Properties())
                {
                    if (prop.Name.Equals("tool", StringComparison.OrdinalIgnoreCase))
                        continue;
                    parameters[prop.Name] = prop.Value.ToString();
                }
            }

            return new ToolCallRequest { ToolName = toolName, Parameters = parameters };
        }
    }
}
