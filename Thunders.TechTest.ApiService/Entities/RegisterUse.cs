using Thunders.TechTest.ApiService.Enumerators;

namespace Thunders.TechTest.ApiService.Entities
{
    public class RegisterUse : Base
    {
        public int TollStationId { get; set; }

        public TollStation TollStation { get; set; } 

        public DateTime UsedAt { get; set; }

        public decimal AmountPaid { get; set; }

        public VehicleType VehicleType { get; set; }
    }
}
