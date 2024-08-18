using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;

#nullable enable

namespace PetPlayApp.Server.Services.Abstractions
{
	public interface IUserService
	{
		public bool TryValidateUser(string username, string password, out Guid userId);
		public IEnumerable<User> GetAllUsers();

		UserDetailsDto GetUserDetails(Guid userId);

		void UpdateUserDetails(Guid? id, string? username, string? email, int? age, string? bio, UserStatus? status, UserInterest? interest, byte[]? imageData);

		void CreateUser(string? username, string? password, string? email, int? age, string? bio, byte[]? imageData);

		public byte[]? GetUserPicture(Guid id);

		IEnumerable<UserSearchDto> SearchUsers(Guid currentUserId, string query);

		public User? GetUser(Guid id);

        void ResetPassword(string email, string oldPassword, string newPassword);

		public Guid GetUserIdByName(string name);
    }
}
