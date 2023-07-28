using CarMaintenanceGarage_API.Entities;
using CarMaintenanceGarage_API.Tools;
using CarMaintenanceGarage_API.Requests;
using CarMaintenanceGarage_API.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarMaintenanceGarage_API.Controllers
{
    [Route("Car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private IConfiguration _conf;
        private BLC _blc;
        private JWT_AUTH jwtAuth;

        public CarController(IConfiguration configuration, BLC blc)
        {
            this._conf = configuration;
            this._blc = blc;
            this.jwtAuth = new JWT_AUTH(_conf["SecretKey"]);
        }

        [Route("get/{carId}")]
        [HttpGet]
        public async Task<ActionResult<CarResponse>> getCarById(int carId, [FromHeader(Name = "Token")] string token , CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(3000, cancellationToken);
                var tokenData = this.jwtAuth.DecodeToken(token);
                if(tokenData is null) return Unauthorized("Invalid credentials");

                var car = await _blc.GetCarResponseByCarId(carId , tokenData.UserId);
                if(car is null) return NotFound($"Invalid car with id {carId}");
                 
                return Ok(car);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }


        [Route("delete/{carId}")]
        [HttpGet]
        public async Task<IActionResult> deleteCar(int carId , [FromHeader(Name = "Token")] string token, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(3000, cancellationToken);
                var tokenData = this.jwtAuth.DecodeToken(token);
                
                if(tokenData is null) return Unauthorized("Invalid credentials");    
                
                var result = await this._blc.DeleteCar(carId , tokenData.UserId);

                if (result) return Ok("Car deleted !!!");

                return BadRequest("Cannot delete car !!!");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult<CarResponse>> createCar([FromBody] CreateCarRequest createCarRequest,[FromHeader(Name = "Token")] string token , CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(3000, cancellationToken);
                var tokenData = this.jwtAuth.DecodeToken(token);

                if (tokenData is null) return Unauthorized("Invalid credentials");

                var result = await this._blc.CreateCar(createCarRequest , tokenData.UserId);
                if (result is null) return BadRequest("Cannot Create Car");

                return Ok(result);
            }
            catch(Exception ex)
            {
               Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
