using Microsoft.AspNetCore.Http;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class UpdateAboutMeViewModel
{
    public string Introduction { get; set; }
    public string FullName { get; set; }
    public string Field { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }
    public IFormFile? ImageFile1 { get; set; }
    public IFormFile? ImageFile2 { get; set; }
}
