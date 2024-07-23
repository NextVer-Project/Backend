
namespace NextVer.Infrastructure.Interfaces
{
    public interface IRatingRepository
    {
        Task<double> GetAverageRating(int productionId, int productionTypeId);
        Task<int> GetRatingCount(int productionId, int productionTypeId);
    }
}
