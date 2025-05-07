using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.DTO;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;

        public UserController(IUser context)
        {
            _userService = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,manager,guest")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("by-hotel-name/{hotelName}")]
        [Authorize(Roles = "admin,manager")]
        public ActionResult<IEnumerable<User>> GetUsersByHotelName(string hotelName)
        {
            var users = _userService.GetUsersByHotel(hotelName);
            if (!users.Any())
            {
                return NotFound($"No users found for hotel with name '{hotelName}'.");
            }
            return Ok(users);
        }



        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "admin,manager,guest")]
        public async Task<ActionResult<User>> PostUser(UserDTO userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role,
                ContactNumber = userDto.ContactNumber
            };
            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,manager,guest")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                await _userService.UpdateUserAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userService.UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id, User user)
        {

            await _userService.DeleteUserAsync(id);
            return NoContent();

        }
        [HttpPost("assign-manager")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AssignManagerToHotel(int hotelId, int managerId)
        {
            try
            {
                await _userService.AssignManagerToHotel(hotelId, managerId);
                return Ok("Manager assigned to hotel successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
