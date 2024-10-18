using App.DTOs.EducationDtos.Portfolio;
using App.DTOs.ExperienceDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class HomePortfolioService(IEducationPortfolioService educationService,IExperiencePortfolioService experienceService) : IHomePortfolioService
{
	public async Task<Result<HomeIndexViewModel>> GetHomeInfosAsync()
	{
		 var model = new HomeIndexViewModel();

        var educationsResult = await educationService.GetAllEducationsAsync();
        var experienceResult = await experienceService.GetAllExperiencesAsync();

        if (educationsResult.IsSuccess)
        {
            model.Educations = Educations(educationsResult.Value);
        }
		if (experienceResult.IsSuccess)
		{
			model.Experiences = Experiences(experienceResult.Value);
		}

		return Result<HomeIndexViewModel>.Success(model);
	}

	private static List<AllEducationsPortfolioViewModel> Educations(List<AllEducationsPortfolioDto> dtos)
	{
		var models = new List<AllEducationsPortfolioViewModel>();

		foreach (var education in dtos)
		{
			var educationToAdd = new AllEducationsPortfolioViewModel();

			educationToAdd.StartDate = education.StartDate;
			educationToAdd.EndDate = education.EndDate;
			educationToAdd.Degree = education.Degree;
			educationToAdd.School = education.School;

			models.Add(educationToAdd);
		}

		return models;
	}
	private static List<AllExperiencesPortfolioViewModel> Experiences(List<AllExperiencesPortfolioDto> dtos)
	{
		var models = new List<AllExperiencesPortfolioViewModel>();

		foreach (var experience in dtos)
		{
			var experienceToAdd = new AllExperiencesPortfolioViewModel();

			experienceToAdd.StartDate = experience.StartDate;
			experienceToAdd.EndDate = experience.EndDate;
			experienceToAdd.Description = experience.Description;
			experienceToAdd.Title = experience.Title;
			experienceToAdd.Company = experience.Company;

			models.Add(experienceToAdd);
		}
		return models;
	}
}
