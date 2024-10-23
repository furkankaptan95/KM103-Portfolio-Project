namespace App.ViewModels.AuthViewModels;
public class RegisterViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public RegisterViewModel(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}
