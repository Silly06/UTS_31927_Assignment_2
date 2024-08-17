namespace PetPlayApp.Server.Models;

public class Story
{
    public Guid Id { get; set; }
    
    public DateTime DateTimePosted { get; set; }
    
    public User? StoryCreator { get; set; }

    public Guid? StoryCreatorId { get; set; }
    
    public byte[]? ImageData { get; set; }
}