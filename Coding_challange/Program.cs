using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Coding_challange.Configuration;
using Coding_challange.Data; // Make sure to include this namespace
using Microsoft.Extensions.Options; // For IOptions<T>
using Microsoft.OpenApi.Models; // For Swagger

var builder = WebApplication.CreateBuilder(args);

// Load DynamoDB configuration from appsettings.json
builder.Services.Configure<DynamoDbConfiguration>(
    builder.Configuration.GetSection("DynamoDbConfiguration"));

// Add services to the container
builder.Services.AddControllers();

// Register the Repository service
builder.Services.AddScoped<Repository>();

// Configure AWS SDK for DynamoDB
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var config = sp.GetRequiredService<IOptions<DynamoDbConfiguration>>().Value;

    var credentials = new Amazon.Runtime.BasicAWSCredentials("test", "test"); // Dummy for LocalStack
    
    var dynamoDbConfig = new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:4566", // LocalStack endpoint
        AuthenticationRegion = "us-east-1", 
    };

    return new AmazonDynamoDBClient(credentials, dynamoDbConfig);
});

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        //c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();