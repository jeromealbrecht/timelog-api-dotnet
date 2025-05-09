using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TimeLog.Models;

namespace TimeLog.Services;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAISettings _settings;

    public OpenAIService(HttpClient httpClient, IOptions<OpenAISettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task<string> GetChatCompletionAsync(string prompt)
    {
        var request = new
        {
            model = _settings.Model,
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