namespace CtrlAltEliteProject.Services
{
    using Microsoft.Extensions.Configuration;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _http;

        public GeminiService(IConfiguration config, HttpClient http)
        {
            // Reads from appsettings.json
            _apiKey = config["GoogleGemini:ApiKey"];
            _http = http;
        }

        public async Task<string> SendMessageAsync(string userInput)
        {
            try
            {
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

                var payload = new
                {
                    contents = new[]
                    {
                        new {
                            parts = new[]
                            {
                                new { text = userInput }
                            }
                        }
                    }
                };

                var response = await _http.PostAsJsonAsync(url, payload);

                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.StatusCode}";

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var resultText = doc.RootElement
                                    .GetProperty("candidates")[0]
                                    .GetProperty("content")
                                    .GetProperty("parts")[0]
                                    .GetProperty("text")
                                    .GetString();

                return resultText ?? "[EMPTY RESPONSE]";
            }
            catch (System.Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
