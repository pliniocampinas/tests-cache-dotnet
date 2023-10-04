public class TimedHostedService : IHostedService, IDisposable
{
  private int executionCount = 0;
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
    var count = Interlocked.Increment(ref executionCount);

    if(_memoryCache is not null)
    {
      _memoryCache.Cache.Compact(0.5);
      _logger.LogInformation("Compacted cache.");
      _logger.LogInformation("Entries cached: {count}", _memoryCache.Cache.Count);
    }

    _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
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