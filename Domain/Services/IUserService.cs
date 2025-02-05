using Lab1.Domain.Models;

namespace Lab1.Domain.Services
{
    public interface IUserService
    {
        User? GetById(long id);
        List<User> GetAllUsers();
        List<User> GetUsersByName(string name);
        bool DeleteUser(long id);
        bool UpdateUser(User user);
    }
}
