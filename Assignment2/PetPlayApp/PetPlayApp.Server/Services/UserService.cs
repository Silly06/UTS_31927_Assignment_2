using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepositoryProviderService repositoryProvider)
        {
            _userRepository = repositoryProvider.GetRepository<User>();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public bool TryValidateUser(string username, string password, out Guid userId)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);
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
            var userToRemove = _userRepository.GetById(id);
            if (userToRemove != null)
            {
                _userRepository.Remove(userToRemove);
            }
        }

        public UserDetailsDto GetUserDetails(Guid userId)
        {
            var user = _userRepository.GetById(userId)!;

            return new UserDetailsDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Age = user.Age,
                Bio = user.Bio
            };
        }

        public void UpdateUserDetails(Guid id, UserDetailsDto userDetails)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("User ID is invalid.");
            }

            if (userDetails == null)
            {
                throw new ArgumentNullException(nameof(userDetails), "UserDetails object is null.");
            }

            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.UserName = userDetails.UserName ?? user.UserName;
            user.Email = userDetails.Email ?? user.Email;
            user.Age = userDetails.Age;
            user.Bio = userDetails.Bio ?? user.Bio;

            _userRepository.Update(user);
        }

    }
}