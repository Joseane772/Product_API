using System.Globalization;
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


        //create a table and add the some data  
        public async Task CreateTable()
        {
            var request = new CreateTableRequest
            {
                TableName = "Products",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "id",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "id",
                        KeyType = "HASH"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };

            await dynamoDbClient.CreateTableAsync(request);

            // Add some data to the table
            await CreateProduct(new Product
            {
                Id = "1",
                Name = "Product 1",
                Price = 10.99M,
                Description = "This is product 1",
                Stock = 100
            });

            await CreateProduct(new Product
            {
                Id = "2",
                Name = "Product 2",
                Price = 20.99M,
                Description = "This is product 2",
                Stock = 200
            });
        }
        
        
        // Create a product
        public async Task CreateProduct(Product product)
        {
            
            //check if the table exists if not create it
            var tableResponse = await dynamoDbClient.ListTablesAsync(); 
            if (!tableResponse.TableNames.Contains("Products"))
            {
                await CreateTable();
            }
            
            // cheack if the product already exists
            var existingProduct = await GetProduct(product.Id);
            if (existingProduct != null)
            {
                throw new Exception("Product already exists");
            }
            
            var request = new PutItemRequest
            {
                TableName = "Products",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = product.Id } },
                    { "name", new AttributeValue { S = product.Name } },
                    { "price", new AttributeValue { N = product.Price.ToString(CultureInfo.CurrentCulture) } },
                    { "description", new AttributeValue { S = product.Description } },
                    { "stock", new AttributeValue { N = product.Stock.ToString() } }
                }
            };

            await dynamoDbClient.PutItemAsync(request);
        }
    

        // Get all products
        public async Task<List<Product>> GetProducts()
        {
            
            
            //check if the table exists if not create it
            var tableResponse = await dynamoDbClient.ListTablesAsync(); 
            if (!tableResponse.TableNames.Contains("Products"))
            {
                await CreateTable();
            }

            
            var request = new ScanRequest
            {
                TableName = "Products"
            };

            var response = await dynamoDbClient.ScanAsync(request);
            return response.Items.Select(item => new Product
            {
                Id = item["id"].S,
                Name = item["name"].S,
                Price = decimal.Parse(item["price"].N),
                Description = item["description"].S,
                Stock = int.Parse(item["stock"].N)
            }).ToList();
        }

        // Get a product by ID
        public async Task<Product?> GetProduct(string id)
        {
            
            //check if the table exists if not create it
            var tableResponse = await dynamoDbClient.ListTablesAsync(); 
            if (!tableResponse.TableNames.Contains("Products"))
            {
                await CreateTable();
            }

            
            var request = new GetItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = id } }
                }
            };

            var response = await dynamoDbClient.GetItemAsync(request);
            if (response.Item == null || response.Item.Count == 0)
            {
                return null; // Product not found
            }

            return new Product
            {
                Id = response.Item["id"].S,
                Name = response.Item["name"].S,
                Price = decimal.Parse(response.Item["price"].N),
                Description = response.Item["description"].S,
                Stock = int.Parse(response.Item["stock"].N)
            };
        }

        // Update a product
        public async Task UpdateProduct(string id, Product updatedProduct)
        {
            //check if the table exists if not create it
            var tableResponse = await dynamoDbClient.ListTablesAsync(); 
            if (!tableResponse.TableNames.Contains("Products"))
            {
                await CreateTable();
            }

            
            var request = new UpdateItemRequest
            {
                
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue { S = id } }
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    { "name", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = updatedProduct.Name } } },
                    { "price", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = updatedProduct.Price.ToString() } } },
                    { "description", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = updatedProduct.Description } } },
                    { "stock", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = updatedProduct.Stock.ToString() } } }
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
                    { "id", new AttributeValue { S = id } }
                }
            };

            await dynamoDbClient.DeleteItemAsync(request);
        }
        
        // Delete all products
        public async Task DeleteAllProducts()
        {
            var products = await GetProducts();
            foreach (var product in products)
            {
                await DeleteProduct(product.Id);
            }
        }
    }
}
