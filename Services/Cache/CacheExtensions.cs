
public static class CacheExtensions
{
  public static void AddCachedService<S,T,P>(this IServiceCollection services) where S : ICachableService<T,P>
  {
    services.AddSingleton((provider) =>
    {
      var memoryCache = provider.GetService<MyMemoryCache>() ?? throw new Exception($"Failed to resolve MemoryCache");
      var service = provider.GetService<S>() ?? throw new Exception($"Failed to resolve {typeof(S).FullName}");

      return new GetCacheWithLock<T, P>(memoryCache, service);
    });
  }
}