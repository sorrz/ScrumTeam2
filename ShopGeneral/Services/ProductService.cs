﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopGeneral.Data;
using ShopGeneral.Infrastructure.Context;
using System.Net;

namespace ShopGeneral.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPricingService _pricingService;
    public static HttpClient _httpClient;
    public static HttpMessageHandler? Handler { get; set; }

    public ProductService(ApplicationDbContext context, IPricingService pricingService, IMapper mapper)
    {
        _context = context;
        _pricingService = pricingService;
        _mapper = mapper;
    }

    public IEnumerable<ProductServiceModel> GetNewProducts(int cnt, CurrentCustomerContext context)
    {
        return _pricingService.CalculatePrices(_mapper.Map<IEnumerable<ProductServiceModel>>(_context.Products
            .Include(e => e.Category)
            .Include(e => e.Manufacturer)
            .OrderByDescending(e => e.AddedUtc)
            .Take(cnt)), context);
    }

    public List<Product> GetAllProductsOrDefault() => _context.Products.OrderBy(x => x.Name).ToList();
    
    public async Task<List<int>> VerifyProductImages()
    {
        //
        var products = _context.Products.ToList();
        List<int> productImageNotFound = new();
        _httpClient = new HttpClient();

        if (Handler is not null)
        {
            _httpClient = new HttpClient(Handler);
        }

        foreach (var product in products)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(product.ImageUrl)) {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        productImageNotFound.Add(product.Id);
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        return productImageNotFound;
    }

    public List<Category> CheckCategories()
    {
        // Get all Categories, sorted after Name
        var categoryList = _context.Categories.OrderBy(y => y.Name).ToList();
        // Get all Products, sorted after Category Name
        var productList = _context.Products.OrderBy(x => x.Category.Name).ToList();
        // Get the Distinct Lists of the Names
        var result = categoryList.ExceptBy(productList
            .Select(a => a.Category.Name), x => x.Name).ToList();
        return result;
    }

    public List<string> RetrieveAllEmailAddresses()
    {
        // TODO Check Live, so we don't get duplicates! 
        // Filter out the manufacturers that don't have an email address
        var manufacturersWithEmail = _context.Manufacturers.Where(m => !string.IsNullOrEmpty(m.EmailReport));

        // Extract the email addresses
        var emailAddresses = manufacturersWithEmail.Select(m => m.EmailReport).ToList();

        return emailAddresses;
    }




}

