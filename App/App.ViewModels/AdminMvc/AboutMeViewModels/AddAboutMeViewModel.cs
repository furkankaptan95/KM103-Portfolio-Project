using Microsoft.AspNetCore.Http;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class AddAboutMeViewModel
{
    public string Introduction { get; set; }
    public string FullName { get; set; }
    public string Field { get; set; }
    public IFormFile Image1 { get; set; }
    public IFormFile Image2 { get; set; }

}
