// File: PetPlayApp/PetPlayApp.Server/Db/Services/LoginService.cs
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Services
{
    public class LoginService
    {
        private readonly Repository<User> userRepository;

        public LoginService(RepositoryProvider repositoryProvider)
        {
            userRepository = repositoryProvider.GetRepository<User>();
        }

        public int? ValidateUser(string username, string password)
        {
            var user = userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            return user?.Id;
        }
    }
}
