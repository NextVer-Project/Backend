using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
﻿using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Helpers;
using NextVer.Infrastructure.Helpers.PaginationParameters;
using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Persistance;

namespace NextVer.Infrastructure.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        private readonly NextVerDbContext _context;
        private readonly IMapper _mapper;

        public MovieRepository(NextVerDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<MovieForListDto>> GetMovies(ProductionParameters parameters)
        {
            try
            {
                var movies = _context.Movies.Where(x => x.ReleaseDate < DateTime.UtcNow)
                    .OrderByDescending(x => x.ReleaseDate)
                    .ProjectTo<MovieForListDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking();

                return await PagedList<MovieForListDto>.CreateAsync(movies, parameters.PageNumber,
                    parameters.PageSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<ProductionVersionDetailsDto>> GetProductionVersionsWithDetailsByMovieId(int movieId)
        {
            try
            {
                return await _context.ProductionVersions
                    .Where(pv => pv.IdMovieTVSerieGame == movieId && pv.ProductionTypeId == 1)
                    .ProjectTo<ProductionVersionDetailsDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IEnumerable<ReleasePlace>> GetProductionVersionsVodByMovieId(int movieId)
        {
            try
            {
                var result = _context.ReleasePlaces
                    .Where(r => _context.ProductionVersions
                        .Where(p => p.IdMovieTVSerieGame == movieId)
                        .Select(p => p.ReleasePlaceId)
                        .Contains(r.Id))
                    .ToList();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesByProductionVersionId(int productionVersionId)
        {
            try
            {
                var result = await _context.Technologies
                    .Where(t => _context.ProductionTechnologies
                        .Where(pt => pt.ProductionVersionId == productionVersionId)
                        .Select(pt => pt.TechnologyId)
                        .Contains(t.Id))
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}