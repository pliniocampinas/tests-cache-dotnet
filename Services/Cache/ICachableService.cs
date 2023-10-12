
using Microsoft.Extensions.Caching.Memory;

namespace tests_cache_dotnet.Services.Cache;

public interface ICachableService<TEntry,TParameters>
{
  Task<TEntry> GetFromSource(TParameters parameters);
  string GetKey(TParameters parameters);
  MemoryCacheEntryOptions GetEntryOptions();
}