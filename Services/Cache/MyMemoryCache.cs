using Microsoft.Extensions.Caching.Memory;
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