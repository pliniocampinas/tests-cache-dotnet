using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace tests_cache_dotnet.Controllers;

[ApiController]
[Route("")]
public class NameAgeController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://api.agify.io"),
    };

    private readonly ILogger<NameAgeController> _logger;

    public NameAgeController(ILogger<NameAgeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<AgeEstimate?> Get([FromServices]MyMemoryCache myCache, string name)
    {
        // Create multiple records for each name.
        var currentSecond = DateTime.Now.Second;
        var key = "key-" + name + currentSecond.ToString();

        // Test 1: Compact cache every 10 seconds or so.
        // if(currentSecond % 10 == 0)
        // {
        //     Console.WriteLine("Compacting");
        //     myCache.Cache.Compact(0.5);
        // }

        var estimateWithBomb = await myCache.Cache.GetOrCreateAsync(key, item =>
        {
            item.SlidingExpiration = TimeSpan.FromSeconds(30);
            item.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            // Test 2: Set bigger size to limit cache entires.
            item.Size = 100;
            var estimate =  GetEstimate(name);
            return estimate;
        });
        
        return estimateWithBomb?.Estimate;
    }

    private async Task<AgeEstimateWithMemoryBomb?> GetEstimate(string name)
    {
        Console.WriteLine("[" + DateTime.Now.ToString() + "]" +  "Calling API for name " + name);
        await Task.Delay(40);
        // HttpResponseMessage response = await httpClient.GetAsync("?name="+name);
        // response.EnsureSuccessStatusCode();
        var estimate = new AgeEstimate(); // await response.Content.ReadFromJsonAsync<AgeEstimate?>();
        // Create a memory bomb with strings and nested objects.
        var memoryBomb = new AgeEstimateWithMemoryBomb(estimate, 1024 * 1024, 20);
        return memoryBomb;
    }
}
