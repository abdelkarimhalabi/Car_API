namespace CarMaintenanceGarage_API.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public int MakerId { get; set; }
        public int UserId { get; set; }
        public bool Available { get; set; }

        public Maker Maker { get; set; }
        public User User { get; set; }
        public List<MaintenanceGarage> MaintenanceGarages { get; set; } = new List<MaintenanceGarage>();
    }
}
