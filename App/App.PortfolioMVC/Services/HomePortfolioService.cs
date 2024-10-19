using App.DTOs.AboutMeDtos.Porfolio;
using App.DTOs.EducationDtos.Portfolio;
using App.DTOs.ExperienceDtos.Portfolio;
using App.DTOs.ProjectDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using Ardalis.Result;

namespace App.PortfolioMVC.Services;
public class HomePortfolioService(IEducationPortfolioService educationService,IExperiencePortfolioService experienceService,IProjectPortfolioService projectService,IAboutMePortfolioService aboutMeService) : IHomePortfolioService
{
	public async Task<Result<HomeIndexViewModel>> GetHomeInfosAsync()
	{
		 var model = new HomeIndexViewModel();

        var educationsResult = await educationService.GetAllEducationsAsync();
        var experienceResult = await experienceService.GetAllExperiencesAsync();
		var projectResult = await projectService.GetAllProjectsAsync();
		var aboutMeResult = await aboutMeService.GetAboutMeAsync();

        if (educationsResult.IsSuccess)
        {
            model.Educations = Educations(educationsResult.Value);
        }
		if (experienceResult.IsSuccess)
		{
			model.Experiences = Experiences(experienceResult.Value);
		}
        if (experienceResult.IsSuccess)
        {
            model.Projects = Projects(projectResult.Value);
        }
		if (aboutMeResult.IsSuccess)
		{
			model.AboutMe = AboutMe(aboutMeResult.Value);
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

	private static List<AllProjectsPortfolioViewModel> Projects(List<AllProjectsPortfolioDto> dtos)
	{
		var models = new List<AllProjectsPortfolioViewModel>();

		foreach(var project in dtos)
		{
			var projectToAdd = new AllProjectsPortfolioViewModel();

			projectToAdd.Title = project.Title;
			projectToAdd.Description = project.Description;
			projectToAdd.ImageUrl = project.ImageUrl;

			models.Add(projectToAdd);
		}
		return models;
    }
	private static AboutMePortfolioViewModel AboutMe(AboutMePortfolioDto dto)
	{
		var model = new AboutMePortfolioViewModel();

		model.Introduction = dto.Introduction;
		model.ImageUrl1 = dto.ImageUrl1;
		model.ImageUrl2 = dto.ImageUrl2;

		return model;
	}
}
