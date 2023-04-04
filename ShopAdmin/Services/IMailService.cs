using ShopAdmin.Data;

namespace ShopAdmin.Services
{
    public interface IMailService
    {
      
            public Task<bool> SendAsync(MailData mailData, CancellationToken ct);
      
    }
}
