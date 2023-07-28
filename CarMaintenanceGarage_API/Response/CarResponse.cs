using CarMaintenanceGarage_API.Entities;

namespace CarMaintenanceGarage_API.Response
{
    public class CarResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public int MakerId { get; set; }
        public int UserId { get; set; }
        public bool Available { get; set; }
        public string MakerName { get; set; }

        public List<MaintenanceGarageResponse> maintenanceGarages { get; set; }
    }
}
