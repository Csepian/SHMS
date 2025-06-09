using SHMS.DTO;
using SHMS.Model;

namespace SHMS.Repositories
{
    public interface IUser
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        IEnumerable<User> GetUsersByHotel(string hotelName);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task AssignManagerToHotel(int hotelId, int managerId);
        bool UserExists(int id);
        IEnumerable<User> GetUsersByRole(string role);
        Task<string> PatchUserAsync(int id, NameEmailContactNumber_patch patch);

    }
}
