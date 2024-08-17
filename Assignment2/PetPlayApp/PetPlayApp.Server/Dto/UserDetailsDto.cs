namespace PetPlayApp.Server.Dto;

public class UserDetailsDto
{
    public Guid? UserId { get; set; }
    
    public string? Username { get; init; }
    
    public string? Email { get; init; }
    
    public int? Age { get; init; }
    
    public string? Bio { get; init; }
}