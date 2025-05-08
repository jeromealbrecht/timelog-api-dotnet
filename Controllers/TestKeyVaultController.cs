using Microsoft.AspNetCore.Mvc;
using TimeLog.Services;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace TimeLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestKeyVaultController : ControllerBase
    {
        private readonly KeyVaultService _keyVaultService;

        public TestKeyVaultController(KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
        }

        [HttpGet("secret")]
        public async Task<IActionResult> GetSecret()
        {
            var secret = await _keyVaultService.GetSecretAsync("JwtSecretKey");
            return Ok(new { secret });
        }
    }
}
