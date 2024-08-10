using PetPlayApp.Server.Db.Repos;

namespace PetPlayApp.Server.Db.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }
    }
}
