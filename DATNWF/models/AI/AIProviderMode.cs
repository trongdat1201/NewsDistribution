namespace DATNWF.Models.AI
{
    /// <summary>
    /// Selector for which AI provider the hybrid orchestrator should run.
    /// </summary>
    public enum AIProviderMode
    {
        GeminiOnly = 0,
        OllamaOnly = 1,
        Hybrid = 2,
        ClaudeOnly = 3
    }
}
