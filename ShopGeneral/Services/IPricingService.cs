using ShopGeneral.Infrastructure.Context;

namespace ShopGeneral.Services;

public interface IPricingService
{
    IEnumerable<ProductServiceModel> CalculatePrices(IEnumerable<ProductServiceModel> products,
        CurrentCustomerContext context);
}