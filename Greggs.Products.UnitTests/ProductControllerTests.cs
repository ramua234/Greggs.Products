using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTests
{
    private static readonly decimal EuroConversionRate = 1.11m;

    private readonly Mock<ILogger<ProductController>> _loggerMock = new ();
    private readonly Mock<IDataAccess<Product>> _dataAccessMock = new();

    private ProductController _sut;
    public ProductControllerTests()
    {
        _sut = new ProductController(_loggerMock.Object, _dataAccessMock.Object);
    }


    [Theory]
    [InlineData("Sausage Roll", 0.5)]
    [InlineData("Vegan Sausage Roll", 10.24)]
    public void GetProducts_HavingData_shouldReturnExpectedResult(string productName, decimal priceInPounds)
    {
        Product product = new Product { Name = productName, PriceInPounds = priceInPounds };
        _dataAccessMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
        .Returns(new[] { product });
        var result = _sut.Get(1, 5);

        Assert.Single(result);
        Assert.Equal(productName, result.First().Name);
        Assert.Equal(priceInPounds * EuroConversionRate, result.First().PriceInEuros);

        _dataAccessMock.Verify(v => v.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
}