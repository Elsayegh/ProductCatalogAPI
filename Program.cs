using Microsoft.Azure.Cosmos;
using ProductCatalogAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var endpoint = config["Cosmos:Endpoint"];
    var key = config["Cosmos:Key"];
    var options = new CosmosClientOptions
    {
        ConsistencyLevel = ConsistencyLevel.Session
    };

    return new CosmosClient(endpoint, key);
});

builder.Services.AddSingleton<CosmosProductRepository>();
builder.Services.AddSingleton<ChangeFeedService>();

var app = builder.Build();

var changeFeed = app.Services.GetRequiredService<ChangeFeedService>();
changeFeed.Start();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();