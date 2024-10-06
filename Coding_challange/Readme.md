

# Coding_challange

A simple ASP.NET Core Web API project that connects to AWS DynamoDB (using LocalStack for local development). The project includes basic CRUD operations for a product inventory system.

## Prerequisites

To run the project locally, make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://docs.docker.com/get-docker/) 
- [Docker Compose](https://docs.docker.com/compose/install/)
- [AWS CLI](https://aws.amazon.com/cli/) (optional, for interacting with LocalStack via CLI)

## Setting Up the Project

### 1. Clone the Repository

```bash
git clone https://github.com/Joseane772/Product_API.git
cd Coding_challange
```

### 2. Install the Required NuGet Packages

Restore the required NuGet packages by running the following command:

```bash
dotnet restore
```

### 3. Set Up LocalStack (for Local DynamoDB)

LocalStack is a fully functional local AWS cloud stack that enables you to develop and test your cloud apps offline. It spins up a testing environment on your local machine that provides the same functionality and APIs as the real AWS cloud environment.

To set up LocalStack, you can use the provided `docker-compose.yml` file. This file defines a LocalStack container that runs a local DynamoDB instance.
```bash
cd docker
docker-comopse up
```

### 4. Run the Project

You can run the project by executing the following command:

```bash
dotnet run
```

The API should now be running at `http://localhost:5097`.

### 5. Test the API

You can test the API using tools like **Postman**, **Swagger**, or **curl**.

The API 

#### Example curl Commands:

- **Create a Product**:

  ```bash
  curl -X POST http://localhost:5097/api/products \
    -H "Content-Type: application/json" \
    -d '{"id": "your-guid", "name": "Sample Product", "price": 19.99, "description": "Sample product description", "stock": 100}'
  ```

- **Get All Products**:

  ```bash
  curl -X GET http://localhost:5097/api/products
  ```

- **Update a Product**:

  ```bash
  curl -X PUT http://localhost:5097/api/products/{id} \
    -H "Content-Type: application/json" \
    -d '{"name": "Updated Product", "price": 29.99, "description": "Updated description", "stock": 150}'
  ```

- **Delete a Product**:

  ```bash
  curl -X DELETE http://localhost:5097/api/products/{id}
  ```

### 7. Running Unit Tests

This project also includes unit tests for the `Repository` class.

To run the tests, use the following command:

```bash
dotnet test
```

This will execute the flowing tests and display the results.

- `RepositoryTests`
- - `GetProductsAsync_ReturnsProducts`
- - `GetProductAsync_ReturnsProduct`
- - `CreateProductAsync_ReturnsProduct`
- - `UpdateProductAsync_ReturnsProduct`
- - `DeleteProductAsync_ReturnsProduct`


- 

