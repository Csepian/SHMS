using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.DTO;
using SHMS.Model;
using SHMS.Repository;



namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly SHMSContext _con;
        private readonly ITokenGenerate _tokenService;

        public TokenController(SHMSContext con, ITokenGenerate tokenService)
        {
            _con = con;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginDto loginDto)
        {
            if (loginDto != null && !string.IsNullOrEmpty(loginDto.Email) && !string.IsNullOrEmpty(loginDto.Password))
            {
                var user = await GetUser(loginDto.Email, loginDto.Password, loginDto.Role);

                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    // Generate token using the User entity
                    var token = _tokenService.GenerateToken(user);
                    return Ok(new { token });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Invalid request data");
            }
        }

        private async Task<User?> GetUser(string email, string password, string role)
        {
            return await _con.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.Role == role);
        }
    }
}
