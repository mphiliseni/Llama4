using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LlamaChat.Services
{
    public class TogetherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "YOUR_API_KEY";
        private const string ApiUrl = "https://api.together.xyz/v1/chat/completions";

        public TogetherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetChatResponseAsync (string UserMessage)
        {
            var requestBody = new 
            {
                model = "meta-llama/Llama-4-Maverick-17B-128E-Instruct-FP8",
                messages = new[]
                {
                    new { role = "user", content = UserMessage }
                }
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiUrl, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error: {response.StatusCode} - {jsonResponse}";
            }

            // Correctly parse only the message content
            var json = JObject.Parse(jsonResponse);
            var messageContent = json["choices"]?[0]?["message"]?["content"]?.ToString();

            return messageContent ?? "No content returned.";
        }
    }
}