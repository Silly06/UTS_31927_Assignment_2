using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Extensions
{
    public static class UserExtensions
    {
        public static bool IsMatched(this User user)
        {
            return (UserStatus)user.UserStatus == UserStatus.Matched;
        }
    }
}