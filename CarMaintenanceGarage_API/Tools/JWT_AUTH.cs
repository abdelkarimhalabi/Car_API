using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CarMaintenanceGarage_API.Tools
{
    public class JWT_AUTH
    {
        private string SecretKey;
        public JWT_AUTH(string secretKey)
        {
            this.SecretKey = secretKey;
        }

        public string EncodeToken(TokenData data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", data.UserId.ToString()),
                    new Claim("isAdmin", data.IsAdmin.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public TokenData DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var userId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
                var isAdmin = bool.Parse(claimsPrincipal.FindFirst("isAdmin").Value);
                return new TokenData
                {
                    UserId = userId,
                    IsAdmin = isAdmin
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }

    public class TokenData
    {
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public override string ToString()
        {
            return $"UserId: {UserId}, IsAdmin: {IsAdmin}";
        }
    }

}
