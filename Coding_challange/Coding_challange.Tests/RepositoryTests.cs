using Amazon.DynamoDBv2;
using Coding_challange.Configuration;
using Coding_challange.Data;
using Coding_challange.Models;
using Microsoft.Extensions.Options;
using Xunit;

namespace Coding_challange.Coding_challange.Tests
{
    public class RepositoryTests
    {
        private readonly Repository _repository;

        public RepositoryTests()
        {
            // Initialize the real AWS DynamoDB client
            var credentials = new Amazon.Runtime.BasicAWSCredentials("test", "test"); // Dummy for LocalStack
    
            var dynamoDbConfig = new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:4566", // LocalStack endpoint
                AuthenticationRegion = "us-east-1", 
            };
            IAmazonDynamoDB dynamoDbClient = new AmazonDynamoDBClient(credentials, dynamoDbConfig);

            // Provide a real configuration for DynamoDbConfiguration (or use appsettings)
            var config = new DynamoDbConfiguration
            {
                AwsAccessKeyId = "YourAWSAccessKey",  // Optional, unless set via environment
                AwsSecretAccessKey = "YourAWSSecretKey",  // Optional, unless set via environment
                Region = "us-east-1",  // Replace with your region
                ServiceURL = "http://localhost:4566" // Not needed when connecting to real AWS
            };
    
            // Initialize the repository with the real DynamoDB client
            _repository = new Repository(dynamoDbClient,Options.Create(config));
        }

        [Fact]
        public async Task CreateProduct_ShouldCreateProduct_IfProductDoesNotExist()
        {
            // Arrange
            var newProduct = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Real DynamoDB Product",
                Price = 19.99M,
                Description = "Testing with real DynamoDB",
                Stock = 50
            };

            // Act
            await _repository.CreateProduct(newProduct);

            // Assert: Try fetching the product to verify if it was created
            var fetchedProduct = await _repository.GetProduct(newProduct.Id);
            Assert.NotNull(fetchedProduct);
            Assert.Equal(newProduct.Name, fetchedProduct.Name);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnProduct_IfProductExists()
        {
            // Arrange: Make sure the product exists
            var existingProduct = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Existing Real DynamoDB Product",
                Price = 39.99M,
                Description = "Testing Get with real DynamoDB",
                Stock = 100
            };

            await _repository.CreateProduct(existingProduct);

            // Act: Fetch the product
            var fetchedProduct = await _repository.GetProduct(existingProduct.Id);

            // Assert
            Assert.NotNull(fetchedProduct);
            Assert.Equal(existingProduct.Name, fetchedProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateProduct_IfProductExists()
        {
            // Arrange: Create a product to update
            var existingProduct = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Update Test Product",
                Price = 49.99M,
                Description = "Original description",
                Stock = 50
            };

            await _repository.CreateProduct(existingProduct);

            // Act: Update the product
            existingProduct.Name = "Updated Product Name";
            existingProduct.Price = 59.99M;
            existingProduct.Description = "Updated description";
            await _repository.UpdateProduct(existingProduct.Id, existingProduct);

            // Assert: Fetch the product and verify the updates
            var updatedProduct = await _repository.GetProduct(existingProduct.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product Name", updatedProduct.Name);
            Assert.Equal(59.99M, updatedProduct.Price);
            Assert.Equal("Updated description", updatedProduct.Description);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct_IfProductExists()
        {
            // Arrange: Create a product to delete
            var productToDelete = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Delete Test Product",
                Price = 29.99M,
                Description = "This product will be deleted",
                Stock = 25
            };

            await _repository.CreateProduct(productToDelete);

            // Act: Delete the product
            await _repository.DeleteProduct(productToDelete.Id);

            // Assert: Try fetching the product and ensure it doesn't exist
            var deletedProduct = await _repository.GetProduct(productToDelete.Id);
            Assert.Null(deletedProduct);  // Should return null
        }
        
        [Fact]
        public async Task GetProducts_ShouldReturnAllProducts()
        {
            // Act: delete all products
            await _repository.DeleteAllProducts();
            
            // Arrange: Create some products
            var product1 = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Product 1",
                Price = 19.99M,
                Description = "This is product 1",
                Stock = 100
            };
            var product2 = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Product 2",
                Price = 20.99M,
                Description = "This is product 2",
                Stock = 200
            };

            await _repository.CreateProduct(product1);
            await _repository.CreateProduct(product2);

            // Act: Fetch all products
            var products = await _repository.GetProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count);
        }
        
        [Fact]
        public async Task GetProducts_ShouldReturnEmptyList_IfNoProductsExist()
        {
            // Act: delete all products
            await _repository.DeleteAllProducts();
            
            // Act: Fetch all products
            var products = await _repository.GetProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Empty(products);
        }
    }
}
