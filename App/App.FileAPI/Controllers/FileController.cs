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

        if(imageFile1 is null && imageFile2 is null)
        {
            return BadRequest("İşleme devam edebilmek için en az 1 adet resim dosyası  (.jpg, .jpeg, .png, .gif) yüklemelisiniz.");
        }

        try
        {
            // Geçerli resim uzantıları
            var validImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            // İlk dosya kontrolü
            if (imageFile1 is not null)
            {
                if (!validImageExtensions.Contains(Path.GetExtension(imageFile1.FileName).ToLower()))
                {
                    return BadRequest("Yalnızca resim dosyaları (.jpg, .jpeg, .png, .gif) yüklenebilir.");
                }

                // İlk dosyayı kaydet
                var fileExtension1 = Path.GetExtension(imageFile1.FileName);
                var uniqueFileName1 = $"{Guid.NewGuid()}{fileExtension1}";
                var filePath1 = Path.Combine(_uploadsFolder, uniqueFileName1);
                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    await imageFile1.CopyToAsync(stream);
                }

                urlDto.ImageUrl1 = uniqueFileName1;
            }

            // İkinci dosya kontrolü
            if (imageFile2 is not null)
            {
                if (!validImageExtensions.Contains(Path.GetExtension(imageFile2.FileName).ToLower()))
                {
                    return BadRequest("Yalnızca resim dosyaları (.jpg, .jpeg, .png, .gif) yüklenebilir.");
                }

                // İlk dosyayı kaydet
                var fileExtension2 = Path.GetExtension(imageFile2.FileName);
                var uniqueFileName2 = $"{Guid.NewGuid()}{fileExtension2}";
                var filePath2 = Path.Combine(_uploadsFolder, uniqueFileName2);
                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await imageFile2.CopyToAsync(stream);
                }

                urlDto.ImageUrl2 = uniqueFileName2;
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


    [HttpDelete("/delete-file/{fileName}")]
    public IActionResult DeleteFileAsync([FromRoute] string fileName)
    {
        try
        {
            // Klasör yolunu belirle (örneğin, wwwroot/uploads)
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            // Dosyanın var olup olmadığını kontrol et
            if (System.IO.File.Exists(filePath))
            {
                // Dosyayı sil
                System.IO.File.Delete(filePath);
                return Ok(new { message = "Dosya başarıyla silindi." });
            }
            else
            {
                return NotFound(new { message = "Dosya bulunamadı." });
            }
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