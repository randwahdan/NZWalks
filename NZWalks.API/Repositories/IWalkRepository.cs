using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalksAsync();
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk?> UpdateWalksAsync(Guid id , Walk walk);
        Task<Walk?> DeleteWalkByIdAsync(Guid id);


    }
}
