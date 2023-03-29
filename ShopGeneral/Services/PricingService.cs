using ShopGeneral.Data;
using ShopGeneral.Infrastructure.Context;

namespace ShopGeneral.Services;

public class PricingService : IPricingService
{
    private readonly ApplicationDbContext _context;

    public PricingService(ApplicationDbContext context)
    {
        _context = context;
    }
    public IEnumerable<ProductServiceModel> CalculatePrices(IEnumerable<ProductServiceModel> products, CurrentCustomerContext customerContext)
    {
        foreach (var product in products)
        {
            var lowest = product.BasePrice;
            if (customerContext != null)
            {
                foreach (var agreement in customerContext.Agreements)
                {
                    foreach (var agreementRow in agreement.AgreementRows)
                    {
                        if (AgreementMatches(agreementRow, product))
                        {
                            var price = (1.0m - (agreementRow.PercentageDiscount / 100.0m)) * product.BasePrice;
                            if (price < lowest)
                                lowest = Convert.ToInt32(Math.Round(price, 0));
                        }

                    }
                }
            }
            product.Price = lowest;
            yield return product;
        }
    }

    private bool AgreementMatches(AgreementRow agreementRow, ProductServiceModel product)
    {
        var productCheck = !string.IsNullOrEmpty(agreementRow.ProductMatch);
        var categoryCheck = !string.IsNullOrEmpty(agreementRow.CategoryMatch);
        var manufacturerCheck = !string.IsNullOrEmpty(agreementRow.ManufacturerMatch);
        if (productCheck && !product.Name.ToLower().Contains((string)agreementRow.ProductMatch.ToLower()))
            return false;
        if (categoryCheck && !product.CategoryName.ToLower().Contains((string)agreementRow.CategoryMatch.ToLower()))
            return false;
        if (manufacturerCheck && !product.ManufacturerName.ToLower().Contains((string)agreementRow.ManufacturerMatch.ToLower()))
            return false;

        return true;

    }
}