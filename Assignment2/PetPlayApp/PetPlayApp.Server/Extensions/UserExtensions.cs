using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Extensions
{
    public static class UserExtensions
    {
        public static bool IsMatched(this User user)
        {
            if (user.UserStatus == null) return false;
            return (UserStatus)user.UserStatus == UserStatus.Matched;
        }
    }
}