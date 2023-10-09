using Microsoft.Extensions.Caching.Memory;
using tests_cache_dotnet;
public class GetAgeEstimateService
{
  private static HttpClient _httpClient = new()
  {
    BaseAddress = new Uri("https://api.agify.io"),
  };

  private readonly MemoryCache Cache;
  private readonly SemaphoreSlim _cacheLock;

  public GetAgeEstimateService(MyMemoryCache cache)
  {
    Cache = cache.Cache;
    _cacheLock = new SemaphoreSlim(1);
  }

  public async Task<AgeEstimate?> GetAgeByName(string name, string key)
  {
    var cacheKey = "key-" + name + "-" + key;

    var cachedEntry = Cache.Get(key) as AgeEstimateWithMemoryBomb;
    if (cachedEntry != null)
    {
      return cachedEntry.Estimate;
    }

    // Acquire the cache lock to prevent cache stampede
    await _cacheLock.WaitAsync();

    try
    {
      // Check if another request has already updated the cache
      cachedEntry = Cache.Get(cacheKey) as AgeEstimateWithMemoryBomb;
      if (cachedEntry != null)
      {
        return cachedEntry.Estimate;
      }

      // Retrieve from the data source
      var estimateWithMemoryBomb = await GetEstimate(name);

      // Cache the entry
      Cache.Set(cacheKey, estimateWithMemoryBomb, new MemoryCacheEntryOptions
      {
        SlidingExpiration = TimeSpan.FromSeconds(10),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
        Size = 1,
      });

      return estimateWithMemoryBomb?.Estimate;
    }
    finally
    {
      // Release the cache lock
      _cacheLock.Release();
    }
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