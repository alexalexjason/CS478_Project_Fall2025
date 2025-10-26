using WebApplication1.Database_Schema.Enums;

namespace WebApplication1.Database_Schema
{
    public class UploadedFile
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; } = default!;          // AspNetUsers.Id
        public string FileName { get; set; } = default!;
        public string MimeType { get; set; } = default!;
        public long SizeBytes { get; set; }
        public string Sha256 { get; set; } = default!;            // integrity / dedupe
        public string StoragePath { get; set; } = default!;       // disk or blob path
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public FileStatus Status { get; set; } = FileStatus.Ready; // Ready, Deleted, Quarantined
        public string? Notes { get; set; }

        public ApplicationUser Owner { get; set; } = default!;
        public ICollection<TranscriptionJob> Jobs { get; set; } = new List<TranscriptionJob>();
    }
}
