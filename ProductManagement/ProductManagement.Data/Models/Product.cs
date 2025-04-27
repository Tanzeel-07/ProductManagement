
namespace ProductManagement.Data.Models
{
    public class Product
    {
        public Product()
        {
            CreatedDate = DateTime.UtcNow;
            StockQuantity = 0; 
        }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
