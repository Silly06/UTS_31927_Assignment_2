namespace PetPlayApp.Server.Dto;

public class StoryDetailsDto
{
    public Guid? StoryId { get; set; }
    
    public Guid? StoryCreatorId { get; set; }
    
    public byte[]? ImageData { get; set; }
}