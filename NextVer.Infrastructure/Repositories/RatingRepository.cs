using AutoMapper;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Persistance;

namespace NextVer.Infrastructure.Repositories
{
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        private readonly NextVerDbContext _context;
        private readonly IMapper _mapper;

        public RatingRepository(NextVerDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<double> GetAverageRating(int productionId, int productionTypeId)
        {
            try
            {
                var result = _context.Ratings
                    .Where(r => r.ProductionTypeId == productionTypeId && r.IdMovieTVSerieGame == productionId)
                    .Average(r => (double?)r.RatingValue) ?? 0.0;

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public async Task<int> GetRatingCount(int productionId, int productionTypeId)
        {
            try
            {
                var count = _context.Ratings
                    .Where(r => r.ProductionTypeId == productionTypeId && r.IdMovieTVSerieGame == productionId)
                    .Count();

                return count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }
    }
}
