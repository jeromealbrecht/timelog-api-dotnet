using Microsoft.AspNetCore.Mvc;
using TimeLog.Services;

namespace TimeLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OpenAIController : ControllerBase
{
    private readonly OpenAIService _openAIService;

    public OpenAIController(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> GetChatCompletion([FromBody] ChatRequest request)
    {
        try
        {
            var response = await _openAIService.GetChatCompletionAsync(request.Prompt);
            return Ok(new { response });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class ChatRequest
{
    public string Prompt { get; set; } = string.Empty;
} 