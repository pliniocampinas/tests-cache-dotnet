
using Microsoft.Extensions.Caching.Memory;

public interface ICachableService<T,P>
{
  Task<T> GetFromSource(P parameters);
  string GetKey(P parameters);
  MemoryCacheEntryOptions GetEntryOptions();
}