using NextVer.Domain.Models;
using NextVer.Infrastructure.Persistance;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace NextVer.Infrastructure.Persistence.DbSeed
{
    public class Seed
    {
        private readonly NextVerDbContext _context;
        public Seed(NextVerDbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            SeedUserTypes();
            SeedUsers();
            SeedProductionTypes();
            SeedGenres();
            SeedUniverses();
            SeedReleasePlaceTypes();
            SeedReleasePlaces();
            SeedRatingCategories();
            SeedMovies();
            SeedTvShows();
            SeedEpisodes();
            SeedMovieGenres();
            SeedTvShowGenres();
            SeedMovieUniverses();
            SeedTvShowUniverses();
            SeedRatings();
            SeedComments();
            SeedTechnologyTypes();
            SeedTechnologies();
            SeedNotificationTypes();
            SeedMediaGalleryTypes();
            SeedUserCollectionTypes();
            SeedNotifications();
            //SeedGalleries();
            SeedProductionVersions();
            SeedProductionTechnologies();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = new byte[64];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }

                passwordSalt = salt;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var userData = File
                    .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/UserData.json");
                var users = JsonSerializer.Deserialize<List<User>>(userData);
                foreach (var user in users)
                {
                    CreatePasswordHash("password", out var passwordHash, out var passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _context.Users.Add(user);
                }
                _context.SaveChanges();
            }
        }

        private void SeedUserTypes()
        {
            if (!_context.UserTypes.Any())
            {
                var userTypeData = File
                    .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/UserTypeData.json");
                var userTypes = JsonSerializer.Deserialize<List<UserType>>(userTypeData);
                foreach (var userType in userTypes)
                {
                    _context.UserTypes.Add(userType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedMovies()
        {
            if (!_context.Movies.Any())
            {
                var movieData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/MovieData.json");
                var movies = JsonSerializer.Deserialize<List<Movie>>(movieData);
                foreach (var movie in movies)
                {
                    _context.Movies.Add(movie);
                }
                _context.SaveChanges();
            }
        }

        private void SeedTvShows()
        {
            if (!_context.TvShows.Any())
            {
                var tvShowData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/TvShowData.json");
                var tvShows = JsonSerializer.Deserialize<List<TvShow>>(tvShowData);
                foreach (var tvShow in tvShows)
                {
                    _context.TvShows.Add(tvShow);
                }
                _context.SaveChanges();
            }
        }

        private void SeedEpisodes()
        {
            if (!_context.Episodes.Any())
            {
                var episodeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/EpisodeData.json");
                var episodes = JsonSerializer.Deserialize<List<Episode>>(episodeData);
                foreach (var episode in episodes)
                {
                    _context.Episodes.Add(episode);
                }
                _context.SaveChanges();
            }
        }

        private void SeedProductionTypes()
        {
            if (!_context.ProductionTypes.Any())
            {
                var productionTypeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/ProductionTypeData.json");
                var productionTypes = JsonSerializer.Deserialize<List<ProductionType>>(productionTypeData);
                foreach (var productionType in productionTypes)
                {
                    _context.ProductionTypes.Add(productionType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedGenres()
        {
            if (!_context.Genres.Any())
            {
                var genreData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/GenreData.json");
                var genres = JsonSerializer.Deserialize<List<Genre>>(genreData);
                foreach (var genre in genres)
                {
                    _context.Genres.Add(genre);
                }
                _context.SaveChanges();
            }
        }

        private void SeedUniverses()
        {
            if (!_context.Universes.Any())
            {
                var universeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/UniverseData.json");
                var universes = JsonSerializer.Deserialize<List<Universe>>(universeData);
                foreach (var universe in universes)
                {
                    _context.Universes.Add(universe);
                }
                _context.SaveChanges();
            }
        }

        private void SeedMovieGenres()
        {
            if (!_context.MovieGenres.Any())
            {
                var movieGenreData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/MovieGenreData.json");
                var movieGenres = JsonSerializer.Deserialize<List<MovieGenre>>(movieGenreData);
                foreach (var movieGenre in movieGenres)
                {
                    _context.MovieGenres.Add(movieGenre);
                }
                _context.SaveChanges();
            }
        }

        private void SeedMovieUniverses()
        {
            if (!_context.MovieUniverses.Any())
            {
                var movieUniverseData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/MovieUniverseData.json");
                var movieUniverses = JsonSerializer.Deserialize<List<MovieUniverse>>(movieUniverseData);
                foreach (var movieUniverse in movieUniverses)
                {
                    _context.MovieUniverses.Add(movieUniverse);
                }
                _context.SaveChanges();
            }
        }

        private void SeedTvShowGenres()
        {
            if (!_context.TvShowGenres.Any())
            {
                var tvShowGenreData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/TvShowGenreData.json");
                var tvShowGenres = JsonSerializer.Deserialize<List<TvShowGenre>>(tvShowGenreData);
                foreach (var tvShowGenre in tvShowGenres)
                {
                    _context.TvShowGenres.Add(tvShowGenre);
                }
                _context.SaveChanges();
            }
        }

        private void SeedTvShowUniverses()
        {
            if (!_context.TvShowUniverses.Any())
            {
                var tvShowUniverseData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/TvShowUniverseData.json");
                var tvShowUniverses = JsonSerializer.Deserialize<List<TvShowUniverse>>(tvShowUniverseData);
                foreach (var tvShowUniverse in tvShowUniverses)
                {
                    _context.TvShowUniverses.Add(tvShowUniverse);
                }
                _context.SaveChanges();
            }
        }

        private void SeedReleasePlaceTypes()
        {
            if (!_context.ReleasePlaceTypes.Any())
            {
                var releasePlaceTypeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/ReleasePlaceTypeData.json");
                var releasePlaceTypes = JsonSerializer.Deserialize<List<ReleasePlaceType>>(releasePlaceTypeData);
                foreach (var releasePlaceType in releasePlaceTypes)
                {
                    _context.ReleasePlaceTypes.Add(releasePlaceType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedReleasePlaces()
        {
            if (!_context.ReleasePlaces.Any())
            {
                var releasePlaceData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/ReleasePlaceData.json");
                var releasePlaces = JsonSerializer.Deserialize<List<ReleasePlace>>(releasePlaceData);
                foreach (var releasePlace in releasePlaces)
                {
                    _context.ReleasePlaces.Add(releasePlace);
                }
                _context.SaveChanges();
            }
        }

        private void SeedRatingCategories()
        {
            if (!_context.RatingCategories.Any())
            {
                var ratingCategoryData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/RatingCategoryData.json");
                var ratingCategories = JsonSerializer.Deserialize<List<RatingCategory>>(ratingCategoryData);
                foreach (var ratingCategory in ratingCategories)
                {
                    _context.RatingCategories.Add(ratingCategory);
                }
                _context.SaveChanges();
            }
        }

        private void SeedRatings()
        {
            if (!_context.Ratings.Any())
            {
                var ratingData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/RatingData.json");
                var ratings = JsonSerializer.Deserialize<List<Rating>>(ratingData);
                foreach (var rating in ratings)
                {
                    _context.Ratings.Add(rating);
                }
                _context.SaveChanges();
            }
        }

        private void SeedTechnologyTypes()
        {
            if (!_context.TechnologyTypes.Any())
            {
                var technologyTypeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/TechnologyTypeData.json");
                var technologyTypes = JsonSerializer.Deserialize<List<TechnologyType>>(technologyTypeData);
                foreach (var technologyType in technologyTypes)
                {
                    _context.TechnologyTypes.Add(technologyType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedTechnologies()
        {
            if (!_context.Technologies.Any())
            {
                var technologyData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/TechnologyData.json");
                var technologies = JsonSerializer.Deserialize<List<Technology>>(technologyData);
                foreach (var technology in technologies)
                {
                    _context.Technologies.Add(technology);
                }
                _context.SaveChanges();
            }
        }

        private void SeedProductionVersions()
        {
            if (!_context.ProductionVersions.Any())
            {
                var productionVersionData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/ProductionVersionData.json");
                var productionVersions = JsonSerializer.Deserialize<List<ProductionVersion>>(productionVersionData);
                foreach (var productionVersion in productionVersions)
                {
                    _context.ProductionVersions.Add(productionVersion);
                }
                _context.SaveChanges();
            }
        }

        private void SeedProductionTechnologies()
        {
            if (!_context.ProductionTechnologies.Any())
            {
                var productionTechnologyData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/ProductionTechnologyData.json");
                var productionTechnologies = JsonSerializer.Deserialize<List<ProductionTechnology>>(productionTechnologyData);
                foreach (var productionTechnology in productionTechnologies)
                {
                    _context.ProductionTechnologies.Add(productionTechnology);
                }
                _context.SaveChanges();
            }
        }

        private void SeedComments()
        {
            if (!_context.Comments.Any())
            {
                var commentData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/CommentData.json");
                var comments = JsonSerializer.Deserialize<List<Comment>>(commentData);
                foreach (var comment in comments)
                {
                    _context.Comments.Add(comment);
                }
                _context.SaveChanges();
            }
        }

        private void SeedUserCollectionTypes()
        {
            if (!_context.UserCollectionTypes.Any())
            {
                var userCollectionTypeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/UserCollectionTypeData.json");
                var userCollectionTypes = JsonSerializer.Deserialize<List<UserCollectionType>>(userCollectionTypeData);
                foreach (var userCollectionType in userCollectionTypes)
                {
                    _context.UserCollectionTypes.Add(userCollectionType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedNotificationTypes()
        {
            if (!_context.NotificationTypes.Any())
            {
                var notificationTypeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/NotificationTypeData.json");
                var notificationTypes = JsonSerializer.Deserialize<List<NotificationType>>(notificationTypeData);
                foreach (var notificationType in notificationTypes)
                {
                    _context.NotificationTypes.Add(notificationType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedNotifications()
        {
            if (!_context.Notifications.Any())
            {
                var notificationData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/NotificationData.json");
                var notifications = JsonSerializer.Deserialize<List<Notification>>(notificationData);
                foreach (var notification in notifications)
                {
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();
            }
        }

        private void SeedMediaGalleryTypes()
        {
            if (!_context.MediaGalleryTypes.Any())
            {
                var mediaGalleryTypeData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/MediaGalleryTypeData.json");
                var mediaGalleryTypes = JsonSerializer.Deserialize<List<MediaGalleryType>>(mediaGalleryTypeData);
                foreach (var mediaGalleryType in mediaGalleryTypes)
                {
                    _context.MediaGalleryTypes.Add(mediaGalleryType);
                }
                _context.SaveChanges();
            }
        }

        private void SeedGalleries()
        {
            if (!_context.Galleries.Any())
            {
                var galleryData = File
                        .ReadAllText("../NextVer.Infrastructure/Persistance/DbSeed/GalleryData.json");
                var galleries = JsonSerializer.Deserialize<List<Gallery>>(galleryData);
                foreach (var gallery in galleries)
                {
                    _context.Galleries.Add(gallery);
                }
                _context.SaveChanges();
            }
        }
    }
}



