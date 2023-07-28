namespace CarMaintenanceGarage_API.Requests
{
    public class CreateCarRequest
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public int MakerId { get; set; }
    }
}
