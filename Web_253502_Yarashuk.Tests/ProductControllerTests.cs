namespace Web_253502_Yarashuk.Tests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Web_253502_Yarashuk.Domain.Models;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.UI.Services.ProductService;
using Web_253502_Yarashuk.UI.Services.CategoryService;
using Web_253502_Yarashuk.UI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.Extensions.Primitives;

public class ProductControllerTests
{
    private readonly ICategoryService _categoryService = Substitute.For<ICategoryService>();
    private readonly IProductService _productService = Substitute.For<IProductService>();
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        var controllerContext = new ControllerContext();
        // Макет HttpContext
        var moqHttpContext = new Mock<HttpContext>();
        moqHttpContext.Setup(c => c.Request.Headers)
            .Returns(new HeaderDictionary());
        controllerContext.HttpContext = moqHttpContext.Object;
        _controller = new ProductController(_productService, _categoryService)
        {
            ControllerContext = controllerContext
        };
    }

    [Fact]
    public async Task Index_ReturnsNotFound_WhenCategoryListNotSuccessful()
    {
        // Arrange
        _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Error("Error")));
        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var result = await _controller.Index(mockConfiguration, null);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Error", notFoundResult.Value);
    }

    [Fact]
    public async Task Index_ReturnsNotFound_WhenProductListNotSuccessful()
    {
        // Arrange
        _categoryService.GetCategoryListAsync()
            .Returns(Task.FromResult(ResponseData<List<Category>>.Success(new List<Category>())));
        _productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(Task.FromResult(ResponseData<ListModel<Product>>.Error("Error")));
        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var result = await _controller.Index(mockConfiguration, "test");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Error", notFoundResult.Value);
    }

    [Fact]
    public async Task Index_SetsViewData_WithCorrectValues()
    {
        // Arrange
        var categories = new List<Category> { new Category { Name = "Category1", NormalizedName = "category1" } };
        _categoryService.GetCategoryListAsync()
            .Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
        _productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(Task.FromResult(ResponseData<ListModel<Product>>.Success(new ListModel<Product>())));

        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var result = await _controller.Index(mockConfiguration, "category1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(categories, _controller.ViewData["categories"]);
        Assert.Equal("Category1", _controller.ViewData["currentCategory"]);
        var model = Assert.IsAssignableFrom<ListModel<Product>>(viewResult.Model);
        Assert.NotNull(model); // Check if model is passed correctly
    }

    [Fact]
    public async Task Index_SetsViewData_WithCorrectDefaultCategory_WhenCategoryIsNull()
    {
        // Arrange
        var categories = new List<Category> { new Category { Name = "Category1", NormalizedName = "category1" } };
        _categoryService.GetCategoryListAsync()
            .Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
        _productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(Task.FromResult(ResponseData<ListModel<Product>>.Success(new ListModel<Product>())));

        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var result = await _controller.Index(mockConfiguration, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(categories, _controller.ViewData["categories"]);
        Assert.Equal("Все", _controller.ViewData["currentCategory"]); // Ensure default value is set as "Все"
        var model = Assert.IsAssignableFrom<ListModel<Product>>(viewResult.Model);
        Assert.NotNull(model); // Check if model is passed correctly
    }
}
