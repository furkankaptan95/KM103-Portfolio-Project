using Microsoft.AspNetCore.Http;

namespace App.DTOs.AboutMeDtos;
public class UpdateAboutMeMVCDto
{
    public string FullName { get; set; }
    public string Field { get; set; }
    public string Introduction { get; set; }
    public IFormFile? ImageFile1 { get; set; }
    public IFormFile? ImageFile2 { get; set; }
}
