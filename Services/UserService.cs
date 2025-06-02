using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services
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
        public IEnumerable<User> GetUsersByHotel(string hotelName)
        {
            return _context.Users
                .Include(u => u.Hotel)
                .Include(b => b.Bookings)
                .Where(u => u.Hotel != null && u.Hotel.Name == hotelName && u.Role == "manager")
                .ToList();
        }



        public async Task AddUserAsync(User user)
        {
            user.SetPassword(user.Password!);
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
        public async Task AssignManagerToHotel(int hotelId, int managerId)
        {
            // Validate that the user exists and has the role of "manager"
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.UserID == managerId && u.Role == "manager");
            if (manager == null)
            {
                throw new InvalidOperationException("The specified user is not a manager or does not exist.");
            }

            // Validate that the hotel exists
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.HotelID == hotelId);
            if (hotel == null)
            {
                throw new InvalidOperationException("The specified hotel does not exist.");
            }

            // Assign the manager to the hotel
            hotel.ManagerID = managerId;
            _context.Entry(hotel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
