using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Interfaces;
using ProductManagement.Data.Models;

namespace ProductManagement.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ProductDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsAsync(
            string category = null,
            string sku = null,
            bool isActiveOnly = false)
        {
            var products = DbSet.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(x => x.Category == category);
            }

            if (!string.IsNullOrEmpty(sku))
            {
                products = products.Where(x => x.Sku == sku);
            }

            if (isActiveOnly)
            {
                products = products.Where(x => x.IsActive);
            }

            return await products.ToListAsync();
        }
    }
}
