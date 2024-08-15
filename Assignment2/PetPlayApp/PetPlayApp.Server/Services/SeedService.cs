using PetPlayApp.Server.Db.Services;

namespace PetPlayApp.Server.Services
{
	public class SeedService
	{
		UserService UserService { get; }
		MatchService MatchService { get; }

		public SeedService(UserService userService, MatchService matchService) 
		{
			this.UserService = userService;
			this.MatchService= matchService;
		}

		public void SeedData()
		{
			UserService.SeedUsers();
			MatchService.SeedMatches();
		}
	}
}
