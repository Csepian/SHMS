using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.DTO;
using SHMS.Model;

namespace SHMS.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly SHMSContext _context;
        private readonly ITokenGenerate _tokenService;
        public TokenController(SHMSContext context, ITokenGenerate tokenService)
        {
            _context = context;
            _tokenService = tokenService;

        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginDTO userData)
        {
            if (userData != null && !string.IsNullOrEmpty(userData.Email) && !string.IsNullOrEmpty(userData.Password))
            {
                var user = await GetUser(userData.Email, userData.Password, userData.Role);

                if (user != null)
                {
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

        private async Task<User> GetUser(string email, string password, string role)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.Role == role) ?? new Model.User();
        }
    }
}