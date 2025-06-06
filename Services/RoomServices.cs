using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.DTO;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services
{
    public class RoomServices : IRoom
    {
        private readonly SHMSContext _context;

        public RoomServices(SHMSContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetRooms()
        {
            return _context.Rooms.Include(r => r.Hotel).ToList();
        }
        public IEnumerable<Room> GetRoomsByHotelId(int hotelId)
        {
            return _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.HotelID == hotelId)
                .ToList();
        }


        public Room GetRoomById(int id)
        {
            return _context.Rooms.Include(r => r.Hotel).FirstOrDefault(r => r.RoomID == id);
        }

        public async Task AddRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public bool RoomExists(int id)
        {
            return _context.Rooms.Any(r => r.RoomID == id);
        }

        public IQueryable<Room> SearchRooms(string? type, decimal? minPrice, decimal? maxPrice, bool? availability)
        {
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(r => r.Type.Contains(type));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(r => r.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= maxPrice.Value);
            }

            if (availability.HasValue)
            {
                query = query.Where(r => r.Availability == availability.Value);
            }

            return query;
        }
        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkInDate, DateTime checkOutDate)
        {
            // Validate input dates
            if (checkInDate >= checkOutDate)
            {
                throw new ArgumentException("Check-out date must be after check-in date.");
            }

            return await _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.HotelID == hotelId && r.Availability &&
                    !_context.Bookings.Any(b =>
                        b.RoomID == r.RoomID &&
                        b.Status != "Cancelled" && // Only consider non-cancelled bookings
                        (
                            (checkInDate >= b.CheckInDate && checkInDate < b.CheckOutDate) ||
                            (checkOutDate > b.CheckInDate && checkOutDate <= b.CheckOutDate) ||
                            (checkInDate <= b.CheckInDate && checkOutDate >= b.CheckOutDate)
                        )
                    )
                )
                .ToListAsync();
        }
        public async Task<string> PatchRoomAsync(int id, RoomDTO patch)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return "Room not found.";

            if (!string.IsNullOrEmpty(patch.Type)) room.Type = patch.Type;
            if (patch.Price != default) room.Price = patch.Price;
            if (patch.Availability != room.Availability) room.Availability = patch.Availability;
            if (!string.IsNullOrEmpty(patch.Features)) room.Features = patch.Features;
            // Add more fields as needed

            await _context.SaveChangesAsync();
            return "Room updated successfully.";
        }


    }
}
