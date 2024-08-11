using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Repos
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public void RemoveAll()
        {
            var users = GetAll();
            foreach (var user in users)
            {
                _context.Users.Remove(user);
            }
            _context.SaveChanges();
        }
    }
}
