namespace WebApplication1.Database_Schema
{
    public class SummaryRequest
    {
        public Guid Id { get; set; }
        public Guid ChatSessionId { get; set; }
        public Guid? TranscriptionJobId { get; set; }              // summarize a full job
        public string Prompt { get; set; } = "Summarize the document.";
        public string? ConstraintsJson { get; set; }               // length, style, bullets
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public ChatSession ChatSession { get; set; } = default!;
        public TranscriptionJob? TranscriptionJob { get; set; }
        public SummaryResult? Result { get; set; }
    }
}
