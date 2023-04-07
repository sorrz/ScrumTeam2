﻿using AutoMapper;
using Bogus.Bson;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using ShopGeneral.Data;
using ShopGeneral.Infrastructure.Context;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Text.Json;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace ShopGeneral.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPricingService _pricingService;
    public static HttpClient? _httpClient;
    public static HttpMessageHandler? _handler { get; set; }

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

        if (_handler is not null)
        {
            _httpClient = new HttpClient(_handler);
        }

        foreach (var product in products)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(product.ImageUrl))
                {
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
        var categoryList = _context.Categories.OrderBy(y => y.Name).ToList();
        var productList = _context.Products.OrderBy(x => x.Category.Name).ToList();
        var result = categoryList.ExceptBy(productList
            .Select(a => a.Category.Name), x => x.Name).ToList();
        return result;
    }

    public List<string> JsonToXml(string JsonInput)
    {
        List<string> xmlString = new();
        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(JsonInput);
        var x = doc.InnerXml.ToString();
        xmlString.Add(x);
        return xmlString;
    }

}

