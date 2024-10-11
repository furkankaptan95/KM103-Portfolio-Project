using App.DTOs.FileApiDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.FileAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly string _uploadsFolder;

    public FileController()
    {
        // Uploads dizininin yolunu belirle
        _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        // Eğer uploads dizini yoksa oluştur
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }
    }

    [HttpPost("upload-files")]
    public async Task<IActionResult> UploadFilesAsync([FromBody] UploadFileDto dto)
    {
     
        // İlk dosyayı kaydet
        var fileName1 = Path.GetFileName(dto.ImageFile1.FileName);
        var filePath1 = Path.Combine(_uploadsFolder, fileName1);
        using (var stream = new FileStream(filePath1, FileMode.Create))
        {
            await dto.ImageFile1.CopyToAsync(stream);
        }

        // İkinci dosyayı kaydet
        var fileName2 = Path.GetFileName(dto.ImageFile2.FileName);
        var filePath2 = Path.Combine(_uploadsFolder, fileName2);
        using (var stream = new FileStream(filePath2, FileMode.Create))
        {
            await dto.ImageFile2.CopyToAsync(stream);
        }

        var urlDto = new ReturnUrlDto
        {
            ImageUrl1 = fileName1,
            ImageUrl2 = fileName2,
        };

        // Başarılı yanıt döndür
        return Ok(urlDto);
    }
}
