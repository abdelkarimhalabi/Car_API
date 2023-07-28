namespace CarMaintenanceGarage_API.Entities
{
    public class Maker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Car> Cars { get; set; }
    }
}
