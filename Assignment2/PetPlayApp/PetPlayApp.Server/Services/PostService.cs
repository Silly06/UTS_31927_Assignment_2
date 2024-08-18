using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services
{
    public class PostService(IRepositoryProviderService repositoryProvider) : IPostService
    {
		private readonly IRepository<Post> _postRepository = repositoryProvider.GetRepository<Post>();
		private readonly IRepository<User> _userRepository = repositoryProvider.GetRepository<User>();

		public void LikePost(Guid postId, Guid userId)
		{
			var post = _postRepository.GetById(postId);
			var user = _userRepository.GetById(userId);

			if (post == null || user == null) return;
			if (post.Likes.Contains(user)) return;
			
			post.Likes.Add(user);
			_postRepository.Update(post);
			CheckForMatch(post, user);
		}


		public void UnlikePost(Guid postId, Guid userId)
		{
			var post = _postRepository.GetById(postId);
			var user = _userRepository.GetById(userId);

			if (post == null || user == null) return;
			if (!post.Likes.Contains(user)) return;
			
			post.Likes.Remove(user);
			_postRepository.Update(post);
		}


		public List<Guid> GetRecentPosts(int page)
		{
			var postsIds = _postRepository.GetAll()
				.OrderByDescending(x => x.DateTimePosted)
				.Take(page * 10)
				.TakeLast(10)
				.OrderByDescending(x => x.DateTimePosted)
				.Select(x => x.Id)
				.ToList();

			return postsIds;
		}

		public List<Guid> GetUserPosts(int page, Guid userid)
		{
			var postsIds = _postRepository.GetAll()
				.Where(x => x.PostCreatorId == userid)
				.OrderBy(x => x.DateTimePosted)
				.Take(page * 10)
				.TakeLast(10)
				.OrderBy(x => x.DateTimePosted)
				.Select(x => x.Id)
				.ToList();

			return postsIds;
		}

		public Post? GetPost(Guid postid)
		{
			var post = _postRepository.GetById(postid);
			return post;
		}

		public void AddPost(Post post)
		{
			_postRepository.Add(post);
		}

		public void CheckForMatch(Post post, User user)
        {
            // if it is matchy match time then make a matchy match
            // also this should be in match service but dw about it
        }
    }
}
