using tests_cache_dotnet.Services.Cache;

namespace tests_cache_dotnet.Services;

public class TimedHostedService : IHostedService, IDisposable
{
  private readonly ILogger<TimedHostedService> _logger;
  private Timer? _timer = null;
  private MyMemoryCache? _memoryCache = null;

  public TimedHostedService(ILogger<TimedHostedService> logger, MyMemoryCache memoryCache)
  {
    _logger = logger;
    _memoryCache = memoryCache;
  }

  public Task StartAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Timed Hosted Service running.");

    _timer = new Timer(DoWork, null, TimeSpan.Zero,
      TimeSpan.FromSeconds(5));

    return Task.CompletedTask;
  }

  private void DoWork(object? state)
  {
    if(_memoryCache is not null)
    {
      _memoryCache.Cache.Compact(0.5);
    }
  }

  public Task StopAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Timed Hosted Service is stopping.");

    _timer?.Change(Timeout.Infinite, 0);

    return Task.CompletedTask;
  }

  public void Dispose()
  {
    _timer?.Dispose();
  }
}