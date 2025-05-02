using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SHMS.Model;

namespace SHMS.Repositories
{
    public interface IRoom
    {
        IEnumerable<Room> GetRooms();
        Room GetRoomById(int id);
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        bool RoomExists(int id);
        IQueryable<Room> SearchRooms(string? type, decimal? minPrice, decimal? maxPrice, bool? availability);
    }
}
