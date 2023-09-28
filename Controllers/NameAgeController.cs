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
    public async Task<AgeEstimate?> Get([FromServices]IMemoryCache cache, string name)
    {
        var estimate = await cache.GetOrCreateAsync("key-"+name, item =>
        {
            item.SlidingExpiration = TimeSpan.FromSeconds(10);
            var estimate =  GetEstimate(name);
            return estimate;
        });
        
        return estimate;
    }

    private async Task<AgeEstimate?> GetEstimate(string name)
    {
        Console.WriteLine("Calling API for name " + name);
        HttpResponseMessage response = await httpClient.GetAsync("?name="+name);
        response.EnsureSuccessStatusCode();
        var estimate = await response.Content.ReadFromJsonAsync<AgeEstimate?>();
        return estimate;
    }
}
