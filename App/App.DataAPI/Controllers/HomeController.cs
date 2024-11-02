using App.Core.Authorization;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeAdminService homeAdminService, IHomePortfolioService homePortfolioService) : ControllerBase
{
	[AuthorizeRolesApi("admin")]
	[HttpGet("/get-home-infos")]
    public async Task<IActionResult> GetHomeInfosAsync()
    {
        try
        {
            var result = await homeAdminService.GetHomeInfosAsync();

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

    [AllowAnonymousManuel]
    [HttpGet("/get-cv-url")]
	public async Task<IActionResult> GetCvUrl()
	{
        try
        {
            var result = await homePortfolioService.GetCvUrlAsync();

            if (result.IsSuccess)
            {
                return Ok(result);
            }

			return NotFound(result);
		}
        
        catch (Exception ex)
        {
			return StatusCode(500, Result<string>.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
		}
	}

	[AuthorizeRolesApi("admin")]
	[HttpPost("/add-cv")]
    public async Task<IActionResult> AddCvAsync([FromBody] string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return BadRequest(Result.Invalid());
        }

        try
        {
            var result = await homeAdminService.UploadCvAsync(url);

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
