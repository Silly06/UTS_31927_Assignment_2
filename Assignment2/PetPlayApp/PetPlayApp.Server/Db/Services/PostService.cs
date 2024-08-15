using Microsoft.AspNetCore.Mvc;
using PetPlayApp.Server.Controllers;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Services
{
    public class PostService
    {
		private readonly Repository<Post> postRepository;

		public PostService(ILogger<PostsController> logger, RepositoryProvider repositoryProvider)
		{
			postRepository = repositoryProvider.GetRepository<Post>();
		}

		public void LikePost(Post post, User user)
        {
            post.Likes.Add(user);
			postRepository.Update(post);
            CheckForMatch(post, user);
        }

		public void UnlikePost(Post post, User user)
		{
			post.Likes.Remove(user);
			postRepository.Update(post);
		}

		public List<Guid> GetRecentPosts(int page)
		{
			var postsIds = postRepository.GetAll()
				.OrderBy(x => x.DateTimePosted)
				.Take(page * 10)
				.TakeLast(10)
				.OrderBy(x => x.DateTimePosted)
				.Select(x => x.Id)
				.ToList();

			return postsIds;
		}

		public List<Guid> GetUserPosts(int page, Guid userid)
		{
			var postsIds = postRepository.GetAll()
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
			var post = postRepository.GetById(postid);
			return post;
		}

		public void AddPost(Post post)
		{
			postRepository.Add(post);
		}

		public void CheckForMatch(Post post, User user)
        {
            // if it is matchy match time then make a matchy match
            // also this should be in match service but dw about it
        }
    }
}
