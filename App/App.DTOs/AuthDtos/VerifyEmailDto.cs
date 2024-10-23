namespace App.DTOs.AuthDtos;
public class VerifyEmailDto
{
    public string Token { get; set; }
    public string Email { get; set; }

    public VerifyEmailDto()
    {
        
    }

    public VerifyEmailDto(string email,string token)
    {
        Email = email;
        Token = token;
    }
}
