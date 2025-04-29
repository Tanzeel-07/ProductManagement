using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.Services.Interfaces;
using ProductManagement.Services.Models.Product;
using ProductManagement.WebApi.Controllers;

namespace ProductManagement.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);


            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Theory]
        [InlineData(null, true)]
        public async Task GetProducts_WhenProductsExist_ReturnsOkWithProducts(string category, bool isActiveOnly)
        {
            // Arrange
            var expectedProducts = new List<ProductRS.ProductItem>();

            var productResult = new ProductRS
            {
                Result = expectedProducts
            };

            _mockService.Setup(s => s.GetProductsAsync(category, isActiveOnly)).ReturnsAsync(productResult);

            // Act
            var result = await _controller.GetProductsAsync(category, isActiveOnly);

            // Assert
            Assert.NotNull(result);

            var actionResult = Assert.IsType<ProductRS>(result);
            var actualProducts = Assert.IsAssignableFrom<List<ProductRS.ProductItem>>(actionResult.Result);

            Assert.Equal(expectedProducts.Count, actualProducts.Count());
            Assert.Equal(expectedProducts, actualProducts);

            _mockService.Verify(s => s.GetProductsAsync(category, isActiveOnly), Times.Once);
        }

        [Fact]
        public async Task GetProductById_WhenProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            var productId = 10003;
            var expectedProducts = new List<ProductRS.ProductItem>()
            {
                new ProductRS.ProductItem { ProductId = productId }
            };

            var productResult = new ProductRS
            {
                Result = expectedProducts
            };

            _mockService.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync(productResult);

            // Act
            var result = await _controller.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);

            var actionResult = Assert.IsType<ProductRS>(result);
            var actualProducts = Assert.IsAssignableFrom<List<ProductRS.ProductItem>>(actionResult.Result);

            Assert.Equal(expectedProducts.Count, actualProducts.Count());
            Assert.Equal(expectedProducts, actualProducts);

            _mockService.Verify(s => s.GetProductByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var request = new ProductRQ
            {
                Name = "New Gadget",
                Price = 99.99m,
                Sku = "SKU-0001",
                Category = "Electronics"
            };

            _mockService.Setup(s => s.CreateProductAsync(request)).Returns(Task.CompletedTask);

            //Act
            var actionResult = await _controller.CreateProductAsync(request);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<CreatedResult>();

            var createdResult = actionResult.As<CreatedResult>();
            createdResult.Value.Should().BeNull();

            _mockService.Verify(s => s.CreateProductAsync(request), Times.Once);
        }

        [Fact]
        public async Task UpdateProductByIdAsync_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var productId = 100003;

            var request = new ProductRQ
            {
                Name = "New Gadget",
                Price = 99.99m,
                Sku = "SKU-0001",
                Category = "Electronics"
            };

            _mockService.Setup(s => s.UpdateProductByIdAsync(productId, request)).Returns(Task.CompletedTask);

            //Act
            var actionResult = await _controller.UpdateProductByIdAsync(productId, request);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<OkResult>();

            _mockService.Verify(s => s.UpdateProductByIdAsync(productId, request), Times.Once);
        }

        [Fact]
        public async Task DeleteProductByIdAsync_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var productId = 100003;

            _mockService.Setup(s => s.DeleteProductByIdAsync(productId)).Returns(Task.CompletedTask);

            //Act
            var actionResult = await _controller.DeleteProductByIdAsync(productId);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<OkResult>();

            _mockService.Verify(s => s.DeleteProductByIdAsync(productId), Times.Once);
        }
    }
}