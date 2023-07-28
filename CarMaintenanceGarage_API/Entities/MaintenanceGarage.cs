namespace CarMaintenanceGarage_API.Entities
{
    public class MaintenanceGarage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
