namespace Thunders.TechTest.ApiService.Entities
{
    public class TollStation : Base
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
