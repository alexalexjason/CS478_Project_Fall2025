using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CtrlAltEliteProject.Models;

namespace CtrlAltEliteProject.Services.Logging
{
    public class UserQueryLogger : IUserQueryLogger
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public UserQueryLogger(IWebHostEnvironment environment)
        {
            var logsDirectory = Path.Combine(environment.ContentRootPath, "Logs");
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            _logFilePath = Path.Combine(logsDirectory, "user_queries.json");
        }

        public async Task LogAsync(InteractionRecord record)
        {
            lock (_lockObject)
            {
                var records = GetRecordsSync();
                records.Add(record);

                var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_logFilePath, json);
            }

            await Task.CompletedTask;
        }

        public async Task<List<InteractionRecord>> GetAllRecordsAsync()
        {
            return await Task.FromResult(GetRecordsSync());
        }

        private List<InteractionRecord> GetRecordsSync()
        {
            lock (_lockObject)
            {
                if (!File.Exists(_logFilePath))
                {
                    return new List<InteractionRecord>();
                }

                try
                {
                    var json = File.ReadAllText(_logFilePath);
                    return JsonSerializer.Deserialize<List<InteractionRecord>>(json) ?? new List<InteractionRecord>();
                }
                catch
                {
                    return new List<InteractionRecord>();
                }
            }
        }
    }
}