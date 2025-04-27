using System.Threading.Tasks;
using ProductManagement.Services.Models.Product;

namespace ProductManagement.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductRS> GetProductsAsync(string category, bool isActiveOnly);

        Task<ProductRS> GetProductByIdAsync(int id);

        Task CreateProductAsync(ProductRQ product);

        Task UpdateProductByIdAsync(int id, ProductRQ request);

        Task DeleteProductByIdAsync(int id);
    }
}
