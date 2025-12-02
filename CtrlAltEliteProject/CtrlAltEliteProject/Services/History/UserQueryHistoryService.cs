using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CtrlAltEliteProject.Services.History
{
    public class UserQueryHistoryService : IUserQueryHistoryService
    {
        private readonly IWebHostEnvironment _env;

        public UserQueryHistoryService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<HistoryEntry>> GetUserHistoryAsync(string userId)
        {
            var folder = Path.Combine(_env.ContentRootPath, "Output");
            if (!Directory.Exists(folder))
                return new List<HistoryEntry>();

            var entries = new List<HistoryEntry>();

            // Read all interaction JSON files
            var files = Directory.GetFiles(folder, "interaction_*.json")
                .OrderByDescending(f => f)
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file);
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    // Filter by userId
                    var recordUserId = root.TryGetProperty("userId", out var userIdProp)
                        ? userIdProp.GetString()
                        : null;

                    if (recordUserId != userId)
                        continue;

                    var id = root.TryGetProperty("id", out var idProp) ? idProp.GetString() : "";
                    var timestamp = root.TryGetProperty("timestampUtc", out var tsProp) ? tsProp.GetString() : "";
                    var source = root.TryGetProperty("source", out var srcProp) ? srcProp.GetString() : "other";
                    var output = root.TryGetProperty("output", out var outProp) ? outProp.GetString() : null;

                    // Build a summary of the input
                    var inputSummary = GetInputSummary(root, source);

                    entries.Add(new HistoryEntry
                    {
                        Id = id ?? "",
                        TimestampUtc = timestamp ?? "",
                        Source = source ?? "other",
                        InputSummary = inputSummary,
                        OutputPath = output
                    });
                }
                catch
                {
                    // Silently skip malformed files
                }
            }

            return entries;
        }

        public async Task<List<HistoryEntry>> SearchHistoryBySourceAsync(string userId, string source)
        {
            var all = await GetUserHistoryAsync(userId);
            return all.Where(e => e.Source == source).ToList();
        }

        public async Task<bool> DeleteHistoryEntryAsync(string userId, string interactionId)
        {
            var folder = Path.Combine(_env.ContentRootPath, "Output");
            if (!Directory.Exists(folder))
                return false;

            var files = Directory.GetFiles(folder, "interaction_*.json");
            foreach (var file in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file);
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    var recordUserId = root.TryGetProperty("userId", out var userIdProp)
                        ? userIdProp.GetString()
                        : null;
                    var recordId = root.TryGetProperty("id", out var idProp) ? idProp.GetString() : "";

                    if (recordUserId == userId && recordId == interactionId)
                    {
                        File.Delete(file);
                        return true;
                    }
                }
                catch
                {
                    // Silently continue
                }
            }

            return false;
        }

        private string GetInputSummary(JsonElement root, string source)
        {
            try
            {
                if (!root.TryGetProperty("input", out var inputProp))
                    return "(no input)";

                if (source == "pdf" && inputProp.TryGetProperty("fileName", out var fileNameProp))
                {
                    var fileName = fileNameProp.GetString();
                    return $"PDF: {fileName}";
                }

                if (source == "audio" && inputProp.TryGetProperty("transcript", out var transcriptProp))
                {
                    var transcript = transcriptProp.GetString() ?? "";
                    var preview = transcript.Length > 50 ? transcript.Substring(0, 50) + "…" : transcript;
                    return $"Audio: {preview}";
                }

                if (source == "chat" && inputProp.TryGetProperty("query", out var queryProp))
                {
                    var query = queryProp.GetString() ?? "";
                    var preview = query.Length > 50 ? query.Substring(0, 50) + "…" : query;
                    return $"Chat: {preview}";
                }

                return "(no summary)";
            }
            catch
            {
                return "(error reading input)";
            }
        }
    }
}using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CtrlAltEliteProject.Services.History
{
    public class UserQueryHistoryService : IUserQueryHistoryService
    {
        private readonly IWebHostEnvironment _env;

        public UserQueryHistoryService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<HistoryEntry>> GetUserHistoryAsync(string userId)
        {
            var folder = Path.Combine(_env.ContentRootPath, "Output");
            if (!Directory.Exists(folder))
                return new List<HistoryEntry>();

            var entries = new List<HistoryEntry>();

            // Read all interaction JSON files
            var files = Directory.GetFiles(folder, "interaction_*.json")
                .OrderByDescending(f => f)
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file);
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    // Filter by userId
                    var recordUserId = root.TryGetProperty("userId", out var userIdProp)
                        ? userIdProp.GetString()
                        : null;

                    if (recordUserId != userId)
                        continue;

                    var id = root.TryGetProperty("id", out var idProp) ? idProp.GetString() : "";
                    var timestamp = root.TryGetProperty("timestampUtc", out var tsProp) ? tsProp.GetString() : "";
                    var source = root.TryGetProperty("source", out var srcProp) ? srcProp.GetString() : "other";
                    var output = root.TryGetProperty("output", out var outProp) ? outProp.GetString() : null;

                    // Build a summary of the input
                    var inputSummary = GetInputSummary(root, source);

                    entries.Add(new HistoryEntry
                    {
                        Id = id ?? "",
                        TimestampUtc = timestamp ?? "",
                        Source = source ?? "other",
                        InputSummary = inputSummary,
                        OutputPath = output
                    });
                }
                catch
                {
                    // Silently skip malformed files
                }
            }

            return entries;
        }

        public async Task<List<HistoryEntry>> SearchHistoryBySourceAsync(string userId, string source)
        {
            var all = await GetUserHistoryAsync(userId);
            return all.Where(e => e.Source == source).ToList();
        }

        public async Task<bool> DeleteHistoryEntryAsync(string userId, string interactionId)
        {
            var folder = Path.Combine(_env.ContentRootPath, "Output");
            if (!Directory.Exists(folder))
                return false;

            var files = Directory.GetFiles(folder, "interaction_*.json");
            foreach (var file in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file);
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    var recordUserId = root.TryGetProperty("userId", out var userIdProp)
                        ? userIdProp.GetString()
                        : null;
                    var recordId = root.TryGetProperty("id", out var idProp) ? idProp.GetString() : "";

                    if (recordUserId == userId && recordId == interactionId)
                    {
                        File.Delete(file);
                        return true;
                    }
                }
                catch
                {
                    // Silently continue
                }
            }

            return false;
        }

        private string GetInputSummary(JsonElement root, string source)
        {
            try
            {
                if (!root.TryGetProperty("input", out var inputProp))
                    return "(no input)";

                if (source == "pdf" && inputProp.TryGetProperty("fileName", out var fileNameProp))
                {
                    var fileName = fileNameProp.GetString();
                    return $"PDF: {fileName}";
                }

                if (source == "audio" && inputProp.TryGetProperty("transcript", out var transcriptProp))
                {
                    var transcript = transcriptProp.GetString() ?? "";
                    var preview = transcript.Length > 50 ? transcript.Substring(0, 50) + "…" : transcript;
                    return $"Audio: {preview}";
                }

                if (source == "chat" && inputProp.TryGetProperty("query", out var queryProp))
                {
                    var query = queryProp.GetString() ?? "";
                    var preview = query.Length > 50 ? query.Substring(0, 50) + "…" : query;
                    return $"Chat: {preview}";
                }

                return "(no summary)";
            }
            catch
            {
                return "(error reading input)";
            }
        }
    }
}