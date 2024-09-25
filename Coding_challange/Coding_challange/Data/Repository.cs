using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Coding_challange.Models;



namespace Coding_challange.Data
{
    public class Repository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        // private readonly DynamoDbConfiguration _configuration;

        public Repository(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
            //_configuration = configuration;
        }

        
        public Product? GetProduct(int id)
        {
            var request = new GetItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id.ToString() } }
                }
            };

            var response = _dynamoDbClient.GetItemAsync(request).Result;

            if (response.Item == null || response.Item.Count == 0)
            {
                return null;
            }

            return new Product
            {
                Id = int.Parse(response.Item["Id"].S),
                Name = response.Item["Name"].S,
                Price = decimal.Parse(response.Item["Price"].N),
                Description = response.Item["Description"].S,
                Stock = int.Parse(response.Item["Stock"].N)
            };
        }

        
        public void CreateProduct(Product product)
        {
            var request = new PutItemRequest
            {
                TableName = "Products",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = product.Id.ToString() } },
                    { "Name", new AttributeValue { S = product.Name } },
                    { "Price", new AttributeValue { N = product.Price.ToString() } },
                    { "Description", new AttributeValue { S = product.Description } },
                    { "Stock", new AttributeValue { N = product.Stock.ToString() } }
                }
            };

            _dynamoDbClient.PutItemAsync(request).Wait();
        }

        
        public void UpdateProduct(Product product)
        {
            var request = new UpdateItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = product.Id.ToString() } }
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    { "Name", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = product.Name } } },
                    { "Price", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = product.Price.ToString() } } },
                    { "Description", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = product.Description } } },
                    { "Stock", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = product.Stock.ToString() } } }
                }
            };

            _dynamoDbClient.UpdateItemAsync(request).Wait();
        }

        
        public void DeleteProduct(int id)
        {
            var request = new DeleteItemRequest
            {
                TableName = "Products",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id.ToString() } }
                }
            };
        
            _dynamoDbClient.DeleteItemAsync(request).Wait();
        }
    }
}
