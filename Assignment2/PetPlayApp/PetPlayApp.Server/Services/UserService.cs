﻿using System.Collections;
using System.Globalization;
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
    
    public IEnumerable<UserSearchDto> SearchUsers(Guid currentUserId, string? query)
    {
        var users = _userRepository.GetAll().Where(x => x.Id.ToString() != currentUserId.ToString());

        var userSearchResults = users
            .Select(user => new
            {
                User = user,
                Similarity = CalculateBioSimilarityZScore(query, user.Bio)
            })
            .OrderByDescending(result => result.Similarity);

        return string.IsNullOrWhiteSpace(query) 
            ? userSearchResults.Select(result => new UserSearchDto
            {
                UserId = result.User.Id,
                Username = result.User.UserName
            })
            : userSearchResults.Take(10).Select(result => new UserSearchDto
            {
                UserId = result.User.Id,
                Username = result.User.UserName
            });
    }

    private static double CalculateBioSimilarityZScore(string? query, string? bio)
    {
        if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(bio))
        {
            return 0;
        }

        var querySet = new HashSet<string>(query.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        var bioSet = new HashSet<string>(bio.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        var intersection = querySet.Intersect(bioSet).Count();
        var union = querySet.Union(bioSet).Count();

        return (double)intersection / union;
    }
}