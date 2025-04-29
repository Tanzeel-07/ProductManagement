using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Services.Interfaces;
using ProductManagement.WebApi.Controllers;

namespace ProductManagement.Tests
{
    public class StockControllerTests
    {
        private readonly Mock<IStockService> _mockService;
        private readonly StockController _controller;

        public StockControllerTests()
        {
            _mockService = new Mock<IStockService>();
            _controller = new StockController(_mockService.Object);


            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Theory]
        [InlineData(100003, 4)]
        [InlineData(100004, 2)]
        public async Task IncrementProductStockByIdAsync_ReturnsOkResult(int id, int quantity)
        {
            // Arrange
            var incrementFlag = true;
            _mockService.Setup(svc => svc.UpdateProductStockByIdAsync(id, quantity, incrementFlag))
                         .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _controller.IncrementProductStockByIdAsync(id, quantity);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<OkResult>();

            _mockService.Verify(svc => svc.UpdateProductStockByIdAsync(id, quantity, incrementFlag), Times.Once);
        }

        [Theory]
        [InlineData(100003, 4)]
        [InlineData(100004, 2)]
        public async Task DecrementProductStockByIdAsync_ReturnsOkResult(int id, int quantity)
        {
            // Arrange
            var incrementFlag = false;
            _mockService.Setup(svc => svc.UpdateProductStockByIdAsync(id, quantity, incrementFlag))
                         .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _controller.DecrementProductStockByIdAsync(id, quantity);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<OkResult>();

            _mockService.Verify(svc => svc.UpdateProductStockByIdAsync(id, quantity, incrementFlag), Times.Once);
        }
    }
}