using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly ICacheService _cacheService;

        public RegisterUseService(IRegisterUseRepository registerUseRepository, IStateRepository stateRepository, 
            ICityRepository cityRepository, ITollStationRepository tollStationRepository, ICacheService cacheService)
        {
            _registerUseRepository = registerUseRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _tollStationRepository = tollStationRepository;
            _cacheService = cacheService;
        }

        public async Task<int> AddRegisterUseAsync(RegisterUseDto registerUseDto)
        {
            var tollStation = await _tollStationRepository.GetByIdAsync(registerUseDto.TollStationId);
            if (tollStation == null)
            {
                throw new ValidationException($"TollStation with ID: {registerUseDto.TollStationId} not found.");
            }

            var entity = new RegisterUse
            {
                UsedAt = registerUseDto.UsedAt,
                TollStationId = registerUseDto.TollStationId,
                AmountPaid = registerUseDto.AmountPaid,
                VehicleType = registerUseDto.VehicleType
            };

            var id = await _registerUseRepository.AddRegisterUseAsync(entity);
            entity = await _registerUseRepository.GetByIdAsync(entity.Id);
            await _cacheService.SetAsync(entity.Id.ToString(), entity);

            return id;
        }
    }
}
