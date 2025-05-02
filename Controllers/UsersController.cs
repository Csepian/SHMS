using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repository;
using SHMS.Service;

namespace SHMS.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class UsersController : ControllerBase
    //{
    //    private readonly SHMSContext _context;

    //    public UsersController(SHMSContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: api/Users
    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    //    {
    //        return await _context.Users.ToListAsync();
    //    }

    //    // GET: api/Users/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<User>> GetUser(int id)
    //    {
    //        var user = await _context.Users.FindAsync(id);

    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        return user;
    //    }

    //    // PUT: api/Users/5
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutUser(int id, User user)
    //    {
    //        if (id != user.UserID)
    //        {
    //            return BadRequest();
    //        }

    //        _context.Entry(user).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!UserExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/Users
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<User>> PostUser(User user)
    //    {
    //        _context.Users.Add(user);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction("GetUser", new { id = user.UserID }, user);
    //    }

    //    // DELETE: api/Users/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteUser(int id)
    //    {
    //        var user = await _context.Users.FindAsync(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.Users.Remove(user);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool UserExists(int id)
    //    {
    //        return _context.Users.Any(e => e.UserID == id);
    //    }
    //}
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;

        public UserController(IUser context)
        {
            _userService = context;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!_userService.UserExists(id))
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }

}


