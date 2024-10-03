
using Microsoft.AspNetCore.Mvc;
using Coding_challange.Models;
using Coding_challange.Data;

namespace Coding_challange.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(Repository repository) : ControllerBase
{
    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await repository.GetProducts();
        return Ok(products); // Return 200 OK with the list of products
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        var product = await repository.GetProduct(id);
        if (product == null)
        {
            return NotFound(); // Return 404 if product not found
        }
        return Ok(product); // Return 200 OK with the product data
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await repository.CreateProduct(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product); // Return 201 Created
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, Product updatedProduct)
    {
        var existingProduct = await repository.GetProduct(id);
        if (existingProduct == null)
        {
            return NotFound(); // Return 404 if the product is not found
        }

        await repository.UpdateProduct(id, updatedProduct); // Update the product
        return NoContent(); // Return 204 No Content on successful update
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var product = await repository.GetProduct(id);
        if (product == null)
        {
            return NotFound(); // Return 404 if the product is not found
        }

        await repository.DeleteProduct(id); // Delete the product
        return NoContent(); // Return 204 No Content on successful delete
    }
}