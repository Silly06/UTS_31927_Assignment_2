using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Services.Abstractions
{
    public interface IUserService
    {
		public bool TryValidateUser(string username, string password, out Guid userId);
		public IEnumerable<User> GetAllUsers();

		UserDetailsDto GetUserDetails(Guid userId);
		
		void UpdateUserDetails(Guid id, UserDetailsDto userDetails);
    }
}
