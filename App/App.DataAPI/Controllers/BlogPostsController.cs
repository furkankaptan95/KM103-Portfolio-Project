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
    private readonly IBlogPostService _blogPostService;
    public BlogPostsController(IValidator<AddBlogPostDto> addValidator, IValidator<UpdateBlogPostDto> updateValidator, IBlogPostService blogPostService)
    {
        _addValidator = addValidator;
        _blogPostService = blogPostService;
        _updateValidator = updateValidator;
    }

    [HttpPost("/add-blog-post")]
    public async Task<IActionResult> AddBlogPostAsync([FromBody] AddBlogPostDto dto)
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

    [HttpGet("/all-blog-posts")]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        var result = await _blogPostService.GetAllBlogPostsAsync();

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("/blog-post-{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
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

    [HttpPut("/update-blog-post")]
    public async Task<IActionResult> UpdateBlogPostAsync([FromBody] UpdateBlogPostDto dto)
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
            return StatusCode(500, result);
        }

        return Ok(result);
    }
    [HttpDelete("/delete-blog-post-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
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

    [HttpGet("/change-blog-post-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibility([FromRoute] int id)
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

}
