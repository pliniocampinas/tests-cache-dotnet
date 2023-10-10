using Microsoft.Extensions.Caching.Memory;
public class GetCacheWithLock<T,P> 
{
  private readonly MemoryCache Cache;
  private readonly SemaphoreSlim _cacheLock;
  private readonly ICachableService<T, P> _service;

  public GetCacheWithLock(MyMemoryCache cache, ICachableService<T,P> service)
  {
    Cache = cache.Cache;
    _cacheLock = new SemaphoreSlim(1);
    _service = service;
  }

  public async Task<T> GetCached(P parameters)
  {
    var cacheKey = _service.GetKey(parameters);

    var cachedEntry = Cache.Get<T>(cacheKey);
    if (cachedEntry != null)
    {
      return cachedEntry;
    }

    // Acquire the cache lock to prevent cache stampede
    await _cacheLock.WaitAsync();

    try
    {
      // Check if another request has already updated the cache
      cachedEntry = Cache.Get<T>(cacheKey);
      if (cachedEntry != null)
      {
        return cachedEntry;
      }

      // Retrieve from the data source
      var entryFromSource = await _service.GetFromSource(parameters);

      // Cache the entry
      Cache.Set(cacheKey, entryFromSource, _service.GetEntryOptions());

      return entryFromSource;
    }
    finally
    {
      // Release the cache lock
      _cacheLock.Release();
    }
  }
}