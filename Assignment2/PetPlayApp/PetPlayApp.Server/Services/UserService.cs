using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;


namespace PetPlayApp.Server.Services
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
    }
}
