using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Services.Sync.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding DbContext to the container
// Using InMemoryDatabase in development mode 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});

// Adding repository implementation for IPlatformRepository dependency request
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

// Adding sync http client to the container
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();


// Register automapper as a service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

SeedData.PopulateData(app);

app.Run();
