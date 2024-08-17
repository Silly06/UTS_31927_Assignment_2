using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PetPlayApp.Server.Models
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }

		public string? UserName { get; set; }
		public string? Password { get; set; }
		public string? Email { get; set; }
		public int? Age { get; set; }
		public string? Bio { get; set; }
		public UserStatus? UserStatus { get; set; }
		public UserInterest? Interest { get; set; }

		public List<Match>? MatchesInitiated { get; set; }
		public List<Match>? MatchesReceived { get; set; }

		public List<Post> LikedPosts { get; } = [];

		[InverseProperty("PostCreator")]
		public List<Post> CreatedPosts { get; } = [];

        public byte[]? ProfilePictureData { get; set; }
    }

	public enum UserStatus
	{
		Unlisted,
		Matched,
		NotMatched
	}

	public enum UserInterest
	{
		Unlisted,
		Mammals,
		Reptiles,
		Amphibians,
		Birds,
	}
}