using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;

namespace Thunders.TechTest.ApiService.Repository.Implementation
{
    public class TollStationRepository : ITollStationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TollStationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> AddAsync(TollStation tollStation)
        {
            _applicationDbContext.TollStations.Add(tollStation);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tollStation = await _applicationDbContext.TollStations.FindAsync(id);

            if (tollStation == null)
                throw new KeyNotFoundException($"TollStation with ID {id} not found.");

            _applicationDbContext.TollStations.Remove(tollStation);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TollStation>> GetAllAsync()
        {
            return await _applicationDbContext.TollStations
                .Include(t => t.City)
                .ThenInclude(c => c.State)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TollStation> GetByIdAsync(int id)
        {
            return await _applicationDbContext.TollStations
                .Include(t => t.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.Id == id);
        }

        public async Task UpdateAsync(int id, TollStation tollStation)
        {
            var tollStationExisting = await _applicationDbContext.TollStations.FindAsync(id);
            if (tollStationExisting != null)
            {
                tollStationExisting.Name = tollStation.Name;
                tollStationExisting.CityId = tollStation.CityId;

                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
