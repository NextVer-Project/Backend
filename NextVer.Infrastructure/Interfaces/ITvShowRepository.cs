using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Helpers;
using NextVer.Infrastructure.Helpers.PaginationParameters;

namespace NextVer.Infrastructure.Interfaces
{
    public interface ITvShowRepository
    {
        Task<PagedList<TvShowForListDto>> GetTvShows(ProductionParameters parameters);
        Task<int> GetSeasonDurationForTvShowInMinutes(int tvShowId, int seasonNum);
        Task<int> GetTotalDurationForTvShowInMinutes(int tvShowId);
        Task<int> GetSeasonDurationForTvShowInSeconds(int tvShowId, int seasonNum);
        Task<int> GetTotalDurationForTvShowInSeconds(int tvShowId);
        Task<Episode> GetEpisodeByNumberAndSeason(int tvShowId, int seasonNumber, int episodeNumber);
    }
}