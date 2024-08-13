﻿namespace PetPlayApp.Server.Models
{
    public class User
    {
        public Guid PK { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public string? Bio { get; set; }
        public int? UserStatus { get; set; }
        public int ?Interest {  get; set; }

        public User(string name, string pass, string email, int age, string bio = "", int status = 0, int interest = 0)
        {
            UserName = name;
            Password = pass;
            Email = email;
            Age = age;
            Bio = bio;
            UserStatus = status;
            Interest = interest;
        }
    }

    public enum UserStatus
    {
        Unlisted,
        Matched,
        NotMatched
    }

    public enum UserInterest
    {
        Unlisted,
        Mammals,
        Reptiles,
        Amphibians,
        Birds,
    }
}