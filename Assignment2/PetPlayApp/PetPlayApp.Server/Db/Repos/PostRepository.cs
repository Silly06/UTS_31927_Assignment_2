using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Repos
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(DatabaseContext context) : base(context)
        {
			
		}
		
		public void LikePost(Post post, User user)
			{
				if(GetById(post.Id) != null)
				GetById(post.Id).Likes.Add(user);
			}
    }
}
