using Lab1.Domain.Models;

namespace Lab1.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>()
        {
            new User() { Id = 1, Name = "Elia Helou", Email = "eliahelou2003@gmail.com" },
            new User() { Id = 2, Name = "John Doe", Email = "Johndoe@gmail.com" },
            new User() { Id = 3, Name = "Jane Smith", Email = "JaneSmith@gmail.com" },
        };

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public User? GetById(long id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public List<User> GetUsersByName(string name)
        {
            return _users.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool DeleteUser(long id)
        {
            var _user = _users.FirstOrDefault(x => x.Id == id);
            if (_user == null)
            {
                return false;
            }
            _users.Remove(_user);
            return true;
        }

        public bool UpdateUser(User user)
        {
            var _user = _users.FirstOrDefault(x => x.Id == user.Id);

            if (_user == null)
            {
                return false;
            }

            _user.Name = user.Name;
            _user.Email = user.Email;

            return true;
        }

    }
}
