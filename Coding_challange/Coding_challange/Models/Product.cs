

namespace Coding_challange.Models
{
    public class Product
    {
        public string Id { get; set; } // Unique identifier
        public string Name { get; set; } // Product name
        public decimal Price { get; set; } // Product price
        public string Description { get; set; } // Product description
        public int Stock { get; set; } // Quantity in stock
    }
}