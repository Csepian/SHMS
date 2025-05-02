using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
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
    }
}
