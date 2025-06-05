using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.DTOs;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services
{
    public class HotelServices : IHotel
    {
        private readonly SHMSContext _context;
        public HotelServices(SHMSContext context)
        {
            _context = context;
        }
        public IEnumerable<Hotel> GetHotels()
        {
            return _context.Hotels.Include(h => h.Rooms).Include(h => h.Reviews).ToList();
        }
        public Hotel GetHotelById(int id)
        {
            return _context.Hotels.Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .FirstOrDefault(a => a.HotelID == id);
        }
        public async Task AddHotelAsync(Hotel hotel)
        {
            // Validate that the ManagerID belongs to a user with the "manager" role
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.UserID == hotel.ManagerID && u.Role == "manager");
            if (manager == null)
            {
                throw new InvalidOperationException("The specified ManagerID does not belong to a user with the 'manager' role.");
            }

            // Add the hotel to the database
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateHotelAsync(Hotel hotel)
        {
            // Validate that the ManagerID belongs to a user with the "manager" role
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.UserID == hotel.ManagerID && u.Role == "manager");
            if (manager == null)
            {
                throw new InvalidOperationException("The specified ManagerID does not belong to a user with the 'manager' role.");
            }

            // Update the hotel in the database
            _context.Entry(hotel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }



        public async Task DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }

        public bool HotelExists(int id)
        {
            return _context.Hotels.Any(h => h.HotelID == id);
        }

        public IQueryable<Hotel> SearchHotels(string? location, string? amenities)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(h => h.Location.Contains(location));
            }

            if (!string.IsNullOrEmpty(amenities))
            {
                var amenitiesList = amenities.Split(',');
                foreach (var amenity in amenitiesList)
                {
                    query = query.Where(h => h.Amenities.Contains(amenity));
                }
            }

            return query;
        }
        public Hotel GetHotelsByManagerId(int managerId)
        {
            return _context.Hotels
                .FirstOrDefault(h => h.ManagerID == managerId);
        }

        public async Task<IEnumerable<object>> GetHotelsWithAvailableRoomsAsync()
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.Rooms.Any(r => r.Availability)) // Filter hotels with available rooms
                .Select(h => new
                {
                    HotelID = h.HotelID,
                    HotelName = h.Name,
                    Location = h.Location,
                    AvailableRoomsCount = h.Rooms.Count(r => r.Availability) // Count available rooms
                })
                .ToListAsync();
        }

        public Hotel GetHotelByName(string name)
        {
            return _context.Hotels.Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .FirstOrDefault(a => a.Name == name);
        }
        public async Task<string> PatchHotelAsync(int id, HotelDTO patch)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return "Hotel not found.";

            if (!string.IsNullOrEmpty(patch.Name)) hotel.Name = patch.Name;
            if (!string.IsNullOrEmpty(patch.Location)) hotel.Location = patch.Location;
            if (patch.ManagerID.HasValue) hotel.ManagerID = patch.ManagerID;
            if (!string.IsNullOrEmpty(patch.Amenities)) hotel.Amenities = patch.Amenities;
            // Add more fields as needed

            await _context.SaveChangesAsync();
            return "Hotel updated successfully.";
        }

    }
}