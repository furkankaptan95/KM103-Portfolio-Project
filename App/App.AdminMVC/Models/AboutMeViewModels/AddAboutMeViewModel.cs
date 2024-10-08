namespace App.AdminMVC.Models.AboutMeViewModels;
public class AddAboutMeViewModel
{
    public string Introduction { get; set; } = string.Empty;
    public IFormFile Image1 { get; set; }
    public IFormFile Image2 { get; set; }

}
