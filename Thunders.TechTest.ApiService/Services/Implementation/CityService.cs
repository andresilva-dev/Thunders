using FluentValidation;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;

        public CityService(ICityRepository cityRepository, IStateRepository stateRepository)
        {
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
        }

        public async Task<int> AddAsync(CityDto cityDto)
        {
            var state = await _stateRepository.GetByIdAsync(cityDto.StateId);
            if (state == null)
            {
                throw new ValidationException($"State with ID: {cityDto.StateId} not found.");
            }

            var city = new City { Name = cityDto.Name,  StateId = cityDto.StateId };
            return await _cityRepository.AddAsync(city);
        }

        public async Task DeleteAsync(int id)
        {
            await _cityRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<City> GetByIdAsync(int id)
        {
            return await _cityRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int id, CityDto cityDto)
        {
            var state = await _stateRepository.GetByIdAsync(cityDto.StateId);
            if (state == null)
            {
                throw new ValidationException($"State with ID: {cityDto.StateId} not found.");
            }

            var city = new City { Name = cityDto.Name, StateId = cityDto.StateId };
            await _cityRepository.UpdateAsync(id, city);
        }
    }
}
