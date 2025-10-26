namespace WebApplication1.Database_Schema
{
    public class SummaryResult
    {
        public Guid Id { get; set; }
        public Guid SummaryRequestId { get; set; }
        public string Output { get; set; } = default!;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public SummaryRequest SummaryRequest { get; set; } = default!;
    }
}
