using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;


namespace PetPlayApp.Server.Services
{
	public class SeedService : ISeedService
	{
		IRepository<User> UserRepository { get; }
		IRepository<Match> MatchRepository { get; }
		IRepository<Post> PostRepository { get; }
		IRepository<Story> StoryRepository { get; }
		IMatchService MatchService { get; }
		IUserService UserService { get; }

		public SeedService(IRepositoryProviderService repositoryProvider, IMatchService matchService, IUserService userService)
		{
			UserRepository = repositoryProvider.GetRepository<User>();
			MatchRepository = repositoryProvider.GetRepository<Match>();
			PostRepository = repositoryProvider.GetRepository<Post>();
			StoryRepository = repositoryProvider.GetRepository<Story>();
			MatchService = matchService;
			UserService = userService;
		}

		public void SeedData()
		{
			UserRepository.RemoveAll();
			MatchRepository.RemoveAll();
			PostRepository.RemoveAll();
			StoryRepository.RemoveAll();
			SeedUsers();
			SeedMatches();
			SeedStories();
		}

		public void SeedUsers()
		{
			// Add new users
			UserRepository.Add(new User
			{
				UserName = "ToddTheTurtle",
				Password = "ToddPassword",
				Email = "Todd@gmail.com",
				Age = 18,
				Bio = "Like long walks on the leash",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Mammals,
				ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Turtle.png")
			});

			UserRepository.Add(new User
			{
				UserName = "BaldwinTheBunny",
				Password = "BaldwinPassword",
				Email = "Baldwin@gmail.com",
				Age = 19,
				Bio = "Like carrots",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Birds,
                ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Bunny.png")
            });

			UserRepository.Add(new User
			{
				UserName = "AidanTheAlpaca",
				Password = "AidanPassword",
				Email = "Aidan@gmail.com",
				Age = 18,
				Bio = "Like apples and climbing",
				UserStatus = UserStatus.Matched,
				Interest = UserInterest.Mammals,
                ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Alpaca.png")
            });

			UserRepository.Add(new User
			{
				UserName = "Garfield",
				Password = "GarfieldPassword",
				Email = "Garfield@gmail.com",
				Age = 22,
				Bio = "Eats lasagna",
				UserStatus = UserStatus.Matched,
				Interest = UserInterest.Reptiles,
                ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Garfield.png")
            });

			UserRepository.Add(new User
			{
				UserName = "DannyTheDog",
				Password = "DannyPassword",
				Email = "Danny@gmail.com",
				Age = 19,
				Bio = "Like dog things",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Amphibians,
                ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Dog.png")
            });

			UserRepository.Add(new User
			{
				UserName = "HanselTheHorse",
				Password = "HanselPassword",
				Email = "Hansel@gmail.com",
				Age = 18,
				Bio = "Like hay",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Unlisted,
                ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Horse.png")
            });
		}

		public void SeedMatches()
		{
			MatchRepository.RemoveAll();

			List<User> users = UserService.GetAllUsers().ToList();

			MatchService.AddMatch(users[0], users[1]); // Pending match between Todd and Baldwin
			MatchService.AddMatch(users[2], users[3], UserResponse.Accepted, UserResponse.Accepted, MatchStatus.Success); //Accepted match between Aidan and Garfield
			MatchService.AddMatch(users[4], users[0], UserResponse.Accepted, UserResponse.Rejected, MatchStatus.Failure); // Failed match between Danny and Todd
		}

		public void SeedStories()
		{
			StoryRepository.Add(new Story
			{
				DateTimePosted = DateTime.UtcNow,
				StoryCreatorId = UserRepository.GetAll().FirstOrDefault()?.Id
			});

			StoryRepository.Add(new Story
			{
				DateTimePosted = DateTime.UtcNow,
				StoryCreatorId = UserRepository.GetAll().Skip(1).FirstOrDefault()?.Id
			});

			StoryRepository.Add(new Story
			{
				DateTimePosted = DateTime.UtcNow,
				StoryCreatorId = UserRepository.GetAll().Skip(2).FirstOrDefault()?.Id
			});
		}
	}
}
