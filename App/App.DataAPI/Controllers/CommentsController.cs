using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("/all-comments")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _commentService.GetAllCommentsAsync();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }

    [HttpDelete("/delete-comment-{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _commentService.DeleteCommentAsync(id);

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

    [HttpGet("/(not)-approve-comment-{id:int}")]
    public async Task<IActionResult> ApproveNotApproveAsync([FromRoute] int id)
    {
        var result = await _commentService.ApproveOrNotApproveCommentAsync(id);

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

    [HttpGet("/get-users-comments-{id:int}")]
    public async Task<IActionResult> GetUsersCommentsAsync([FromRoute] int id)
    {
        var result = await _commentService.GetUsersCommentsAsync(id);

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}
