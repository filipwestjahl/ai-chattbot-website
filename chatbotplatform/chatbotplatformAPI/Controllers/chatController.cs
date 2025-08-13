[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly OpenAiService _openAi;

    public ChatController(OpenAiService openAi)
    {
        _openAi = openAi;
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage([FromBody] ChatRequest request)
    {
        var reply = await _openAi.GetCompletionAsync(request.Message, request.CustomerId);
        return Ok(new ChatResponse { Reply = reply });
    }
}
