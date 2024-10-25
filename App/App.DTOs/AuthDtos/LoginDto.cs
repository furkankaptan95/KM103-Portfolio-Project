namespace App.DTOs.AuthDtos;
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

    public LoginDto()
    {
        
    }
    public LoginDto(string email, string password,bool isAdmin)
    {
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }
}
