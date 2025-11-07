namespace WebApplication1.Database_Schema
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; } = default!;
        public string Title { get; set; } = "New session";
        public Guid? UploadedFileId { get; set; }                 // link a doc if relevant
        public Guid? TranscriptionJobId { get; set; }             // link a job if relevant
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ArchivedUtc { get; set; }

        public ApplicationUser Owner { get; set; } = default!;
        public UploadedFile? UploadedFile { get; set; }
        public TranscriptionJob? TranscriptionJob { get; set; }
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<SummaryRequest> SummaryRequests { get; set; } = new List<SummaryRequest>();
    }
}
