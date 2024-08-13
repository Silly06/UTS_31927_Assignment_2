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

        public User? GetUser(Guid id)
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

        public void RemoveUser(Guid id)
        {
            var userToRemove = _userRepo.GetById(id);
            _userRepo.Remove(userToRemove);
        }

        public void SeedUsers()
        {
            // Remove existing users
            _userRepo.RemoveAll();

            // Add new users
            _userRepo.Add(new User("ToddTheTurtle", "ToddPassword", "Todd@gmail.com", 18, "Likes long walks on the leash", (int)UserStatus.NotMatched, (int)UserInterest.Mammals));
            _userRepo.Add(new User("BaldwinTheBunny", "BaldwinPassword", "Baldwin@gmail.com", 19, "Likes carrots", (int)UserStatus.NotMatched, (int)UserInterest.Birds));
            _userRepo.Add(new User("AidanTheAlpaca", "AidanPassword", "Aidan@gmail.com", 18, "Likes apples and climbing", (int)UserStatus.Matched, (int)UserInterest.Mammals));
            _userRepo.Add(new User("Garfield", "GarfieldPassword", "Garfield@gmail.com", 22, "Eats lasagna", (int)UserStatus.Matched, (int)UserInterest.Reptiles));
            _userRepo.Add(new User("DannyTheDog", "DannyPassword", "Danny@gmail.com", 19, "Likes dog things", (int)UserStatus.NotMatched, (int)UserInterest.Amphibians));
            _userRepo.Add(new User("HanselTheHorse", "HanselPassword", "Hansel@gmail.com", 18, "Likes hay", (int)UserStatus.NotMatched, (int)UserInterest.Unlisted));
            _userRepo.SaveChanges();
        }
    }
}
