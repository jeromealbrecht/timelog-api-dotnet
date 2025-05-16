using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TimeLog.Services;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://api.openai.com/v1/";
    private readonly string _model = "gpt-4o";

    public OpenAIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? string.Empty;
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        Console.WriteLine("API KEY: " + _apiKey); // Pour debug
    }

    public async Task<string> GetChatCompletionAsync(string prompt)
    {
        var request = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("chat/completions", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"URL appelée : {_httpClient.BaseAddress}chat/completions");
            Console.WriteLine($"Code retour : {response.StatusCode}");
            Console.WriteLine($"Réponse : {responseContent}");
            throw new Exception($"Erreur OpenAI : {response.StatusCode} - {responseContent}");
        }

        var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
        return responseObject.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
    }
} 