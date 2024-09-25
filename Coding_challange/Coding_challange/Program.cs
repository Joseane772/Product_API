using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Extensions.NETCore.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation and UI testing.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DynamoDB to use LocalStack with a specified region
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var credentials = new Amazon.Runtime.BasicAWSCredentials("test", "test");
    var config = new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:4566",  
        RegionEndpoint = Amazon.RegionEndpoint.USEast1  
    };
    return new AmazonDynamoDBClient(credentials, config);
});

// Register the Repository class so it can be injected into controllers
builder.Services.AddScoped<Coding_challange.Data.Repository>();

var app = builder.Build();

// Enable middleware to serve Swagger UI and Swagger JSON endpoint.
if (app.Environment.IsDevelopment())  // only in development.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
