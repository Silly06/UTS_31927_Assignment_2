using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Dto;
using PetPlayApp.Server.Models;
using PetPlayApp.Server.Services.Abstractions;

namespace PetPlayApp.Server.Services

{
	public class NotificationService : INotificationService
	{
		private readonly IRepository<Notification> notificationRepository;
		private readonly IRepository<Post> postRepository;
		private readonly IRepository<User> userRepository;


		public NotificationService(IRepositoryProviderService repositoryProviderService)
		{
			notificationRepository = repositoryProviderService.GetRepository<Notification>();
			postRepository = repositoryProviderService.GetRepository<Post>();
			userRepository = repositoryProviderService.GetRepository<User>();
		}



		public void NotifyCommentCreated(Guid postId, Guid creatorId, Guid subjectId)
		{
			var post = postRepository.GetById(postId);
			var creator = userRepository.GetById(creatorId);
			var subject = userRepository.GetById(subjectId);
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.Comment,
				PostId = postId,
				Post = post,
				Subject = subject,
				SubjectId = creatorId,
				CreatorId = creatorId,
				Creator = creator
			});
		}

		public void NotifyCommentLiked(Guid postId, Guid creatorId, Guid subjectId)
		{
			var post = postRepository.GetById(postId);
			var creator = userRepository.GetById(creatorId);
			var subject = userRepository.GetById(subjectId);
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.CommentLike,
				PostId = postId,
				Post = post,
				Subject = subject,
				SubjectId = creatorId,
				CreatorId = creatorId,
				Creator = creator
			});
		}

		public void NotifyPostLiked(Guid postId, Guid creatorId, Guid subjectId)
		{
			var post = postRepository.GetById(postId);
			var creator = userRepository.GetById(creatorId);
			var subject = userRepository.GetById(subjectId);
			notificationRepository.Add(new Notification
			{
				Id = Guid.NewGuid(),
				Timestamp = DateTime.Now,
				NotificationType = NotificationType.PostLike,
				PostId = postId,
				Post = post,
				Subject = subject,
				SubjectId = creatorId,
				CreatorId = creatorId,
				Creator = creator

			});
		}

		public List<NotificationDto> GetRecentNotifications(Guid userId)
		{
			return notificationRepository.GetAll()
				.Where(n => n.SubjectId == userId)
				.OrderBy(x => x.Timestamp)
				.Select(x => 
					new NotificationDto
					{
						PostId = x.PostId,
						Content = GetContent(x),
					})
				.ToList();
		}

		private string GetContent(Notification notification)
		{
			var user = userRepository.GetById(notification.CreatorId) ?? throw new ArgumentNullException("User not found");
			return notification.NotificationType switch
			{
				NotificationType.Comment => $"{user.UserName} commented on your post",
				NotificationType.CommentLike => $"{user.UserName} liked your comment",
				NotificationType.PostLike => $"{user.UserName} liked your post",
				_ => throw new ArgumentException("Invalid notification type"),
			};
		}
	}
}