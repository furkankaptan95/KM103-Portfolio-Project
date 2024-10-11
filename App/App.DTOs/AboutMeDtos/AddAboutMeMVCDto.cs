using Microsoft.AspNetCore.Http;

namespace App.DTOs.AboutMeDtos;
public class AddAboutMeMVCDto
{
    public string Introduction { get; set; }
    public IFormFile? ImageFile1 { get; set; }
    public IFormFile? ImageFile2 { get; set; }
}
