using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IValidator<AddBlogPostDto> _addValidator;
    private readonly IBlogPostService _blogPostService;
    public BlogPostsController(IValidator<AddBlogPostDto> addValidator, IBlogPostService blogPostService)
    {
        _addValidator = addValidator;
        _blogPostService = blogPostService;
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
}
