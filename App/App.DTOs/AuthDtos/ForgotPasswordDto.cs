namespace App.DTOs.AuthDtos;
public class ForgotPasswordDto
{
    public string Email { get; set; }
    public string Url { get; set; }
    public bool IsAdmin { get; set; }

    public ForgotPasswordDto(string email, string url,bool isAdmin)
    {
        Email = email;
        Url = url;
        IsAdmin = isAdmin;
    }

}
