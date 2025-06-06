﻿using Thunders.TechTest.ApiService.Enumerators;

namespace Thunders.TechTest.ApiService.Dto
{
    public class RegisterUseDto
    {
        public int TollStationId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime UsedAt { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
