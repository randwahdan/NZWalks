using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalksAsync(string? filterOn=null , string? filterQuery = null,string? sortBy=null,bool isAscending=true,int pageNumber=1,int pageSize=1000);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk?> UpdateWalksAsync(Guid id , Walk walk);
        Task<Walk?> DeleteWalkByIdAsync(Guid id);


    }
}
