using CarMaintenanceGarage_API.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CarMaintenanceGarage_API.DALC
{
    public class MSSQL : IDALC
    {
        private IConfiguration _config;
        private string _connectionString;
        
        public MSSQL(IConfiguration configuration)
        {
            this._config = configuration;
            this._connectionString = _config["ConnectionString"];
        }
        public async Task<User> GetUserByLoginId(int loginId)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "GET_USER_BY_LOGIN_ID";
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@LOGIN_ID", loginId);
                        using (SqlDataReader reader = await _cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                    return new User
                                    {
                                        Id = reader.GetInt32("USER_ID"),
                                        FullName = reader.GetString("FULL_NAME"),
                                        PhoneNumber = reader.GetInt32("PHONE_NUMBER"),
                                        LoginId = reader.GetInt32("LOGIN_ID")
                                    };
                            }
                            return null;
                        }
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<Login> GetLogin(string email, string password)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "GET_LOGIN";
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@EMAIL", email);
                        _cmd.Parameters.AddWithValue("@PASSWORD", password);
                        using (SqlDataReader reader = await _cmd.ExecuteReaderAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                    return new Login
                                    {
                                        Id = reader.GetInt32("LOGIN_ID"),
                                        Email = reader.GetString("EMAIL"),
                                        Password = reader.GetString("PASSWORD"),
                                        IsAdmin = reader.GetBoolean("IS_ADMIN")
                                    };
                            }
                            return null;
                        }
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Login> GetLoginById(int loginId)
        {
            throw new NotImplementedException();
        }

        public async Task<MaintenanceGarage> GetMaintenanceGarageById(int maintenanceGarageId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MaintenanceGarage>> GetMaintenanceGaragesByCarId(int carId)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "GET_MAINTENANCE_GARAGE_BY_CAR_ID";
                    List<MaintenanceGarage> maintenanceGarages = new List<MaintenanceGarage>();
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@CAR_ID", carId);
                        using (SqlDataReader reader = await _cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    maintenanceGarages.Add(new MaintenanceGarage
                                    {
                                        Id = reader.GetInt32("GARAGE_ID"),
                                        Name = reader.GetString("GARAGE_NAME"),
                                    });
                                }

                                return maintenanceGarages;
                            }
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }


        public async Task<int> CreateCar(Car car)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "CREATE_CAR";
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@CAR_NAME", car.Name);
                        _cmd.Parameters.AddWithValue("@YEAR", car.Year);
                        _cmd.Parameters.AddWithValue("@MODEL", car.Model);
                        _cmd.Parameters.AddWithValue("@USER_ID", car.UserId);
                        _cmd.Parameters.AddWithValue("@MAKER_ID", car.MakerId);

                        return Convert.ToInt32(await _cmd.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteCar(int carId , int userId)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "DELETE_CAR";
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@CAR_ID", carId);
                        _cmd.Parameters.AddWithValue("@USER_ID", userId);
                        await _cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Car> EditCar(Car car)
        {
            throw new NotImplementedException();
        }

        public async Task<Car> GetCarById(int carId)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "GET_CAR_BY_ID";
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@ID", carId);
                        using (SqlDataReader reader = await _cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Car
                                {
                                    Id = reader.GetInt32("CAR_ID"),
                                    Name = reader.GetString("CAR_NAME"),
                                    Year = reader.GetInt32("YEAR"),
                                    Model = reader.GetString("Model"),
                                    MakerId = reader.GetInt32("MAKER_ID"),
                                    UserId = reader.GetInt32("USER_ID"),
                                    Available= reader.GetBoolean("AVAILABLE"),
                                    Maker = new Maker
                                    {
                                        Id = reader.GetInt32("MAKER_ID"),
                                        Name = reader.GetString("MAKER_NAME")
                                    },
                                    User = new User
                                    {
                                        Id = reader.GetInt32("USER_ID"),
                                        FullName = reader.GetString("FULL_NAME"),
                                        PhoneNumber = reader.GetInt32("PHONE_NUMBER")
                                    }
                                };
                            }
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Car>> GetCarsByGarageId(int GarageId)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    await _con.OpenAsync();
                    string SqlOperation = "GET_CARS_BY_MAINTENANCE_GARAGE_ID";
                    List<Car> cars = new List<Car>();
                    using (SqlCommand _cmd = new SqlCommand(SqlOperation, _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@MAINTENANCE_GARAGE_ID", GarageId);
                        using (SqlDataReader reader = await _cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    cars.Add(new Car
                                    {
                                        Id = reader.GetInt32("CAR_ID"),
                                        Name = reader.GetString("CAR_NAME"),
                                        Year = reader.GetInt32("YEAR"),
                                        Model = reader.GetString("Model"),
                                        MakerId = reader.GetInt32("MAKER_ID"),
                                        UserId = reader.GetInt32("USER_ID"),
                                        Available = reader.GetBoolean("AVAILABLE"),
                                        Maker = new Maker
                                        {
                                            Id = reader.GetInt32("MAKER_ID"),
                                            Name = reader.GetString("MAKER_NAME")
                                        },
                                        User = new User
                                        {
                                            Id = reader.GetInt32("USER_ID"),
                                            FullName = reader.GetString("FULL_NAME"),
                                            PhoneNumber = reader.GetInt32("PHONE_NUMBER")
                                        }
                                    });
                                }

                                return cars;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Car>> GetCarsByModelName(string modelName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Car>> GetCarsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<MaintenanceGarage> EditMaintenanceGarage(MaintenanceGarage maintenanceGarage)
        {
            throw new NotImplementedException();
        }
    }
}
