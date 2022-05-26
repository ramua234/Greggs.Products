using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly decimal EuroConversionRate = 1.11m;
    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _productDataAccess;

    public ProductController(
        ILogger<ProductController> logger,
        IDataAccess<Product> productDataAccess)
    {
        _logger = logger;
        _productDataAccess = productDataAccess;
    }

    [HttpGet]
    public IEnumerable<ProductDto> Get(int pageStart = 0, int pageSize = 5)
    {
        return _productDataAccess.List(pageStart, pageSize).Select(p =>
        new ProductDto
        {
            Name = p.Name,
            PriceInEuros = p.PriceInPounds * EuroConversionRate
        });
    }
}