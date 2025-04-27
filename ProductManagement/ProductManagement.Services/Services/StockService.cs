using System;
using System.Threading.Tasks;
using ProductManagement.Data.Interfaces;
using ProductManagement.Data.Models;
using ProductManagement.Services.Interfaces;

namespace ProductManagement.Services.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository RepoProduct;

        public StockService(IProductRepository _repo) : base()
        {
            RepoProduct = _repo;
        }

        public async Task UpdateProductStockByIdAsync(int id, int quantity, bool isIncrement)
        {
            var product = await FetchAndValidateProductAsync(id);

            if (isIncrement)
            {
                product.StockQuantity += quantity;
            }
            else
            {
                if (product.StockQuantity < quantity)
                {
                    throw new Exception("Total stock quantity of the product is less than the provided quantity.");
                }

                product.StockQuantity -= quantity;
            }

            product.IsActive = product.StockQuantity > 0;
            product.UpdatedDate = DateTime.UtcNow;

            RepoProduct.Update(product);

            try
            {
                await RepoProduct.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        // -------- Privates --------
        private async Task<Product> FetchAndValidateProductAsync(int id)
        {
            var product = await RepoProduct.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception($"Product with id: {id} does not exist");
            }

            return product;
        }
    }
}
