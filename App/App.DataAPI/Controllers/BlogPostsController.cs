using App.DTOs.BlogPostDtos;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IValidator<AddBlogPostDto> _addValidator;
    public BlogPostsController(IValidator<AddBlogPostDto> addValidator)
    {
        _addValidator = addValidator;
    }

    [HttpPost]
    [Route("/add-blog-post")]
    public async Task<IActionResult> AddBlogPostAsync([FromBody] AddBlogPostDto dto)
    {
        var validationResult = await _addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
        }

        return Ok();
    }
}
