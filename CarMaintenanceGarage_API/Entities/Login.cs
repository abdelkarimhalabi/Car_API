namespace CarMaintenanceGarage_API.Entities
{
    public class Login
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public User User { get; set; }
    }
}
