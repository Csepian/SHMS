using SHMS.Model;

namespace SHMS.Repository
{
    public interface IUser
    {
     
            IEnumerable<User> GetAllUsers();
            User GetUserById(int id);
            Task AddUserAsync(User user);
            Task UpdateUserAsync(User user);
            Task DeleteUserAsync(int id);
            bool UserExists(int id);
       

    }

}
