
using System;
using Microsoft.AspNetCore.Mvc;
using Coding_challange.Models;
using Coding_challange.Data;


namespace Coding_challange.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly Repository _repository;

        public ProductsController(Repository repository)
        {
            _repository = repository;
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _repository.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        
        // POST: api/products
        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {
            _repository.CreateProduct(product);
        
            return Created();
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = _repository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Description = updatedProduct.Description;
            product.Stock = updatedProduct.Stock;
            _repository.UpdateProduct(product);

            return NoContent();
        }
        
        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _repository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            
            _repository.DeleteProduct(id);
            
            return NoContent();
        }
    }
}
