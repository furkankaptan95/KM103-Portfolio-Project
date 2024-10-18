using App.DTOs.EducationDtos;
using App.Services.AdminServices.Abstract;
using App.ViewModels.AdminMvc.EducationsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class EducationsController(IEducationService educationService) : Controller
{

    [HttpGet]
    [Route("all-educations")]
    public async Task<IActionResult> AllEducations()
    {
        try
        {
            var result = await educationService.GetAllEducationsAsync();

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/home/index");
            }

            var models = new List<AdminAllEducationsViewModel>();
            var dtos = result.Value;

            models = dtos
           .Select(item => new AdminAllEducationsViewModel
           {
               Id = item.Id,
               School = item.School,
               Degree = item.Degree,
               StartDate = item.StartDate,
               EndDate = item.EndDate,
               IsVisible = item.IsVisible
           })
           .ToList();

            return View(models);
        }
        
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Eğitimler getirilirken beklenmedik bir hata oluştu..";
            return Redirect("/home/index");
        }
    }

    [HttpGet]
    [Route("add-education")]
    public IActionResult AddEducation()
    {
        return View();
    }

    [HttpPost]
    [Route("add-education")]
    public async Task<IActionResult> AddEducation([FromForm] AddEducationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new AddEducationDto
            {
                Degree = model.Degree,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                School = model.School,
            };

            var result = await educationService.AddEducationAsync(dto);

            if (!result.IsSuccess)
            {
                ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return View(model);
            }

            TempData["Message"] = result.SuccessMessage;

            return Redirect("/all-educations");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Eğitim bilgisi eklenirken beklenmedik bir hata oluştu..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("update-education-{id:int}")]
    public async Task<IActionResult> UpdateEducation([FromRoute] int id)
    {
        try
        {
            var result = await educationService.GetEducationByIdAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return Redirect("/all-educations");
            }

            var dto = result.Value;

            var educationToUpdate = new UpdateEducationViewModel
            {
                Id = id,
                School = dto.School,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Degree = dto.Degree,
            };

            return View(educationToUpdate);
        }
      

        catch (Exception)
        {
            TempData["ErrorMessage"] = "Güncellemek istediğiniz Eğitim bilgileri getirilirken beklenmeyen bir hata oluştu.";
            return Redirect("/all-educations");
        }
    }

    [HttpPost]
    [Route("update-education")]
    public async Task<IActionResult> UpdateEducation([FromForm] UpdateEducationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var dto = new UpdateEducationDto
            {
                Id = model.Id,
                School = model.School,
                Degree = model.Degree,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
            };

            var result = await educationService.UpdateEducationAsync(dto);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault();

                if(result.Status == ResultStatus.NotFound)
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return Redirect("/all-educations");
                }

                ViewData["ErrorMessage"] = errorMessage;
                return View(model);               
            }
            
            TempData["Message"] = result.SuccessMessage;

            return Redirect("/all-educations");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Güncelleme işlemi sırasında beklenmedik bir hata oluştu!..Tekrar deneyebilirsiniz.";
            return View(model);
        }
    }

    [HttpGet]
    [Route("delete-education-{id:int}")]
    public async Task<IActionResult> DeleteEducation([FromRoute] int id)
    {
        try
        {
            var result = await educationService.DeleteEducationAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }

            return Redirect("/all-educations");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Eğitim bilgisi silinirken beklenmedik bir hata oluştu..";
            return Redirect("/all-educations");
        }
    }


    [HttpGet]
    [Route("change-education-visibility-{id:int}")]
    public async Task<IActionResult> ChangeVisibility([FromRoute] int id)
    {
        try
        {
            var result = await educationService.ChangeEducationVisibilityAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }

            else
            {
                TempData["Message"] = result.SuccessMessage;
            }
            return Redirect("/all-educations");
        }
       
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Eğitim'in görünürlüğü değiştirilirken beklenmeyen bir hata oluştu..";
            return Redirect("/all-educations");
        }
    }
}
