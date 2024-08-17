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
		IRepository<Comment> CommentRepository { get; }
		IMatchService MatchService { get; }
		IUserService UserService { get; }

		public SeedService(IRepositoryProviderService repositoryProvider, IMatchService matchService, IUserService userService)
		{
			UserRepository = repositoryProvider.GetRepository<User>();
			MatchRepository = repositoryProvider.GetRepository<Match>();
			PostRepository = repositoryProvider.GetRepository<Post>();
			StoryRepository = repositoryProvider.GetRepository<Story>();
			CommentRepository = repositoryProvider.GetRepository<Comment>();
			MatchService = matchService;
			UserService = userService;
		}

		public void SeedData()
		{
			UserRepository.RemoveAll();
			MatchRepository.RemoveAll();
			PostRepository.RemoveAll();
			StoryRepository.RemoveAll();
			CommentRepository.RemoveAll();
			SeedUsers();
			SeedMatches();
			SeedStories();
			SeedPosts();
		}

		private void SeedUsers()
		{
			UserRepository.Add(new User
			{
				UserName = "ToddTheTurtle",
				Password = "ToddPassword",
				Email = "Todd@gmail.com",
				Age = 18,
				Bio = "Like long walks on the leash",
				UserStatus = UserStatus.NotMatched,
				Interest = UserInterest.Mammals,
				ProfilePictureData = File.ReadAllBytes(@"Assets/SeededProfilePictures/Turtle.png")
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
                ProfilePictureData = File.ReadAllBytes(@"Assets/SeededProfilePictures/Bunny.png")
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
                ProfilePictureData = File.ReadAllBytes(@"Assets/SeededProfilePictures/Alpaca.png")
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
                ProfilePictureData = File.ReadAllBytes(@"Assets/SeededProfilePictures/Garfield.png")
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
                ProfilePictureData = File.ReadAllBytes(@"Assets/SeededProfilePictures/Dog.png")
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
                ProfilePictureData = File.ReadAllBytes(@"Assets/SeededProfilePictures/Horse.png")
            });
		}

		private void SeedMatches()
		{
			MatchRepository.RemoveAll();

			var users = UserService.GetAllUsers().ToList();

			MatchService.AddMatch(users[0], users[1]); // Pending match between Todd and Baldwin
			MatchService.AddMatch(users[2], users[3], UserResponse.Accepted, UserResponse.Accepted, MatchStatus.Success); //Accepted match between Aidan and Garfield
			MatchService.AddMatch(users[4], users[0], UserResponse.Accepted, UserResponse.Rejected, MatchStatus.Failure); // Failed match between Danny and Todd
		}

		private void SeedStories()
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
		
		public void SeedPosts()
        {
            var users = UserService.GetAllUsers().ToList();

            var post1 = new Post
            {
                DateTimePosted = DateTime.UtcNow.AddDays(-2),
                PostCreatorId = users[0].Id,
                Description = "Just had a great walk in the park!",
                // ImageData = File.ReadAllBytes(@"../petplayapp.client/src/assets/SeededPostImages/Walk.png")
            };
            PostRepository.Add(post1);

            CommentRepository.Add(new Comment
            {
                Content = "Looks fun!",
                PostId = post1.Id,
                UserId = users[1].Id
            });

            CommentRepository.Add(new Comment
            {
                Content = "Wish I could join!",
                PostId = post1.Id,
                UserId = users[2].Id
            });

            var post2 = new Post
            {
                DateTimePosted = DateTime.UtcNow.AddDays(-1),
                PostCreatorId = users[1].Id,
                Description = "Eating some fresh carrots, life is good!",
                // ImageData = File.ReadAllBytes(@"../petplayapp.client/src/assets/SeededPostImages/Carrots.png")
            };
            PostRepository.Add(post2);

            CommentRepository.Add(new Comment
            {
                Content = "Yum, those look delicious!",
                PostId = post2.Id,
                UserId = users[3].Id
            });

            var post3 = new Post
            {
                DateTimePosted = DateTime.UtcNow,
                PostCreatorId = users[2].Id,
                Description = "Climbing mountains is the best!",
                // ImageData = File.ReadAllBytes(@"../petplayapp.client/src/assets/SeededPostImages/Mountains.png")
            };
            PostRepository.Add(post3);

            CommentRepository.Add(new Comment
            {
                Content = "Great view!",
                PostId = post3.Id,
                UserId = users[4].Id
            });

            CommentRepository.Add(new Comment
            {
                Content = "Stay safe out there!",
                PostId = post3.Id,
                UserId = users[0].Id
            });
        }
	}
}
