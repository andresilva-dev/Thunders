using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;

namespace Thunders.TechTest.ApiService.Repository.Implementation
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CityRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> AddAsync(City city)
        {
            _applicationDbContext.Cities.Add(city);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var city = await _applicationDbContext.Cities.FindAsync(id);

            if (city == null)
                throw new KeyNotFoundException($"City with ID {id} not found.");

            _applicationDbContext.Cities.Remove(city);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _applicationDbContext.Cities
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<City?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(int id, City city)
        {
            var cityExisting = await _applicationDbContext.Cities.FindAsync(id);
            if (cityExisting is null)
                throw new InvalidOperationException($"City with id {id} not found.");

            cityExisting.Name = city.Name;
            cityExisting.StateId = city.StateId;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
