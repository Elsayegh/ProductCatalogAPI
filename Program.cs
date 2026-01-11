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

    return new CosmosClient(endpoint, key);
});

builder.Services.AddSingleton<CosmosProductRepository>();

var app = builder.Build();

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