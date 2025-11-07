namespace WebApplication1.Database_Schema
{
    public class TranscriptSegment
    {
        public long Id { get; set; }
        public Guid TranscriptionJobId { get; set; }
        public double StartMs { get; set; }
        public double EndMs { get; set; }
        public string Text { get; set; } = default!;
        public float? Confidence { get; set; }

        public TranscriptionJob TranscriptionJob { get; set; } = default!;
    }
}
