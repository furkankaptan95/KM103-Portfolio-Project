namespace App.ViewModels.AuthViewModels;
public class ForgotPasswordViewModel
{
    public string Email { get; set; }
    public ForgotPasswordViewModel()
    {
        
    }

    public ForgotPasswordViewModel(string email)
    {
        Email = email;
    }
}
