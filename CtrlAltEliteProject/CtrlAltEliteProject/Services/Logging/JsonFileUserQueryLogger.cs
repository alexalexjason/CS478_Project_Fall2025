using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CtrlAltEliteProject.Services.Logging
{
    public class JsonFileUserQueryLogger : IUserQueryLogger
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<JsonFileUserQueryLogger> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public JsonFileUserQueryLogger(IWebHostEnvironment env, ILogger<JsonFileUserQueryLogger> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> LogAsync(InteractionRecord record)
        {
            try
            {
                var folder = Path.Combine(_env.ContentRootPath, "Output");
                Directory.CreateDirectory(folder);

                var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);
                var fileName = $"interaction_{DateTime.UtcNow:yyyyMMddHHmmss}_{shortGuid}.json";
                var path = Path.Combine(folder, fileName);

                var json = JsonSerializer.Serialize(record, _jsonOptions);
                await File.WriteAllTextAsync(path, json, Encoding.UTF8).ConfigureAwait(false);

                return path;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write interaction JSON file.");
                return string.Empty;
            }
        }
    }
}