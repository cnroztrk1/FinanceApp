using FinanceApp.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Api.Controllers
{
    [Route("api/cache")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<CacheController> _logger;

        public CacheController(ICacheService cacheService, ILogger<CacheController> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpPost("clear")]
        public IActionResult ClearCache([FromQuery] int tenantId)
        {
            if (tenantId <= 0)
            {
                return BadRequest("Geçersiz TenantId");
            }

            string cacheKey = $"jobs_{tenantId}";
            _cacheService.Remove(cacheKey);

            string cacheKey1 = $"risk_analysis_{tenantId}";
            _cacheService.Remove(cacheKey1);

            _logger.LogInformation($"Cache temizlendi: {cacheKey}");
            return Ok(new { Message = $"Cache temizlendi: {cacheKey}" });
        }
    }
}
