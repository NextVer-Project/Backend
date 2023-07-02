using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Repositories;

namespace NextVerBackend.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
        }
    }
}