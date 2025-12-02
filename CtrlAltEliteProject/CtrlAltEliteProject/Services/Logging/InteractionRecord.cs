using System;
using System.Collections.Generic;

namespace CtrlAltEliteProject.Services.Logging
{
    public class InteractionRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TimestampUtc { get; set; } = DateTime.UtcNow.ToString("o");
        public string? UserId { get; set; }
        public string Source { get; set; } = "other"; // "pdf", "audio", "chat", etc.
        public object? Input { get; set; }
        public string? Output { get; set; }
        public Dictionary<string, object?>? Metadata { get; set; }
    }
}