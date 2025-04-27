using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Services.Models.Product
{
    public class ProductRQ : BaseRQ
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public class ProductRS : BaseRS<List<ProductRS.ProductItem>>
    {
        public class ProductItem
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Sku { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
            public int StockQuantity { get; set; }
        }
    }

}
