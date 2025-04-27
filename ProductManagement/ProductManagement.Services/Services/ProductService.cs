using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagement.Data.Interfaces;
using ProductManagement.Data.Models;
using ProductManagement.Services.Interfaces;
using ProductManagement.Services.Models.Product;

namespace ProductManagement.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository RepoProduct;

        public ProductService(IProductRepository _repo) : base()
        {
            RepoProduct = _repo;
        }

        public async Task CreateProductAsync(ProductRQ product)
        {
            var products = await RepoProduct.GetProductsAsync(sku: product.Sku);

            if (products.Any())
            {
                throw new Exception($"Product with SKU: {product.Sku} already exist");
            }

            var productItem = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Sku = product.Sku,
                Price = product.Price,
                IsActive = false,
                CreatedDate = DateTime.UtcNow,
            };

            RepoProduct.Add(productItem);

            try
            {
                await RepoProduct.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ProductRS> GetProductsAsync(string category, bool isActiveOnly)
        {
            var products = await RepoProduct.GetProductsAsync(category: category, isActiveOnly: isActiveOnly);

            var ret = products.Select(t => GenerateProductItem(t)).ToList();

            return new ProductRS
            {
                Result = ret,
            };
        }

        public async Task<ProductRS> GetProductByIdAsync(int id)
        {
            var product = await FetchAndValidateProductAsync(id);

            var ret = GenerateProductItem(product);

            return new ProductRS
            {
                Result = [ret]
            };
        }

        public async Task UpdateProductByIdAsync(int id, ProductRQ request)
        {
            var product = await FetchAndValidateProductAsync(id);

            if (product.Sku != request.Sku)
            {
                throw new Exception("Product SKU mismatch.");
            }

            product.Name = request.Name;
            product.Description = request.Description ?? product.Description;
            product.Price = request.Price;
            product.Category = request.Category;

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

        public async Task DeleteProductByIdAsync(int id)
        {
            var product = await FetchAndValidateProductAsync(id);

            RepoProduct.Remove(product);

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
        private ProductRS.ProductItem GenerateProductItem(Product product)
        {
            var item = new ProductRS.ProductItem
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Sku = product.Sku,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return item;
        }

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
