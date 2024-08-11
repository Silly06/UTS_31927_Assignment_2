using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;


#nullable enable

namespace PetPlayApp.Server.Db.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User? GetUser(string userName)
        {
            return _userRepo.GetAll().Where(u => u.UserName == userName).FirstOrDefault();
        }

        public User? GetUser(int id)
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

        public void RemoveUser(string name)
        {
            var userToRemove = GetUser(name);
            if (userToRemove != null)
            {
                _userRepo.Remove(userToRemove);
            }
        }

        public void RemoveUser(int id)
        {
            var userToRemove = _userRepo.GetById(id);
            _userRepo.Remove(userToRemove);
        }

        public void SeedUsers()
        {
            _userRepo.RemoveAll();
            // plant the seeds
        }
    }
}
