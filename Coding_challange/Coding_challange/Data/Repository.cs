using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Coding_challange.Models;
using Coding_challange.Configuration;
using Microsoft.Extensions.Options;

namespace Coding_challange.Data
{
    public class Repository(IAmazonDynamoDB dynamoDbClient, IOptions<DynamoDbConfiguration> configuration)
    {
        private readonly DynamoDbConfiguration _configuration = configuration.Value;


        // Create a product
        public async Task CreateProduct(Product product)
        {
            var request = new PutItemRequest
            {
                TableName = "Products",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = product.Id } },
                    { "Name", new AttributeValue { S = product.Name } },
                }
            };

            await dynamoDbClient.PutItemAsync(request);
        }
    

        // Get all products
        public async Task<List<Product>> GetProducts()
        {
            var request = new ScanRequest
            {
                TableName = "Products"
            };

            var response = await dynamoDbClient.ScanAsync(request);
            return response.Items.Select(item => new Product
            {
                Id = item["Id"].S,
                Name = item["Name"].S,
                Price = decimal.Parse(item["Price"].N),
                Description = item["Description"].S,
                Stock = int.Parse(item["Stock"].N)
            }).ToList();
        }

        // Get a product by ID
        public async Task<Product?> GetProduct(string id)
        {
            var request = new GetItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id } }
                }
            };

            var response = await dynamoDbClient.GetItemAsync(request);
            if (response.Item == null || response.Item.Count == 0)
            {
                return null; // Product not found
            }

            return new Product
            {
                Id = response.Item["Id"].S,
                Name = response.Item["Name"].S,
                Price = decimal.Parse(response.Item["Price"].N),
                Description = response.Item["Description"].S,
                Stock = int.Parse(response.Item["Stock"].N)
            };
        }

        // Update a product
        public async Task UpdateProduct(string id, Product updatedProduct)
        {
            var request = new UpdateItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id } }
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    { "Name", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = updatedProduct.Name } } },
                    { "Price", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = updatedProduct.Price.ToString() } } },
                    { "Description", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = updatedProduct.Description } } },
                    { "Stock", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = updatedProduct.Stock.ToString() } } }
                }
            };

            await dynamoDbClient.UpdateItemAsync(request);
        }

        // Delete a product
        public async Task DeleteProduct(string id)
        {
            var request = new DeleteItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id } }
                }
            };

            await dynamoDbClient.DeleteItemAsync(request);
        }
    }
}
