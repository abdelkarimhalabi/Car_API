using CarMaintenanceGarage_API.DALC;
using CarMaintenanceGarage_API.Entities;
using CarMaintenanceGarage_API.Requests;
using CarMaintenanceGarage_API.Response;
using CarMaintenanceGarage_API.Tools;

namespace CarMaintenanceGarage_API
{
    public class BLC
    {
        private IDALC _dalc;

        public BLC(IDALC dalc)
        {
            this._dalc = dalc;
        }

        private async Task<List<CarResponse>> GetMaintenanceGaragesCars(int carageId)
        {
            List<Car> cars = await this._dalc.GetCarsByGarageId(carageId);
            List<CarResponse> result = new List<CarResponse>();

            foreach (Car car in cars)
            {
                result.Add(new CarResponse
                {
                    Id = car.Id,
                    Name = car.Name,
                    Year = car.Year,
                    Model = car.Model,
                    MakerId = car.MakerId,
                    UserId = car.UserId,
                    Available = car.Available,
                    MakerName = car.Maker.Name,
                    maintenanceGarages = await this.GetMaintenanceGarages(car.Id)
                });
            }
            return result;
        }
        private async Task<List<MaintenanceGarageResponse>> GetMaintenanceGarages(int carId)
        {
            List<MaintenanceGarage> maintenanceGarages = await this._dalc.GetMaintenanceGaragesByCarId(carId);
            List<MaintenanceGarageResponse> maintenanceGaragesResponse = new List<MaintenanceGarageResponse>();

            foreach (MaintenanceGarage maintenanceGarage in maintenanceGarages)
            {
                maintenanceGaragesResponse.Add(new MaintenanceGarageResponse
                {
                    Id = maintenanceGarage.Id,
                    Name = maintenanceGarage.Name
                });
            }

            return maintenanceGaragesResponse;
        }

        public async Task<TokenData> GetLogin(LoginRequest loginRequest)
        {
            Login loginResult = await this._dalc.GetLogin(loginRequest.Email, loginRequest.Password);
            Console.WriteLine(loginResult.Email);
            if (loginResult != null)
            {
                loginResult.User = await this._dalc.GetUserByLoginId(loginResult.Id);
                return new TokenData { UserId = loginResult.User.Id , IsAdmin = loginResult.IsAdmin};
            }
            return null;
        }

        public async Task<CarResponse> GetCarResponseByCarId(int carId , int userId)
        {
            Car car = await this._dalc.GetCarById(carId);
            if(car.UserId == userId)
            {
                CarResponse carResponse = new CarResponse
                {
                    Id=car.Id,
                    Name=car.Name,
                    Year = car.Year,
                    Model = car.Model,
                    MakerId = car.MakerId,
                    UserId = userId,
                    Available = car.Available,
                    MakerName = car.Maker.Name
                };

                carResponse.maintenanceGarages = await this.GetMaintenanceGarages(car.Id);
                return carResponse;
            }
            return null;
        }

        public async Task<CarResponse> CreateCar(CreateCarRequest createCarRequest , int userId)
        {
            int carId = await this._dalc.CreateCar(new Car
            {
                UserId = userId,
                Name = createCarRequest.Name,
                MakerId = createCarRequest.MakerId,
                Model = createCarRequest.Model,
                Year = createCarRequest.Year
            });
            var result = await this.GetCarResponseByCarId(carId, userId);
            Console.WriteLine(result.Name);
            return result;
        }

        public async Task<bool> DeleteCar(int carId , int userId)
        {
            return await this._dalc.DeleteCar(carId , userId);
        }
    }
}
