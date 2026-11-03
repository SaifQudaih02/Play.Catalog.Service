using Microsoft.OpenApi;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repository;
using Play.Catalog.Service.Repository.Interface;
using Play.Catalog.Service.settings;


var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB GUID serialization
BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard)
    );

var serviceSettings  = builder.Configuration.GetSection("ServiceSettings")
    .Get<ServiceSettings>() ??
                       throw new InvalidOperationException("ServiceSettings is missing.");
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings")
    .Get<MongoDbSettings>() ??
                      throw new InvalidOperationException("MongoDbSettings is missing.");


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Play Catalog API",
        Version = "v1"
    });
});

builder.Services.AddControllers(options => {options.SuppressAsyncSuffixInActionNames = false;});

builder.Services.AddSingleton(serviceProvider  =>
{
var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
return mongoClient.GetDatabase(serviceSettings.ServiceName);
});

// Register repositories
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Play Catalog API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "illy", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
