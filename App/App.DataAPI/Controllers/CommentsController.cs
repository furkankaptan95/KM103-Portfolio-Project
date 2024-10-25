using App.Core;
using App.DTOs.CommentDtos.Portfolio;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentAdminService _commentAdminService;
    private readonly ICommentPortfolioService _commentPortfolioService;
    private readonly IValidator<AddCommentSignedDto> _addCommentSignedDtoValidator;
    private readonly IValidator<AddCommentUnsignedDto> _addCommentUnsignedDtoValidator;
    public CommentsController(ICommentAdminService commentAdminService,ICommentPortfolioService commentPortfolioService, IValidator<AddCommentSignedDto> addCommentSignedDtoValidator, IValidator<AddCommentUnsignedDto> addCommentUnsignedDtoValidator)
    {
        _commentAdminService = commentAdminService;
        _commentPortfolioService = commentPortfolioService;
        _addCommentSignedDtoValidator = addCommentSignedDtoValidator;
        _addCommentUnsignedDtoValidator = addCommentUnsignedDtoValidator;
    }

    [AuthorizeRoles("admin")]

    [HttpGet("/all-comments")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _commentAdminService.GetAllCommentsAsync();

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

    [AuthorizeRoles("admin")]
    [HttpDelete("/delete-comment-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _commentAdminService.DeleteCommentAsync(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
    [AuthorizeRoles("admin")]
    [HttpGet("/(not)-approve-comment-{id:int}")]
    public async Task<IActionResult> ApproveNotApproveAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _commentAdminService.ApproveOrNotApproveCommentAsync(id);

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
            return StatusCode(500, Result.Error($"Beklenmedik bir hata oluştu: {ex.Message}"));
        }
    }
    [AuthorizeRoles("admin")]
    [HttpGet("/get-users-comments-{id:int}")]
    public async Task<IActionResult> GetUsersCommentsAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _commentAdminService.GetUsersCommentsAsync(id);

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
    [HttpPost("/add-comment-unsigned")]
    public async Task<IActionResult> AddUnsignedAsync([FromBody] AddCommentUnsignedDto dto)
    {
        try
        {
            var validationResult = await _addCommentUnsignedDtoValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _commentPortfolioService.AddCommentUnsignedAsync(dto);

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
    [AuthorizeRoles("admin", "commenter")]
    [HttpPost("/add-comment-signed")]
    public async Task<IActionResult> AddSignedAsync([FromBody] AddCommentSignedDto dto)
    {
        try
        {
            var validationResult = await _addCommentSignedDtoValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _commentPortfolioService.AddCommentSignedAsync(dto);

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
