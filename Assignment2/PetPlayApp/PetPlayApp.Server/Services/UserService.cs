using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services;

public class UserService(IRepositoryProviderService repositoryProvider) : IUserService
{
    private readonly IRepository<User> _userRepository = repositoryProvider.GetRepository<User>();

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

    public User? GetUser(Guid id)
    {
        return _userRepository.GetById(id);
    }

    public UserDetailsDto GetUserDetails(Guid userId)
    {
        var user = _userRepository.GetById(userId)!;

        return new UserDetailsDto
        {
            Username = user.UserName,
            Email = user.Email,
            Age = user.Age,
            Bio = user.Bio,
            ProfilePicture = user.ProfilePictureData,
            Status = user.UserStatus,
            Interest = user.Interest
        };
    }

    public void UpdateUserDetails(Guid? id, string? username, string? email, int? age, string? bio, UserStatus? status, UserInterest? interest, byte[]? image)
    {
        var user = _userRepository.GetById(id ?? Guid.Empty);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.UserName = username;
        user.Email = email;
        user.Age = age;
        user.Bio = bio;
        user.UserStatus = status;
        user.Interest = interest;
        if (image != null)
        {
            user.ProfilePictureData = image;
        }

        _userRepository.Update(user);
    }

    public void CreateUser(string? username, string? password, string? email, int? age, string? bio, byte[]? imagedata)
    {
        var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.UserName == username || u.Email == email);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }
        imagedata ??= File.ReadAllBytes("Assets/SeededProfilePictures/DefaultProfile.png");

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Password = password,
            Email = email,
            Age = age,
            Bio = bio,
            ProfilePictureData = imagedata
        };
        
        _userRepository.Add(user);
    }
    
    public byte[]? GetUserPicture(Guid id)
    {
        var user = _userRepository.GetById(id);
        if (user == null) return null;
        var picture = user.ProfilePictureData ?? File.ReadAllBytes(@"Assets/SeededProfilePictures/DefaultProfile.png");
        return picture;
    }
    
    public IEnumerable<UserSearchDto> SearchUsers(Guid currentUserId, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return _userRepository.GetAll()
                .Where(x => x.Id != currentUserId)
                .Select(user => new UserSearchDto
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    ProfilePicture = user.ProfilePictureData
                });
        }

        var lowerCaseQuery = query.Trim().ToLower();
        var users = _userRepository.GetAll().Where(x => x.Id != currentUserId);

        var userSearchResults = users
            .Select(user => new
            {
                User = user,
                Similarity = CalculateUserMatchScore(lowerCaseQuery, user.UserName?.ToLower(), user.Bio?.ToLower(), user.Interest)
            })
            .OrderByDescending(result => result.Similarity);

        return userSearchResults
            .Take(10)
            .Select(result => new UserSearchDto
            {
                UserId = result.User.Id,
                Username = result.User.UserName,
                ProfilePicture = result.User.ProfilePictureData
            });
    }

    public void ResetPassword(string email, string oldPassword, string newPassword)
    {
        var user = _userRepository.GetAll().FirstOrDefault(u => u.Email == email && u.Password == oldPassword);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        user.Password = newPassword;
        _userRepository.Update(user);
    }

    private static double CalculateUserMatchScore(string query, string? username, string? bio, UserInterest? interest)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return 0;
        }

        var querySet = new HashSet<string>(query.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        var bioSet = bio?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

        var usernameMatch = username != null && username.Contains(query, StringComparison.OrdinalIgnoreCase) ? 0.5 : 0;
        var bioScore = querySet.Count(term => bioSet.Contains(term)) / (double)(bioSet.Length + 1);
        var interestScore = interest.HasValue && query.Contains(interest.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase) ? 0.2 : 0;

        return usernameMatch + bioScore + interestScore;
    }
}