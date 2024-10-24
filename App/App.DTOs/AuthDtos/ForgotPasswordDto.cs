namespace App.DTOs.AuthDtos;
public class ForgotPasswordDto
{
    public string Email { get; set; }
    public string Url { get; set; }
    public ForgotPasswordDto(string email, string url)
    {
        Email = email;
        Url = url;
    }
}
