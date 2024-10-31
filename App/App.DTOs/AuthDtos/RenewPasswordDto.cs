namespace App.DTOs.AuthDtos;
public class RenewPasswordDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public bool IsAdmin { get; set; }

    public RenewPasswordDto()
    {
        
    }

    public RenewPasswordDto(string email,string token, bool isAdmin)
    {
        Email = email;
        Token = token;
        IsAdmin = isAdmin;
    }
}
