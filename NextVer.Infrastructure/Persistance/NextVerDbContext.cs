using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.Extensions.Configuration;
using NextVer.Domain.Models;

namespace NextVer.Infrastructure.Persistance
{
    public class NextVerDbContext : DbContext
    {
        private readonly IEncryptionProvider _provider;

        public NextVerDbContext(DbContextOptions<NextVerDbContext> options, IConfiguration configuration) :
            base(options)
        {
            var encryptionKey = Convert.FromBase64String(configuration.GetSection("AppSettings:EncryptionKey").Value);
            var keyInfo = AesProvider.GenerateKey(AesKeySize.AES256Bits);
            byte[] initializeVector = keyInfo.IV;
            _provider = new AesProvider(encryptionKey, initializeVector);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<ProductionVersion> ProductionVersions { get; set; }
        public DbSet<ProductionTechnology> ProductionTechnologies { get; set; }
        public DbSet<ProductionType> ProductionTypes { get; set; }
        public DbSet<ReleasePlace> ReleasePlaces { get; set; }
        public DbSet<ReleasePlaceType> ReleasePlaceTypes { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<TechnologyType> TechnologyTypes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<TvShowGenre> TvShowGenres { get; set; }
        public DbSet<Universe> Universes { get; set; }
        public DbSet<MovieUniverse> MovieUniverses { get; set; }
        public DbSet<TvShowUniverse> TvShowUniverses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<MediaGalleryType> MediaGalleryTypes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RatingCategory> RatingCategories { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<UserCollectionProduction> UserCollectionProductions { get; set; }
        public DbSet<UserCollectionType> UserCollectionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(_provider);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                if (relationship.Properties.Any(p => p.Name == "UserId"))
                {
                    relationship.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

            modelBuilder.Entity<MovieGenre>()
                .HasKey(x => new { x.MovieId, x.GenreId });
            /*            modelBuilder.Entity<MovieGenre>()
                            .Ignore(mg => mg.LinkedEntity);*/
            modelBuilder.Entity<MovieUniverse>()
                .HasKey(x => new { x.MovieId, x.UniverseId });
            modelBuilder.Entity<TvShowGenre>()
                .HasKey(x => new { x.TvShowId, x.GenreId });
            modelBuilder.Entity<TvShowUniverse>()
                .HasKey(x => new { x.TvShowId, x.UniverseId });
            modelBuilder.Entity<ProductionTechnology>()
                .HasKey(x => new { x.ProductionVersionId, x.TechnologyId });
            modelBuilder.Entity<UserCollectionProduction>()
                .HasKey(x => new { x.UserCollectionId, x.ProductionVersionId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
