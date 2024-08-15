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

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.GetAll();
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

        public void UpdateUserData()
        {

        }

        public void SeedUsers()
        {
            // Remove existing users
            _userRepo.RemoveAll();

            // Add new users
			_userRepo.Add(new User
			{
				UserName = "ToddTheTurtle",
				Password = "ToddPassword",
				Email = "Todd@gmail.com",
				Age = 18,
				Bio = "Like long walks on the leash",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Mammals
			});

			_userRepo.Add(new User
			{
				UserName = "BaldwinTheBunny",
				Password = "BaldwinPassword",
				Email = "Baldwin@gmail.com",
				Age = 19,
				Bio = "Like carrots",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Birds
			});

			_userRepo.Add(new User
			{
				UserName = "AidanTheAlpaca",
				Password = "AidanPassword",
				Email = "Aidan@gmail.com",
				Age = 18,
				Bio = "Like apples and climbing",
				UserStatus = (int)UserStatus.Matched,
				Interest = (int)UserInterest.Mammals
			});

			_userRepo.Add(new User
			{
				UserName = "Garfield",
				Password = "GarfieldPassword",
				Email = "Garfield@gmail.com",
				Age = 22,
				Bio = "Eats lasagna",
				UserStatus = (int)UserStatus.Matched,
				Interest = (int)UserInterest.Reptiles
			});

			_userRepo.Add(new User
			{
				UserName = "DannyTheDog",
				Password = "DannyPassword",
				Email = "Danny@gmail.com",
				Age = 19,
				Bio = "Like dog things",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Amphibians
			});

			_userRepo.Add(new User
			{
				UserName = "HanselTheHorse",
				Password = "HanselPassword",
				Email = "Hansel@gmail.com",
				Age = 18,
				Bio = "Like hay",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Unlisted
			});
            _userRepo.SaveChanges();
        }
    }
}
