using Microsoft.AspNetCore.Http;

namespace App.DTOs.ProjectDtos;
public class AddProjectMVCDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile ImageFile { get; set; }
}
