using App.DTOs.FileApiDtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.FileAPI.Controllers;

[EnableCors("AllowMvcClient")]
[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly string _uploadsFolder;

    public FileController()
    {
        try
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }
        catch (Exception ex)
        {
            // Hata kaydı yapabilir veya uygun bir hata mesajı dönebilirsin
            Console.WriteLine($"Dizin oluşturulurken hata oluştu: {ex.Message}");
            // Hata yönetimi için başka bir çözüm düşünebilirsin.
        }
    }

    [HttpPost("/upload-files")]
    public async Task<IActionResult> UploadFilesAsync([FromBody] IFormFile imageFile1, IFormFile imageFile2)
    {
     
        // İlk dosyayı kaydet
        var fileName1 = Path.GetFileName(imageFile1.FileName);
        var filePath1 = Path.Combine(_uploadsFolder, fileName1);
        using (var stream = new FileStream(filePath1, FileMode.Create))
        {
            await imageFile1.CopyToAsync(stream);
        }

        // İkinci dosyayı kaydet
        var fileName2 = Path.GetFileName(imageFile2.FileName);
        var filePath2 = Path.Combine(_uploadsFolder, fileName2);
        using (var stream = new FileStream(filePath2, FileMode.Create))
        {
            await imageFile2.CopyToAsync(stream);
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
