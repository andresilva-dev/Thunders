using FluentValidation;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Implementation;
using Thunders.TechTest.ApiService.Repository.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class TollStationService : ITollStationService
    {
        private readonly ITollStationRepository _tollStationRepository;
        private readonly ICityRepository _cityRepository;

        public TollStationService(ITollStationRepository tollStationRepository, ICityRepository cityRepository)
        {
            _tollStationRepository = tollStationRepository;
            _cityRepository = cityRepository;   
        }

        public async Task<int> AddAsync(TollStationDto tollStationDto)
        {
            var city = await _cityRepository.GetByIdAsync(tollStationDto.CityId);
            if (city == null)
            {
                throw new ValidationException($"City with ID: {tollStationDto.CityId} not found.");
            }

            var tollStation = new TollStation { Name = tollStationDto.Name, CityId = tollStationDto.CityId };
            return await _tollStationRepository.AddAsync(tollStation);
        }

        public async Task<IEnumerable<TollStation>> GetAllAsync()
        {
            return await _tollStationRepository.GetAllAsync();
        }

        public async Task<TollStation> GetByIdAsync(int id)
        {
            return await _tollStationRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int id, TollStationDto tollStationDto)
        {
            var city = await _cityRepository.GetByIdAsync(tollStationDto.CityId);
            if (city == null)
            {
                throw new ValidationException($"City with ID: {tollStationDto.CityId} not found.");
            }

            var tollStation = new TollStation() { CityId = tollStationDto.CityId, Name = tollStationDto.Name };
            await _tollStationRepository.UpdateAsync(id, tollStation);
        }
    }
}
