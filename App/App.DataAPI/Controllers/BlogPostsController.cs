using App.Core;
using App.DTOs.BlogPostDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using App.Core.Authorization;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class BlogPostsController : ControllerBase
{
    private readonly IValidator<AddBlogPostDto> _addValidator;
    private readonly IValidator<UpdateBlogPostDto> _updateValidator;
    private readonly IBlogPostAdminService _blogPostAdminService;
	private readonly IBlogPostPortfolioService _blogPostPortfolioService;

	public BlogPostsController(IValidator<AddBlogPostDto> addValidator, IValidator<UpdateBlogPostDto> updateValidator, IBlogPostAdminService blogPostAdminService, IBlogPostPortfolioService blogPostPortfolioService)
    {
        _addValidator = addValidator;
		_blogPostAdminService = blogPostAdminService;
        _updateValidator = updateValidator;
        _blogPostPortfolioService = blogPostPortfolioService;
    }
    [AuthorizeRolesApi("admin")]
    [HttpPost("/add-blog-post")]
    public async Task<IActionResult> AddBlogPostAsync([FromBody] AddBlogPostDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _blogPostAdminService.AddBlogPostAsync(dto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return StatusCode(500, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
    [AuthorizeRolesApi("admin")]
    [HttpGet("/all-blog-posts")]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        try
        {
            var result = await _blogPostAdminService.GetAllBlogPostsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
    [AllowAnonymousManuel]
    [HttpGet("/home-blog-posts")]
    public async Task<IActionResult> GetHomeBlogPosts()
    {
        try
        {
            var result = await _blogPostPortfolioService.GetHomeBlogPostsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
    [AuthorizeRolesApi("admin")]
    [HttpGet("/blog-post-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _blogPostAdminService.GetBlogPostById(id);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500,$"Beklenmedik bir hata oluştu: {ex.Message}");
        }
    }
    [CommonArea]
    [HttpGet("/portfolio-blog-post-{id:int}")]
	public async Task<IActionResult> GetByIdPortfolioAsync([FromRoute] int id)
	{
		if (id <= 0)
		{
			return BadRequest(Result.Error("Geçersiz ID bilgisi."));
		}

		try
		{
			var result = await _blogPostPortfolioService.GetBlogPostById(id);

			if (!result.IsSuccess)
			{
				if (result.Status == ResultStatus.NotFound)
				{
					return NotFound(result);
				}

				return StatusCode(500, result);
			}

			return Ok(result);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Beklenmedik bir hata oluştu: {ex.Message}");
		}
	}
    [AuthorizeRolesApi("admin")]
    [HttpPut("/update-blog-post")]
    public async Task<IActionResult> UpdateBlogPostAsync([FromBody] UpdateBlogPostDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _blogPostAdminService.UpdateBlogPostAsync(dto);

            if (!result.IsSuccess)
            {
                if(result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Beklenmedik bir hata oluştu: {ex.Message}");
        }

    }
    [AuthorizeRolesApi("admin")]
    [HttpDelete("/delete-blog-post-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _blogPostAdminService.DeleteBlogPostAsync(id);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Beklenmedik bir hata oluştu: {ex.Message}");
        }
    }
    [AuthorizeRolesApi("admin")]
    [HttpGet("/change-blog-post-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _blogPostAdminService.ChangeBlogPostVisibilityAsync(id);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }
                return StatusCode(500, result);
            }
            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Beklenmedik bir hata oluştu: {ex.Message}");
        }
    }
}
