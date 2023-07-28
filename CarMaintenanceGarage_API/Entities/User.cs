namespace CarMaintenanceGarage_API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int PhoneNumber { get; set; }
        public int LoginId { get; set; }
        public Login Login { get; set; }
        public List<Car> Cars { get; set; } 
    }
}
