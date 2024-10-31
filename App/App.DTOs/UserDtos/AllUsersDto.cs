namespace App.DTOs.UserDtos;
public class AllUsersDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public List<UsersCommentsDto> Comments { get; set; } = new();
}
