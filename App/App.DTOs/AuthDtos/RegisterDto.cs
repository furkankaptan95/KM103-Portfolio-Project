namespace App.DTOs.AuthDtos;
public class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public RegisterDto(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}
