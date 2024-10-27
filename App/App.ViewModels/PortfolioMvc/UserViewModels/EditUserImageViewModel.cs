using Microsoft.AspNetCore.Http;

namespace App.ViewModels.PortfolioMvc.UserViewModels;
public class EditUserImageViewModel
{
    public string Email { get; set; }
    public IFormFile ImageFile { get; set; }
}
