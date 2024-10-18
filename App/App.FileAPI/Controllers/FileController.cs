using App.DTOs.FileApiDtos;
using Microsoft.AspNetCore.Cors;
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
            Console.WriteLine($"Dizin oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    [HttpPost("/upload-files")]
    public async Task<IActionResult> UploadFilesAsync([FromForm] IFormFile? imageFile1, IFormFile? imageFile2)
    {
        var urlDto = new ReturnUrlDto();

        try
        {
            if (imageFile1 is not null)
            {
                // İlk dosyayı kaydet
                var fileName1 = Path.GetFileName(imageFile1.FileName);
                var filePath1 = Path.Combine(_uploadsFolder, fileName1);
                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    await imageFile1.CopyToAsync(stream);
                }

                urlDto.ImageUrl1 = fileName1;
            }

            if (imageFile2 is not null)
            {
                // İkinci dosyayı kaydet
                var fileName2 = Path.GetFileName(imageFile2.FileName);
                var filePath2 = Path.Combine(_uploadsFolder, fileName2);
                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await imageFile2.CopyToAsync(stream);
                }

                urlDto.ImageUrl2 = fileName2;
            }

            return Ok(urlDto);
        }

        catch (IOException ex)
        {
            return StatusCode(500, $"Dosya yükleme sırasında bir hata oluştu: {ex.Message}");
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Beklenmedik bir hata oluştu: {ex.Message}");
        }
    }

}
