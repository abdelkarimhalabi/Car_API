using CarMaintenanceGarage_API.Entities;
using System.Threading.Tasks;

namespace CarMaintenanceGarage_API.DALC
{
    public interface IDALC
    {
        //Login Functions
        public  Task<Login> GetLogin(string email ,  string password);
        public Task<Login> GetLoginById(int loginId);

        //User Functions
        public Task<User> GetUserByLoginId(int loginId);

        //Car Functions
        public Task<int> CreateCar(Car car);
        public Task<Car> GetCarById(int carId);
        public Task<List<Car>> GetCarsByModelName(string modelName);
        public Task<List<Car>> GetCarsByUserId(int userId);
        public Task<List<Car>> GetCarsByGarageId(int GarageId);
        public Task<Car> EditCar(Car car);
        public Task<bool> DeleteCar(int carId , int userId);

        //Maintenance Garage Functions
        public Task<MaintenanceGarage> GetMaintenanceGarageById(int maintenanceGarageId);
        public Task<List<MaintenanceGarage>> GetMaintenanceGaragesByCarId (int carId);
        public Task<MaintenanceGarage> EditMaintenanceGarage(MaintenanceGarage maintenanceGarage);
    }
}
