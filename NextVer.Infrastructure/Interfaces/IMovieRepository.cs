using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Helpers;
using NextVer.Infrastructure.Helpers.PaginationParameters;

namespace NextVer.Infrastructure.Interfaces
{
    public interface IMovieRepository
    {
        Task<PagedList<MovieForListDto>> GetMovies(ProductionParameters parameters);
        Task<List<ProductionVersionDetailsDto>> GetProductionVersionsWithDetailsByMovieId(int movieId);
        Task<IEnumerable<ReleasePlace>> GetProductionVersionsVodByMovieId(int movieId);
        Task<IEnumerable<Technology>> GetTechnologiesByProductionVersionId(int productionVersionId);
    }
}
