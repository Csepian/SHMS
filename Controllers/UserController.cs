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
        public async Task<ActionResult<User>> PostUser(UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Role = userDto.Role,
                ContactNumber = userDto.ContactNumber
            };
            // Hash the password
            user.SetPassword(userDto.Password!);

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,manager,guest")]
        public async Task<IActionResult> PutUser(int id, UserDTO userdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userdto.Name;
            user.Email = userdto.Email;
            user.Role = userdto.Role;
            user.ContactNumber = userdto.ContactNumber;

            // Hash the password if it is being updated
            if (!string.IsNullOrEmpty(userdto.Password))
            {
                user.SetPassword(userdto.Password);
            }

            await _userService.UpdateUserAsync(user);
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
