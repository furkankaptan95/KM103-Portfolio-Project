using Microsoft.AspNetCore.Http;

namespace App.ViewModels.PortfolioMvc.UserViewModels;
public class EditUserViewModel
{
    public string Email { get; set; }
    public string? Username { get; set; }
    public IFormFile? ImageFile { get; set; }
}
