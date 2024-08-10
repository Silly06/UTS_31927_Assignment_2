using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Models.Match;

# nullable enable

namespace PetPlayApp.Server.Db.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User? GetUserByName(string userName)
        {
            return _userRepo.GetAll().Where(u => u.UserName == userName).FirstOrDefault();
        }

        public User? GetUserById(int id)
        {
            return _userRepo.GetById(id);
        }

        public void AddUser(User user)
        {
            if (ValidateUser(user))
            {
                _userRepo.Add(user);
            }
        }

        public bool ValidateUser(User user)
        {
            // Bryan can add log in stuff here
            return true;
        }
    }
}
