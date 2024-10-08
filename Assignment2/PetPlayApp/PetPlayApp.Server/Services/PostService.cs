﻿using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

#nullable enable

namespace PetPlayApp.Server.Services
{
	public class PostService(IRepositoryProviderService repositoryProvider, INotificationService notificationService) : IPostService
	{
		private readonly IRepository<Post> _postRepository = repositoryProvider.GetRepository<Post>();
		private readonly IRepository<User> _userRepository = repositoryProvider.GetRepository<User>();
		private readonly INotificationService _notificationService = notificationService;

		public PostDetailsDto? LikePost(Guid postId, Guid userId)
		{
			var post = _postRepository.GetById(postId);
			var user = _userRepository.GetById(userId);

			if (post == null || user == null) return null;
			if (post.Likes.Contains(user)) return new PostDetailsDto
			{
				PostId = postId,
				LikesCount = post.Likes.Count,
				LikedByUser = true
			};

			post.Likes.Add(user);
			_postRepository.Update(post);

			_notificationService.NotifyPostLiked(postId, post.PostCreatorId, userId);

			return new PostDetailsDto
			{
				PostId = postId,
				LikesCount = post.Likes.Count,
				LikedByUser = true
			};
		}

		public PostDetailsDto? UnlikePost(Guid postId, Guid userId)
		{
			var post = _postRepository.GetById(postId);
			var user = _userRepository.GetById(userId);

			if (post == null || user == null) return null;
			if (!post.Likes.Contains(user)) return new PostDetailsDto
			{
				PostId = postId,
				LikesCount = post.Likes.Count,
				LikedByUser = false
			};

			post.Likes.Remove(user);
			_postRepository.Update(post);

			return new PostDetailsDto
			{
				PostId = postId,
				LikesCount = post.Likes.Count,
				LikedByUser = false
			};
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
