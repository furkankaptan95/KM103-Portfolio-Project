using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Controllers;

public class HomeController(IEducationPortfolioService educationService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel();

        var educationsResult = await educationService.GetAllEducationsAsync();

        if (educationsResult.IsSuccess && educationsResult.Value.Count > 0)
        {
            model.Educations = new();

            foreach(var education in educationsResult.Value)
            {
                var educationToAdd = new AllEducationsPortfolioViewModel();

                educationToAdd.StartDate = education.StartDate;
                educationToAdd.EndDate = education.EndDate;
                educationToAdd.Degree = education.Degree;
                educationToAdd.School = education.School;

                model.Educations.Add(educationToAdd);
            }
        }

        return View(model);
    }

}
