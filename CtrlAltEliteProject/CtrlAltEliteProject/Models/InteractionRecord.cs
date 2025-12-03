using System;
using System.Collections.Generic;

namespace CtrlAltEliteProject.Models
{
    public class InteractionRecord
    {
        public string? UserId { get; set; }
        public string? Source { get; set; }
        public object? Input { get; set; }
        public object? Output { get; set; }
        public Dictionary<string, object?>? Metadata { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}