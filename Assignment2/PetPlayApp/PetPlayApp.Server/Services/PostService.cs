using PetPlayApp.Server.Db;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;
using System.ComponentModel.Design;

namespace PetPlayApp.Server.Services
{
    public class PostService(IRepositoryProviderService repositoryProvider) : IPostService
    {
		private readonly IRepository<Post> postRepository;
		private readonly IRepository<User> userRepository;

		public void LikePost(Guid postId, Guid userId)
		{
			postRepository = repositoryProvider.GetRepository<Post>();
			userRepository = repositoryProvider.GetRepository<User>();
		}

		public void LikePost(Guid postId, Guid userId)
        {
			var post = postRepository.GetById(postId) ?? throw new ArgumentException("Post not found");
			var user = userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
			post.Likes.Add(user);
			postRepository.Update(post);
            CheckForMatch(post, user);
        }

		public void UnlikePost(Guid postId, Guid userId)
		{
			var post = postRepository.GetById(postId) ?? throw new ArgumentException("Post not found");
			var user = userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
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
