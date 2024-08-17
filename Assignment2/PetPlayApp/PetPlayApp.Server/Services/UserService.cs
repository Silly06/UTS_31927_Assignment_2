using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services;

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
            Username = user.UserName,
            Email = user.Email,
            Age = user.Age,
            Bio = user.Bio,
        };
    }

    public void UpdateUserDetails(Guid? id, string? username, string? email, int? age, string? bio)
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

        _userRepository.Update(user);
    }

    public void CreateUser(string? username, string? password, string? email, int? age, string? bio)
    {
        var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.UserName == username || u.Email == email);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Password = password,
            Email = email,
            Age = age,
            Bio = bio
        };
        
        _userRepository.Add(user);
    }
}