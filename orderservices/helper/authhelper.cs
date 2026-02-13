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

        public (string UserId, string UserName)? DecodeJwtToken(string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_jwtSecret);

                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = "AuthService",

                    ValidateAudience = true,
                    ValidAudience = "Microservices",

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = principal.FindFirst(ClaimTypes.Name)?.Value;

                return (userId, username);
            }
            catch
            {
                return null; 
            }
        }


    }
}
