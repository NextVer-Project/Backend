using Hangfire;

namespace NextVerBackend.Configuration
{
    public static class HangfireConfiguration
    {
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHangfireServer(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
            });
        }
    }
}