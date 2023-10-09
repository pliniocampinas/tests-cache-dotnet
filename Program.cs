var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MyMemoryCache>();
builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddSingleton<GetAgeEstimateService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
