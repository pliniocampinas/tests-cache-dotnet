using Microsoft.Extensions.Caching.Memory;
using tests_cache_dotnet.Models;
using tests_cache_dotnet.Services.Cache;

namespace tests_cache_dotnet.Services;

public class GetAgeEstimateService: ICachableService<AgeEstimateWithMemoryBomb, GetAgeEstimateParameters>
{
  private static HttpClient _httpClient = new()
  {
    BaseAddress = new Uri("https://api.agify.io"),
  };

  public async Task<AgeEstimateWithMemoryBomb> GetFromSource(GetAgeEstimateParameters parameters)
  {
    Console.WriteLine("[" + DateTime.Now.ToString() + "]" +  "Calling API for name " + parameters.Name);
    await Task.Delay(40);
    // HttpResponseMessage response = await httpClient.GetAsync("?name="+name);
    // response.EnsureSuccessStatusCode();
    var estimate = new AgeEstimate(); // await response.Content.ReadFromJsonAsync<AgeEstimate?>();
    // Create a memory bomb with strings and nested objects.
    var memoryBomb = new AgeEstimateWithMemoryBomb(estimate, 1024 * 1024, 20);
    return memoryBomb;
  }

  public string GetKey(GetAgeEstimateParameters parameters)
  {
    return "key-" + parameters.Name + "-" + parameters.Key;
  }

  public MemoryCacheEntryOptions GetEntryOptions()
  {
    return new MemoryCacheEntryOptions
    {
      SlidingExpiration = TimeSpan.FromSeconds(10),
      AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
      Size = 1,
    };
  }
}