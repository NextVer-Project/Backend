using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Helpers;
using NextVer.Infrastructure.Helpers.PaginationParameters;
using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Persistance;

namespace NextVer.Infrastructure.Repositories
{
    public class TvShowRepository : BaseRepository<TvShow>, ITvShowRepository
    {
        private readonly NextVerDbContext _context;
        private readonly IMapper _mapper;

        public TvShowRepository(NextVerDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<TvShowForListDto>> GetTvShows(ProductionParameters parameters)
        {
            try
            {
                var tvShows = _context.TvShows
                    .Where(x => x.Episodes.Any(e => e.SeasonNumber == 1 && e.EpisodeNumber == 1 && e.ReleaseDate < DateTime.UtcNow))
                    .OrderBy(x => x.Episodes
                        .Where(e => e.SeasonNumber == 1 && e.EpisodeNumber == 1)
                        .Min(e => e.ReleaseDate))
                    .ProjectTo<TvShowForListDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking();



                return await PagedList<TvShowForListDto>.CreateAsync(tvShows, parameters.PageNumber, parameters.PageSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<int> GetSeasonDurationForTvShowInMinutes(int tvShowId, int seasonNum)
        {
            try
            {
                return await _context.Episodes
                    .Where(x => x.TvShowId == tvShowId && x.SeasonNumber == seasonNum)
                    .AsNoTracking()
                    .Select(x => (x.Duration.Hours * 60) + x.Duration.Minutes + x.Duration.Seconds)
                    .SumAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<int> GetTotalDurationForTvShowInMinutes(int tvShowId)
        {
            try
            {
                return await _context.Episodes
                    .Where(x => x.TvShowId == tvShowId)
                    .AsNoTracking()
                    .Select(x => (x.Duration.Hours * 60) + x.Duration.Minutes)
                    .SumAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<int> GetSeasonDurationForTvShowInSeconds(int tvShowId, int seasonNum)
        {
            try
            {
                return await _context.Episodes
                    .Where(x => x.TvShowId == tvShowId && x.SeasonNumber == seasonNum)
                    .AsNoTracking()
                    .SumAsync(x => x.Duration.Hours * 3600 + x.Duration.Minutes * 60 + x.Duration.Seconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<int> GetTotalDurationForTvShowInSeconds(int tvShowId)
        {
            try
            {
                return await _context.Episodes
                      .Where(x => x.TvShowId == tvShowId)
                      .AsNoTracking()
                      .SumAsync(x => x.Duration.Hours * 3600 + x.Duration.Minutes * 60 + x.Duration.Seconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<Episode> GetEpisodeByNumberAndSeason(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return await _context.Episodes
                .FirstOrDefaultAsync(e => e.TvShowId == tvShowId
                                          && e.SeasonNumber == seasonNumber
                                          && e.EpisodeNumber == episodeNumber);
        }
    }
}