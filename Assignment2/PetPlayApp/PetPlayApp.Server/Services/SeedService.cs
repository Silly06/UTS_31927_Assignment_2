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
		IMatchService MatchService { get; }
		IUserService UserService { get; }

		public SeedService(IRepositoryProviderService repositoryProvider, IMatchService matchService, IUserService userService)
		{
			UserRepository = repositoryProvider.GetRepository<User>();
			MatchRepository = repositoryProvider.GetRepository<Match>();
			PostRepository = repositoryProvider.GetRepository<Post>();
			MatchService = matchService;
			UserService = userService;
		}

		public void SeedData()
		{
			UserRepository.RemoveAll();
			MatchRepository.RemoveAll();
			PostRepository.RemoveAll();
			SeedUsers();
			SeedMatches();
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
				// ProfilePictureData = File.ReadAllBytes("..\\petplayapp.client\\src\\assets\\SeededProfilePictures\\Turtle.png")
			});

			UserRepository.Add(new User
			{
				UserName = "BaldwinTheBunny",
				Password = "BaldwinPassword",
				Email = "Baldwin@gmail.com",
				Age = 19,
				Bio = "Like carrots",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Birds
			});

			UserRepository.Add(new User
			{
				UserName = "AidanTheAlpaca",
				Password = "AidanPassword",
				Email = "Aidan@gmail.com",
				Age = 18,
				Bio = "Like apples and climbing",
				UserStatus = UserStatus.Matched,
				Interest = UserInterest.Mammals
			});

			UserRepository.Add(new User
			{
				UserName = "Garfield",
				Password = "GarfieldPassword",
				Email = "Garfield@gmail.com",
				Age = 22,
				Bio = "Eats lasagna",
				UserStatus = UserStatus.Matched,
				Interest = UserInterest.Reptiles
			});

			UserRepository.Add(new User
			{
				UserName = "DannyTheDog",
				Password = "DannyPassword",
				Email = "Danny@gmail.com",
				Age = 19,
				Bio = "Like dog things",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Amphibians
			});

			UserRepository.Add(new User
			{
				UserName = "HanselTheHorse",
				Password = "HanselPassword",
				Email = "Hansel@gmail.com",
				Age = 18,
				Bio = "Like hay",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Unlisted
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
	}
}
