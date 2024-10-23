namespace App.DTOs.AuthDtos;
public class ForgotPasswordDto
{
    public string Email { get; set; }
    public ForgotPasswordDto(string email)
    {
        Email = email;
    }
}
