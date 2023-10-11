using Microsoft.AspNetCore.Mvc;
using tests_cache_dotnet.Models;

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
    public async Task<AgeEstimate?> Get([FromServices]GetCacheWithLock<AgeEstimateWithMemoryBomb, GetAgeEstimateParameters> getAgeEstimateService, string name, string cacheKey)
    {     
        var estimateWithMemoryBomb = await getAgeEstimateService.GetCached(new GetAgeEstimateParameters()
        {
            Name = name, 
            Key = cacheKey
        });

        return estimateWithMemoryBomb.Estimate;
    }
}
