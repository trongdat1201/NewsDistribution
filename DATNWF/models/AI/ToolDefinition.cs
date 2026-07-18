using System.Collections.Generic;
using Newtonsoft.Json;

namespace DATNWF.Models.AI
{
    public class ToolParameter
    {
        public string Type { get; set; } = "string";
        public string Description { get; set; } = string.Empty;
    }

    public class ToolDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, ToolParameter> Parameters { get; set; } = new Dictionary<string, ToolParameter>();
    }

    public class ToolCallRequest
    {
        public string ToolName { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }

    public class ToolResult
    {
        public string ToolName { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
