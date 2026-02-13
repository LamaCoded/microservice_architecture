using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace userservice.helper
{
    public class AuthHelper
    {
        private readonly string _jwtSecret;

        public AuthHelper(IConfiguration configuration)
        {
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                         ?? configuration["Jwt:Key"]
                         ?? throw new Exception("JWT secret key not configured");
        }

        public string GenerateJwtToken(string userId, string username)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSecret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username)
        }),
                Expires = DateTime.UtcNow.AddMinutes(30),

                Issuer = "AuthService",       
                Audience = "Microservices",  

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (string UserId, string Username) DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token.Replace("Bearer ", ""));

            var userId = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return (userId, username);
        }



    }
}
