using NextVer.Domain.Email;

namespace NextVer.Infrastructure.Interfaces
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(EmailMessage message);
    }
}