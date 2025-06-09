using SHMS.Model;

namespace SHMS.Repository
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

       

    }

}
