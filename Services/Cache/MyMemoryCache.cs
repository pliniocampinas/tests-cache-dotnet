using Microsoft.Extensions.Caching.Memory;

namespace tests_cache_dotnet.Services.Cache;

public class MyMemoryCache
{
  public MemoryCache Cache { get; private set; }

  public MyMemoryCache()
  {
    Cache = new MemoryCache(new MemoryCacheOptions
    {
      SizeLimit = 1_000
    });
  }
}