using PetPlayApp.Server.Models;

#nullable enable

namespace PetPlayApp.Server.Extensions
{
    public static class UserExtensions
    {
        public static bool IsMatched(this User? user)
        {
            if (user == null) return false;
            if (user.UserStatus == null) return false;
            return (UserStatus)user.UserStatus == UserStatus.Matched;
        }
    }
}