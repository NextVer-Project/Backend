using AutoMapper;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;

namespace NextVerBackend.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserForLoginDto, User>();
            CreateMap<EmailForChange, User>();
            CreateMap<MovieForAddDto, Movie>();
            CreateMap<MovieForEditDto, Movie>();
            CreateMap<MovieCountDto, Movie>();
            CreateMap<Movie, MovieForListDto>();
            CreateMap<TvShowForAddDto, TvShow>();
            CreateMap<TvShowForEditDto, TvShow>();
            CreateMap<TvShowCountDto, TvShow>();
            CreateMap<TvShow, TvShowForListDto>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Episodes
                    .Where(e => e.SeasonNumber == 1 && e.EpisodeNumber == 1)
                    .Min(e => e.ReleaseDate)));
            CreateMap<EpisodeForAddDto, Episode>();
            CreateMap<EpisodeForEditDto, Episode>();
            CreateMap<ReleasePlaceTypeForAddDto, ReleasePlaceType>();
            CreateMap<ReleasePlaceTypeForEditDto, ReleasePlaceType>();
            CreateMap<ReleasePlaceForAddDto, ReleasePlace>();
            CreateMap<ReleasePlaceForEditDto, ReleasePlace>();
            CreateMap<GenreForAddDto, Genre>();
            CreateMap<GenreForEditDto, Genre>();
            CreateMap<UniverseForAddDto, Universe>();
            CreateMap<UniverseForEditDto, Universe>();
            CreateMap<MediaGalleryTypeForAddDto, MediaGalleryType>();
            CreateMap<MediaGalleryTypeForEditDto, MediaGalleryType>();
            CreateMap<GalleryForAddDto, Gallery>();
            CreateMap<GalleryForEditDto, Gallery>();
            CreateMap<NotificationTypeForAddDto, NotificationType>();
            CreateMap<NotificationTypeForEditDto, NotificationType>();
            CreateMap<NotificationForAddDto, Notification>();
            CreateMap<NotificationForEditDto, Notification>();
            CreateMap<RatingCategoryForAddDto, RatingCategory>();
            CreateMap<RatingCategoryForEditDto, RatingCategory>();
            CreateMap<RatingForAddDto, Rating>();
            CreateMap<RatingForEditDto, Rating>();
            CreateMap<TechnologyTypeForAddDto, TechnologyType>();
            CreateMap<TechnologyTypeForEditDto, TechnologyType>();
            CreateMap<TechnologyForAddDto, Technology>();
            CreateMap<TechnologyForEditDto, Technology>();
            CreateMap<UserTypeForAddDto, UserType>();
            CreateMap<UserTypeForEditDto, UserType>();
            CreateMap<UserCollectionTypeForAddDto, UserCollectionType>();
            CreateMap<UserCollectionTypeForEditDto, UserCollectionType>();
            CreateMap<UserCollectionForAddDto, UserCollection>();
            CreateMap<UserCollectionForEditDto, UserCollection>();
            CreateMap<ProductionTypeForAddDto, ProductionType>();
            CreateMap<ProductionTypeForEditDto, ProductionType>();
            CreateMap<ProductionVersionForAddDto, ProductionVersion>();
            CreateMap<ProductionVersionForEditDto, ProductionVersion>();
            CreateMap<CommentForAddDto, Comment>();
            CreateMap<CommentForEditDto, Comment>();
            CreateMap<ProductionVersion, ProductionVersionDetailsDto>();
        }
    }
}