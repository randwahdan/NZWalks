using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public NZWalksDbContext NZWalksDbContext { get; }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();  
            return walk;
        }

        public async Task<List<Walk>> GetAllWalksAsync()
        {
            return  await nZWalksDbContext.Walks.Include("Difficulty").Include("Region").ToListAsync(); 
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateWalksAsync(Guid id, Walk walk)
        {
            var existingWalk= await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) 
            {
                return null;
            }

            existingWalk.Name=walk.Name;    
            existingWalk.Description=walk.Description;  
            existingWalk.LengthInKm= walk.LengthInKm;   
            existingWalk.WalksImageUrl= walk.WalksImageUrl;
            existingWalk.DifficultyId=walk.DifficultyId;
            existingWalk.RegionId=walk.RegionId;
            await nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk?> DeleteWalkByIdAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) 
            {
                return null;
            }
            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();  
            return existingWalk;    
        }
    }
}
