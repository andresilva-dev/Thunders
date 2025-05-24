using FluentValidation;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class RegisterUseService : IRegisterUseService
    {
        private readonly IRegisterUseRepository _registerUseRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ITollStationRepository _tollStationRepository;

        public RegisterUseService(IRegisterUseRepository registerUseRepository, IStateRepository stateRepository, 
            ICityRepository cityRepository, ITollStationRepository tollStationRepository)
        {
            _registerUseRepository = registerUseRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _tollStationRepository = tollStationRepository;
        }

        public async Task AddRegisterUseAsync(RegisterUseDto registerUseDto)
        {
            var city = await _cityRepository.GetByIdAsync(registerUseDto.CityId);
            if (city == null)
            {
                throw new ValidationException($"City with ID: {registerUseDto.CityId} not found.");
            }

            var state = await _stateRepository.GetByIdAsync(registerUseDto.StateId);
            if (state == null)
            {
                throw new ValidationException($"State with ID: {registerUseDto.StateId} not found.");
            }

            var tollStation = await _tollStationRepository.GetByIdAsync(registerUseDto.TollStationId);
            if (tollStation == null)
            {
                throw new ValidationException($"TollStation with ID: {registerUseDto.TollStationId} not found.");
            }

            var entity = new RegisterUse
            {
                UsedAt = DateTime.UtcNow,  
                TollStationId = registerUseDto.TollStationId,
                CityId = registerUseDto.CityId,
                StateId = registerUseDto.StateId,
                AmountPaid = registerUseDto.AmountPaid,
                VehicleType = registerUseDto.VehicleType
            };

            await _registerUseRepository.AddRegisterUseAsync(entity);
        }

        public Task GetTopTollStationsByMonthAsync(int month, int year, int top)
        {
            throw new NotImplementedException();
        }

        public Task GetTotalAmountPerHourByCityAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetQtdVehicleTypesByTollStationAsync(string tollStationName)
        {
            throw new NotImplementedException();
        }
    }
}
