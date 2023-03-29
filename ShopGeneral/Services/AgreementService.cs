using ShopGeneral.Data;

namespace ShopGeneral.Services;

public class AgreementService : IAgreementService
{
    private readonly ApplicationDbContext _context;

    public AgreementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Agreement> GetActiveAgreements()
    {
        return _context.Agreements.Where(e => e.ValidTo >= DateTime.Now.Date);
    }
}