
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Coding_challange.Models;


namespace Coding_challange.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _service.GetProduct(id);

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
            _service.CreateProduct(product);
        
            return Created();
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(Guid id, Product updatedProduct)
        {
            var product = _service.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Description = updatedProduct.Description;
            product.Stock = updatedProduct.Stock;
            _service.UpdateProduct(product);

            return NoContent();
        }
        
        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _service.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            
            _service.DeleteProduct(product);
            
            return NoContent();
        }
    }
}
