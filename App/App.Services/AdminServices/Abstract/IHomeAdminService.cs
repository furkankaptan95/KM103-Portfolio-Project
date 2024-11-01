using App.DTOs.HomeDtos;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Services.AdminServices.Abstract;
public interface IHomeAdminService
{
    Task<Result<HomeDto>> GetHomeInfosAsync();
    Task<Result> UploadCvAsync(IFormFile cv);
    Task<Result> UploadCvAsync(string url);
}
