using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class HuggingFaceService : IHuggingFaceService
    {
        private readonly HttpClient _httpClient;
        private readonly HuggingFaceOptions _options;

        public HuggingFaceService(HttpClient httpClient, IOptions<HuggingFaceOptions> opts)
        {
            _httpClient = httpClient;
            _options = opts.Value;
        }

        public async Task<string> GetResponseAsync(string input, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException("HuggingFace API key is not configured.");

            var model = string.IsNullOrWhiteSpace(_options.Model) ? "gpt2" : _options.Model.Trim();
            var endpoint = $"https://api-inference.huggingface.co/models/{model}";

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var payload = new
            {
                inputs = input,
                parameters = new { max_new_tokens = 256 }
            };

            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var resp = await _httpClient.SendAsync(request, cancellationToken);
            var content = await resp.Content.ReadAsStringAsync(cancellationToken);

            if (!resp.IsSuccessStatusCode)
            {
                // return the error body for debugging; consider logging and returning a generic message in production
                return $"[HuggingFace Error: {resp.StatusCode}] {content}";
            }

            // Try to parse common HF response shapes:
            // - Array with generated_text: [{"generated_text":"..."}]
            // - Plain text (depends on model/endpoint)
            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                {
                    var first = doc.RootElement[0];

                    if (first.TryGetProperty("generated_text", out var gen))
                        return gen.GetString() ?? string.Empty;

                    // some models return "generated_text" nested differently; fallback to raw
                    return first.ToString();
                }

                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("generated_text", out var single))
                {
                    return single.GetString() ?? string.Empty;
                }

                // fallback: return full JSON if not matching expected shapes
                return content;
            }
            catch (JsonException)
            {
                // Not JSON — return raw text
                return content;
            }
        }
    }
}