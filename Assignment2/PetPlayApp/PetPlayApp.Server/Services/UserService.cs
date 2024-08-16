using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;


namespace PetPlayApp.Server.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> userRepository;

		public UserService(IRepositoryProviderService repositoryProvider)
		{
			userRepository = repositoryProvider.GetRepository<User>();
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
	}
}
