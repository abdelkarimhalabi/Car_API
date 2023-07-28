
using CarMaintenanceGarage_API.Requests;
using CarMaintenanceGarage_API.Response;
using CarMaintenanceGarage_API.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarMaintenanceGarage_API.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _conf;
        private BLC _blc;
        private JWT_AUTH jwtAuth;

        public AuthController(IConfiguration configuration , BLC blc)
        {
            this._conf = configuration;
            this._blc = blc;
            this.jwtAuth = new JWT_AUTH(_conf["SecretKey"]);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> userLogin(CancellationToken cancellationToken , [FromBody] LoginRequest loginRequest)
        {
            try
            {
                await Task.Delay(3000 , cancellationToken);
                var tokenResposne = await this._blc.GetLogin(loginRequest);
                Console.WriteLine(tokenResposne);
                if (tokenResposne != null) 
                    return Ok(new TokenResponse { Token = this.jwtAuth.EncodeToken(tokenResposne)});

                return Unauthorized();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
