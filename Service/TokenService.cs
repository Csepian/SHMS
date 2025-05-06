using Microsoft.IdentityModel.Tokens;
using SHMS.DTO;
using SHMS.Model;
using SHMS.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SHMS.Service
{
    public class TokenService : ITokenGenerate
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]!));
        }

        public string GenarateToken(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Name!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            return GenerateJwtToken(claims);
        }

        public string GenerateToken(LoginDto loginDto)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, loginDto.Email),
                new Claim(ClaimTypes.Role, loginDto.Role)
            };

            return GenerateJwtToken(claims);
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

