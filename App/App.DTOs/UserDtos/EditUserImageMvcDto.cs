using Microsoft.AspNetCore.Http;

namespace App.DTOs.UserDtos;

public class EditUserImageMvcDto
{
    public string Email { get; set; }
    public IFormFile ImageFile { get; set; }
}
