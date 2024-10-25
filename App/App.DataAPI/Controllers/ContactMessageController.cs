using App.Core;
using App.DataAPI.Services.AdminServices;
using App.DTOs.ContactMessageDtos.Admin;
using App.DTOs.ContactMessageDtos.Portfolio;
using App.Services.AdminServices.Abstract;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App.DataAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactMessageController : ControllerBase
{
    private readonly IContactMessagePortfolioService _contactMessagePortfolioService;
    private readonly IContactMessageAdminService _contactMessageAdminService;
    private readonly IValidator<AddContactMessageDto> _addValidator;
    private readonly IValidator<ReplyContactMessageDto> _replyValidator;

    public ContactMessageController(IContactMessagePortfolioService contactMessagePortfolioService, IValidator<AddContactMessageDto> addValidator,IContactMessageAdminService contactMessageAdminService, IValidator<ReplyContactMessageDto> replyValidator)
    {
        _contactMessagePortfolioService = contactMessagePortfolioService;
        _addValidator = addValidator;
        _contactMessageAdminService = contactMessageAdminService;
        _replyValidator = replyValidator;
    }
    [AllowAnonymousManuel]
    [HttpPost("/add-contact-message")]
    public async Task<IActionResult> AddAsync([FromBody] AddContactMessageDto dto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(dto);
            string errorMessage;

            if (!validationResult.IsValid)
            {
                errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _contactMessagePortfolioService.AddContactMessageAsync(dto);

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
    [AuthorizeRoles("admin")]
    [HttpGet("/all-contact-messages")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _contactMessageAdminService.GetAllContactMessagesAsync();

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
    [HttpGet("/get-contact-message-{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _contactMessageAdminService.GetContactMessageByIdAsync(id);

            if (!result.IsSuccess)
            {
                if (result.Status == ResultStatus.NotFound)
                {
                    return NotFound(result);
                }
                else if(result.Status == ResultStatus.Conflict)
                {
                    return Conflict(result);
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
    [AuthorizeRoles("admin")]
    [HttpPut("/reply-contact-message")]
    public async Task<IActionResult> ReplyAsync([FromBody] ReplyContactMessageDto dto)
    {
        try
        {
            var validationResult = await _replyValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            }

            var result = await _contactMessageAdminService.ReplyContactMessageAsync(dto);

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
    [AuthorizeRoles("admin")]
    [HttpGet("/make-message-read-{id:int}")]
    public async Task<IActionResult> MakeMessageReadAsync([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(Result.Error("Geçersiz ID bilgisi."));
        }

        try
        {
            var result = await _contactMessageAdminService.ChangeIsReadAsync(id);

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
