using ShopGeneral.Data;

namespace ShopGeneral.Infrastructure.Context;

public class CurrentCustomerContext
{
    public Guid UserId { get; set; }
    public string Email { get; set; }

    public List<Agreement> Agreements { get; set; }
}