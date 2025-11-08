using System;
using System.Net.Http; // HTTP requests to Python FastAPI server
using System.Text;
using System.Text.Json; // Serialize and deserialize JSON data
using System.Threading.Tasks;

namespace MistralChatbotClient
{
    class MistralConsole
    {
        // JSON request for Python API
        public class Request
        {
            public string prompt { get; set; } // User entry
        }

        // JSON response from Python API
        public class Response
        {
            public string response { get; set; } // AI response
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Mistral Chatbot Console Client");
            Console.WriteLine("Type 'exit' to quit.\n");

            // Make requests to Python server - timeout if no response
            using HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10) // Large time window for testing and because it's done locally
            };

            string url = "http://127.0.0.1:8000/generate"; // Address

            // Main loop 
            while (true)
            {
                Console.Write("You: ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input.ToLower() == "exit") break;

                try
                {
                    // Serialize request
                    var requestData = new Request { prompt = input };
                    string jsonString = JsonSerializer.Serialize(requestData);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    // Exception if failure 
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<Response>(responseBody);

                    Console.WriteLine("Mistral: " + responseData?.response + "\n");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out. The model may be taking too long to respond.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\n");
                }
            }

            Console.WriteLine("Exiting the Mistral Chatbot Console Client...");
        }
    }
}

