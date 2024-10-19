using App.DTOs.ContactMessageDtos.Portfolio;
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
    private readonly IValidator<AddContactMessageDto> _validator;
    public ContactMessageController(IContactMessagePortfolioService contactMessagePortfolioService, IValidator<AddContactMessageDto> validator)
    {
        _contactMessagePortfolioService = contactMessagePortfolioService;
        _validator = validator;
    }
    [HttpPost("/add-contact-messgae")]
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
}
