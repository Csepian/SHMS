using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repository;

namespace SHMS.Service
{
    public class UserService : IUser
    {

      
            private readonly SHMSContext _context;

            public UserService(SHMSContext context)
            {
                _context = context;
            }

            public IEnumerable<User> GetAllUsers()
            {
                return _context.Users.ToList();
            }

            public User GetUserById(int id)
            {
                return _context.Users.FirstOrDefault(u => u.UserID == id);
            }

            public async Task AddUserAsync(User user)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateUserAsync(User user)
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            public async Task DeleteUserAsync(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }

            public bool UserExists(int id)
            {
                return _context.Users.Any(u => u.UserID == id);
            }


    }



    }



