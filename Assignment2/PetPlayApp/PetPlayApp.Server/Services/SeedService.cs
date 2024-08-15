using PetPlayApp.Server.Db;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services
{
	public class SeedService
	{
		Repository<User> UserRepository { get; }
		Repository<Match> MatchRepository { get; }
		Repository<Post> PostRepository { get; }
		MatchService MatchService { get; }
		UserService UserService { get; }

		public SeedService(RepositoryProvider repositoryProvider, MatchService matchService, UserService userService)
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
				Interest = UserInterest.Mammals
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
