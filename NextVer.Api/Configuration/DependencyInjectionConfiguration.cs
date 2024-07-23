using MailKit.Net.Smtp;
using NextVer.Domain.Models;
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
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ITvShowRepository, TvShowRepository>();
            services.AddScoped<IBaseRepository<Movie>, BaseRepository<Movie>>();
            services.AddScoped<IBaseRepository<TvShow>, BaseRepository<TvShow>>();
            services.AddScoped<IBaseRepository<Episode>, BaseRepository<Episode>>();
            services.AddScoped<IBaseRepository<Genre>, BaseRepository<Genre>>();
            services.AddScoped<IBaseRepository<Universe>, BaseRepository<Universe>>();
            services.AddScoped<IBaseRepository<Comment>, BaseRepository<Comment>>();
            services.AddScoped<IBaseRepository<Gallery>, BaseRepository<Gallery>>();
            services.AddScoped<IBaseRepository<MediaGalleryType>, BaseRepository<MediaGalleryType>>();
            services.AddScoped<IBaseRepository<Notification>, BaseRepository<Notification>>();
            services.AddScoped<IBaseRepository<NotificationType>, BaseRepository<NotificationType>>();
            services.AddScoped<IBaseRepository<ProductionType>, BaseRepository<ProductionType>>();
            services.AddScoped<IBaseRepository<ProductionVersion>, BaseRepository<ProductionVersion>>();
            services.AddScoped<IBaseRepository<RatingCategory>, BaseRepository<RatingCategory>>();
            services.AddScoped<IBaseRepository<Rating>, BaseRepository<Rating>>();
            services.AddScoped<IBaseRepository<ReleasePlaceType>, BaseRepository<ReleasePlaceType>>();
            services.AddScoped<IBaseRepository<ReleasePlace>, BaseRepository<ReleasePlace>>();
            services.AddScoped<IBaseRepository<TechnologyType>, BaseRepository<TechnologyType>>();
            services.AddScoped<IBaseRepository<Technology>, BaseRepository<Technology>>();
            services.AddScoped<IBaseRepository<UserCollectionType>, BaseRepository<UserCollectionType>>();
            services.AddScoped<IBaseRepository<UserCollection>, BaseRepository<UserCollection>>();
            services.AddScoped<IBaseRepository<UserType>, BaseRepository<UserType>>();
            services.AddScoped<IRatingRepository, RatingRepository>();

            services.AddScoped<ISmtpClient, SmtpClient>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}