using WebApplication1.Database_Schema.Enums;

namespace WebApplication1.Database_Schema
{
    public class TranscriptionJob
    {
        public Guid Id { get; set; }
        public Guid UploadedFileId { get; set; }
        public string Engine { get; set; } = "Vosk";
        public string? Language { get; set; }                     // e.g., "en-US"
        public TranscriptionStatus Status { get; set; } = TranscriptionStatus.Pending;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? StartedUtc { get; set; }
        public DateTime? CompletedUtc { get; set; }
        public double? AudioDurationSec { get; set; }
        public string? Error { get; set; }

        public UploadedFile UploadedFile { get; set; } = default!;
        public ICollection<TranscriptSegment> Segments { get; set; } = new List<TranscriptSegment>();
    }
}
