using tests_cache_dotnet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MyMemoryCache>();
builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddSingleton<GetAgeEstimateService>();
builder.Services.AddSingleton((provider) =>
{
  var memoryCache = provider.GetService<MyMemoryCache>();
  var getAgeEstimateService = provider.GetService<GetAgeEstimateService>();

  if(memoryCache == null || getAgeEstimateService == null)
  {
    throw new Exception("Resolved Faild");
  }

  return new GetCacheWithLock<AgeEstimateWithMemoryBomb, GetAgeEstimateParameters>(memoryCache, getAgeEstimateService);
});
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
