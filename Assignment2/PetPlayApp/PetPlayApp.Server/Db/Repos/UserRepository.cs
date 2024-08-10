namespace PetPlayApp.Server.Db.Repos
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
