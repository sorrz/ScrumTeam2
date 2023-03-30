using ShopGeneral.Data;
using ShopGeneral.Infrastructure.Context;

namespace ShopGeneral.Services;

public interface IProductService
{
    public IEnumerable<ProductServiceModel> GetNewProducts(int cnt, CurrentCustomerContext context);
    public List<Product> GetAllProductsOrDefault();
    public Task<List<Product>> VerifyProductImages();
}