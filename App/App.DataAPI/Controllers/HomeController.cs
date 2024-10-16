using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService) : ControllerBase
{
    [HttpGet("/get-home-infos")]
    public async Task<IActionResult> GetHomeInfosAsync()
    {
        try
        {
            var result = await homeService.GetHomeInfosAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
}
