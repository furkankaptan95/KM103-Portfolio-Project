using App.DataAPI.Services.AdminServices;
using App.DTOs.BlogPostDtos.Admin;
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
    private readonly IValidator<AddContactMessageDto> _validator;
    public ContactMessageController(IContactMessagePortfolioService contactMessagePortfolioService, IValidator<AddContactMessageDto> validator,IContactMessageAdminService contactMessageAdminService)
    {
        _contactMessagePortfolioService = contactMessagePortfolioService;
        _validator = validator;
        _contactMessageAdminService = contactMessageAdminService;
    }
    [HttpPost("/add-contact-message")]
    public async Task<IActionResult> AddAsync([FromBody] AddContactMessageDto dto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(dto);
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

                return StatusCode(500, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Beklenmedik bir hata oluştu: {ex.Message}");
        }
    }

    [HttpPut("/reply-contact-message")]
    public async Task<IActionResult> ReplyAsync([FromBody] ReplyContactMessageDto dto)
    {
        try
        {
            //var validationResult = await _updateValidator.ValidateAsync(dto);

            //if (!validationResult.IsValid)
            //{
            //    var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            //    return BadRequest(Result.Invalid(new ValidationError(errorMessage)));
            //}

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
}
