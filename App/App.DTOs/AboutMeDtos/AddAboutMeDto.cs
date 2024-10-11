using Microsoft.AspNetCore.Http;

namespace App.DTOs.AboutMeDtos;
public class AddAboutMeDto
{
    public string Introduction { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }
    public IFormFile? ImageFile1 { get; set; }
    public IFormFile? ImageFile2 { get; set; }
}
