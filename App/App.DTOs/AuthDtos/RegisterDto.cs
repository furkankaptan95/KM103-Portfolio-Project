namespace App.DTOs.AuthDtos;
public class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }

    public RegisterDto()
    {
        
    }
    public RegisterDto(string username, string email, string password,string url)
    {
        Username = username;
        Email = email;
        Password = password;
        Url = url;
    }
}
