using Microsoft.AspNetCore.Mvc;
using TimeLog.Services;

namespace TimeLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppNotificationController : ControllerBase
    {
        private readonly IWhatsAppNotificationService _whatsAppService;

        public WhatsAppNotificationController(IWhatsAppNotificationService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] WhatsAppNotificationRequest request)
        {
            try
            {
                var result = await _whatsAppService.SendMessageAsync(
                    request.ToNumber,
                    request.ContentSid,
                    request.Variables
                );
                return Ok(new { message = "Notification sent successfully", result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestNotification()
        {
            try
            {
                var testRequest = new WhatsAppNotificationRequest
                {
                    ToNumber = "+33699126183",
                    ContentSid = "HXb5b62575e6e4ff6129ad7c8efe1f983e",
                    Variables = new Dictionary<string, string>
                    {
                        { "1", "Test" },
                        { "2", DateTime.Now.ToString() }
                    }
                };

                var result = await _whatsAppService.SendMessageAsync(
                    testRequest.ToNumber,
                    testRequest.ContentSid,
                    testRequest.Variables
                );
                return Ok(new { message = "Test notification sent successfully", result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class WhatsAppNotificationRequest
    {
        public string ToNumber { get; set; } = string.Empty;
        public string ContentSid { get; set; } = string.Empty;
        public Dictionary<string, string> Variables { get; set; } = new();
    }
} 