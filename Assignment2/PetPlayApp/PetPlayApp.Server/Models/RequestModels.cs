using Newtonsoft.Json;

namespace PetPlayApp.Server.Models
{
	public class PostRequestModel
	{
		[JsonProperty("postCreatorId")]
		public Guid? PostCreatorId { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }
	}
}
