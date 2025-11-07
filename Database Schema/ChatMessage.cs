using WebApplication1.Database_Schema.Enums;

namespace WebApplication1.Database_Schema
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public Guid ChatSessionId { get; set; }
        public ChatRole Role { get; set; }                         // User, Assistant, System
        public string Content { get; set; } = default!;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public int? PromptTokens { get; set; }
        public int? CompletionTokens { get; set; }
        public string? ReferenceJson { get; set; }                 // e.g., span refs, citations

        public ChatSession ChatSession { get; set; } = default!;
    }
}
