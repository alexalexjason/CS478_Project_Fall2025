using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlAltEliteProject.Services.History
{
    public class HistoryEntry
    {
        public string Id { get; set; } = string.Empty;
        public string TimestampUtc { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty; // "pdf", "audio", "chat"
        public string InputSummary { get; set; } = string.Empty;
        public string? OutputPath { get; set; }
    }

    public interface IUserQueryHistoryService
    {
        /// <summary>
        /// Retrieves all query history entries for a user.
        /// </summary>
        Task<List<HistoryEntry>> GetUserHistoryAsync(string userId);

        /// <summary>
        /// Searches user history by source type.
        /// </summary>
        Task<List<HistoryEntry>> SearchHistoryBySourceAsync(string userId, string source);

        /// <summary>
        /// Deletes a specific history entry.
        /// </summary>
        Task<bool> DeleteHistoryEntryAsync(string userId, string interactionId);
    }
}