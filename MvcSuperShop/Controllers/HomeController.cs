using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MvcSuperShop.Models;
using MvcSuperShop.ViewModels;
using ShopGeneral.Data;
using ShopGeneral.Services;

namespace MvcSuperShop.Controllers;

public class HomeController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;

    public HomeController(ICategoryService categoryService, IProductService productService, IMapper mapper,
        ApplicationDbContext context)
        : base(context)
    {
        _categoryService = categoryService;
        _productService = productService;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var model = new HomeIndexViewModel
        {
            TrendingCategories = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetTrendingCategories(3)),
            NewProducts =
                _mapper.Map<List<ProductBoxViewModel>>(_productService.GetNewProducts(10, GetCurrentCustomerContext()))
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}