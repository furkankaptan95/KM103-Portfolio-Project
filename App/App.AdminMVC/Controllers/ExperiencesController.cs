using App.DTOs.ExperienceDtos;
using App.DTOs.ExperienceDtos.Admin;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.ExperiencesViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ExperiencesController(IExperienceAdminService experienceService) : Controller
{
    [HttpGet]
    [Route("all-experiences")]
    public async Task<IActionResult> AllExperiences()
    {
        try
        {
            var result = await experienceService.GetAllExperiencesAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/home/index");
            }

            var models = new List<AdminAllExperiencesViewModel>();
            var dtos = result.Value;

            models = dtos
           .Select(item => new AdminAllExperiencesViewModel
           {
               Id = item.Id,
               Title = item.Title,
               Company = item.Company,
               Description = item.Description,
               StartDate = item.StartDate,
               EndDate = item.EndDate,
               IsVisible = item.IsVisible
           })
           .ToList();

            return View(models);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Deneyimler getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("add-experience")]
    public IActionResult AddExperience()
    {
        return View();
    }

    [HttpPost]
    [Route("add-experience")]
    public async Task<IActionResult> AddExperience([FromForm] AddExperienceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new AddExperienceDto
            {
                Title = model.Title,
                Company = model.Company,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
            };

            var result = await experienceService.AddExperienceAsync(dto);

            if (!result.IsSuccess)
            {
                ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return View(model);
            }
            
            TempData["Message"] = result.SuccessMessage;

            return Redirect("/all-experiences");
        }
        
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Deneyim bilgisi eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("update-experience-{id:int}")]
    public async Task<IActionResult> UpdateExperience([FromRoute] int id)
    {
        try
        {
            var result = await experienceService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/all-experiences");
            }

            var dto = result.Value;

            var model = new UpdateExperienceViewModel
            {
                Id = id,
                Company = dto.Company,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Title = dto.Title,
                Description = dto.Description,
            };

            return View(model);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Güncellemek istediğiniz Deneyim bilgileri getirilirken beklenmeyen bir hata oluştu.";
            return Redirect("/all-experiences");
        }
    }

    [HttpPost]
    [Route("update-experience")]
    public async Task<IActionResult> UpdateExperience([FromForm] UpdateExperienceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new UpdateExperienceDto
            {
                Id = model.Id,
                Description = model.Description,
                Title = model.Title,
                Company = model.Company,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
            };

            var result = await experienceService.UpdateExperienceAsync(dto);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if (result.Status==ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/all-experiences");
                }
                ViewData["ErrorMessage"] = errorMessage;
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/all-experiences");
        }
       
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("delete-experience-{id:int}")]
    public async Task<IActionResult> DeleteExperience([FromRoute] int id)
    {
        try
        {
            var result = await experienceService.DeleteExperienceAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-experiences");
        }
        
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Deneyim bilgisi silinirken beklenmedik bir hata oluştu..";
            return Redirect("/all-experiences");
        }
    }

    [HttpGet]
    [Route("change-experience-visibility-{id:int}")]
    public async Task<IActionResult> ChangeExperienceVisibility([FromRoute] int id)
    {
        try
        {
            var result = await experienceService.ChangeExperienceVisibilityAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-experiences");
        }
        
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Deneyimin görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-experiences");
        }
    }

}
