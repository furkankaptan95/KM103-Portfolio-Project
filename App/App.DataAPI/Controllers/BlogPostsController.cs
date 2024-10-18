using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;


namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IValidator<AddBlogPostDto> _addValidator;
    private readonly IValidator<UpdateBlogPostDto> _updateValidator;
    private readonly IBlogPostAdminService _blogPostService;
    public BlogPostsController(IValidator<AddBlogPostDto> addValidator, IValidator<UpdateBlogPostDto> updateValidator, IBlogPostAdminService blogPostService)
    {
        _addValidator = addValidator;
        _blogPostService = blogPostService;
        _updateValidator = updateValidator;
    }

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

            var result = await _blogPostService.AddBlogPostAsync(dto);

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

    [HttpGet("/all-blog-posts")]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        try
        {
            var result = await _blogPostService.GetAllBlogPostsAsync();

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


    [HttpGet("/blog-post-{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _blogPostService.GetBlogPostById(id);

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

            var result = await _blogPostService.UpdateBlogPostAsync(dto);

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

    [HttpDelete("/delete-blog-post-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _blogPostService.DeleteBlogPostAsync(id);

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

    [HttpGet("/change-blog-post-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibilityAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _blogPostService.ChangeBlogPostVisibilityAsync(id);

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
