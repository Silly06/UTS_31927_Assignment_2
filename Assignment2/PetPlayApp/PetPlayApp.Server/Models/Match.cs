using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetPlayApp.Server.Models
{
    [PrimaryKey("User1Id", "User2Id")]
	public class Match
	{
		[ForeignKey("User1Id")]
		public Guid User1Id { get; set; }
		public User? User1 { get; set; }

		[ForeignKey("User2Id")]
		public Guid User2Id { get; set; }
		public User? User2 { get; set; }

		public UserResponse User1Response { get; set; }
		public UserResponse User2Response { get; set; }
		public MatchStatus OverallStatus { get; set; }
	}

	public enum MatchStatus
	{
		Success,
		Failure,
		AwaitingResponse
	}

	public enum UserResponse
	{
		Accepted,
		Rejected,
		Pending
	}
}