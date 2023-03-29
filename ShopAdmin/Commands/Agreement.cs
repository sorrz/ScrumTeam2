using Microsoft.Extensions.Logging;
using ShopGeneral.Services;

namespace ShopAdmin.Commands;

public class Agreement : ConsoleAppBase
{
    private readonly IAgreementService _agreementService;
    private readonly ILogger<Agreement> _logger;

    public Agreement(ILogger<Agreement> logger, IAgreementService agreementService)
    {
        _logger = logger;
        _agreementService = agreementService;
    }

    public void Expires(int days)
    {
        _logger.LogInformation("Expires starting");
        foreach (var agreement in _agreementService.GetActiveAgreements()
                     .Where(e => e.ValidTo < DateTime.Now.AddDays(days)))
        {
            var expiresInDays = agreement.ValidTo - DateTime.Now;
            Console.WriteLine($"{agreement.Id} expires in {expiresInDays} days");
        }

        _logger.LogInformation("Expires ending");
    }
}