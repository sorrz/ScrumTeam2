using ShopGeneral.Data;

namespace ShopGeneral.Services;

public interface IAgreementService
{
    IEnumerable<Agreement> GetActiveAgreements();
}