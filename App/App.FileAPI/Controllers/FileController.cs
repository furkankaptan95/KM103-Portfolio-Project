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
}
