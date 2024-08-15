using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;


#nullable enable

namespace PetPlayApp.Server.Db.Services
{
    public class UserService
    {
        private readonly Repository<User> userRepository;

        public UserService(RepositoryProvider repositoryProvider)
        {
            this.userRepository = repositoryProvider.GetRepository<User>();
        }

        public User? GetUser(string userName)
        {
            return userRepository.GetAll().Where(u => u.UserName == userName).FirstOrDefault();
        }

        public User? GetUser(Guid id)
        {
            return userRepository.GetById(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return userRepository.GetAll();
        }

		public bool TryValidateUser(string username, string password, out Guid userId)
		{
			var user = userRepository.GetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);
			if (user != null)
			{
				userId = user.Id;
				return true;
			}
			userId = Guid.Empty;
			return false;
		}

        public void RemoveUser(Guid id)
        {
            var userToRemove = userRepository.GetById(id);
			if (userToRemove != null)
			{
				userRepository.Remove(userToRemove);
			}
        }

        public void UpdateUserData()
        {

        }

        public void SeedUsers()
        {
			// Remove existing users
			userRepository.RemoveAll();

			// Add new users
			userRepository.Add(new User
			{
				UserName = "ToddTheTurtle",
				Password = "ToddPassword",
				Email = "Todd@gmail.com",
				Age = 18,
				Bio = "Like long walks on the leash",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Mammals
			});

			userRepository.Add(new User
			{
				UserName = "BaldwinTheBunny",
				Password = "BaldwinPassword",
				Email = "Baldwin@gmail.com",
				Age = 19,
				Bio = "Like carrots",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Birds
			});

			userRepository.Add(new User
			{
				UserName = "AidanTheAlpaca",
				Password = "AidanPassword",
				Email = "Aidan@gmail.com",
				Age = 18,
				Bio = "Like apples and climbing",
				UserStatus = (int)UserStatus.Matched,
				Interest = (int)UserInterest.Mammals
			});

			userRepository.Add(new User
			{
				UserName = "Garfield",
				Password = "GarfieldPassword",
				Email = "Garfield@gmail.com",
				Age = 22,
				Bio = "Eats lasagna",
				UserStatus = (int)UserStatus.Matched,
				Interest = (int)UserInterest.Reptiles
			});

			userRepository.Add(new User
			{
				UserName = "DannyTheDog",
				Password = "DannyPassword",
				Email = "Danny@gmail.com",
				Age = 19,
				Bio = "Like dog things",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Amphibians
			});

			userRepository.Add(new User
			{
				UserName = "HanselTheHorse",
				Password = "HanselPassword",
				Email = "Hansel@gmail.com",
				Age = 18,
				Bio = "Like hay",
				UserStatus = (int)UserStatus.NotMatched,
				Interest = (int)UserInterest.Unlisted
			});
        }
    }
}
