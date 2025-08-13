public class OpenAiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public OpenAiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetCompletionAsync(string message, string customerId)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var systemPrompt = $"Du är en hjälpsam AI-assistent för företaget med ID {customerId}.";

        var payload = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = message }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", payload);
        var json = await response.Content.ReadFromJsonAsync<JsonNode>();
        return json?["choices"]?[0]?["message"]?["content"]?.ToString() ?? "Inget svar från AI.";
    }
}
