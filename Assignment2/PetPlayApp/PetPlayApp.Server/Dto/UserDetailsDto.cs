namespace PetPlayApp.Server.Dto;

public class UserDetailsDto
{
    public Guid? UserId { get; set; }
    
    public string? Username { get; set; }
    
    public string? Email { get; set; }
    
    public int? Age { get; set; }
    
    public string? Bio { get; set; }

    public byte[]? ProfilePicture { get; set; }
}