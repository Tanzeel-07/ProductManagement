using ProductManagement.Data.Models;

namespace ProductManagement.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductsAsync(
            string category = null,
            string sku = null,
            bool isActiveOnly = false);
    }
}
