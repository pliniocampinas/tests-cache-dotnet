using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace tests_cache_dotnet.Controllers;

[ApiController]
[Route("")]
public class NameAgeController : ControllerBase
{
    private readonly ILogger<NameAgeController> _logger;

    public NameAgeController(ILogger<NameAgeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<AgeEstimate?> Get([FromServices]GetAgeEstimateService getAgeEstimateService, string name, string cacheKey)
    {     
        return await getAgeEstimateService.GetAgeByName(name, cacheKey);
    }
}
