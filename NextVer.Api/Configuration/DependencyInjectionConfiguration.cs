using MailKit.Net.Smtp;
using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Repositories;
using NextVer.Infrastructure.Services;

namespace NextVerBackend.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddScoped<ISmtpClient, SmtpClient>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}