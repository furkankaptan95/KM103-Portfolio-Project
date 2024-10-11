using Microsoft.AspNetCore.Http;

namespace App.DTOs.FileApiDtos;
public class UploadFileDto
{
    public IFormFile ImageFile1 { get; set; }
    public IFormFile? ImageFile2 { get; set; }
}
