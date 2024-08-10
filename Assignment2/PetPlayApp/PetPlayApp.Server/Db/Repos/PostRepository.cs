using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Repos
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
