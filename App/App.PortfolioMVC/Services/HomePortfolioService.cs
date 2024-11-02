using App.DTOs.AboutMeDtos.Porfolio;
using App.DTOs.BlogPostDtos.Porfolio;
using App.DTOs.EducationDtos.Portfolio;
using App.DTOs.ExperienceDtos.Portfolio;
using App.DTOs.PersonalInfoDtos.Portfolio;
using App.DTOs.ProjectDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using App.ViewModels.PortfolioMvc;
using App.ViewModels.PortfolioMvc.BlogPostsViewModels;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Services;
public class HomePortfolioService(IEducationPortfolioService educationService,IExperiencePortfolioService experienceService,IProjectPortfolioService projectService,IPersonalInfoPortfolioService personalInfoService,IAboutMePortfolioService aboutMeService,IBlogPostPortfolioService blogPostService, IHttpClientFactory factory) : IHomePortfolioService
{
	private HttpClient FileApiClient => factory.CreateClient("fileApi");
	private HttpClient DataApiClient => factory.CreateClient("dataApi");
	public async Task<Result<HomeIndexViewModel>> GetHomeInfosAsync()
	{
		var model = new HomeIndexViewModel();

		try
		{
            var educationsResult = await educationService.GetAllEducationsAsync();
            var experienceResult = await experienceService.GetAllExperiencesAsync();
            var projectResult = await projectService.GetAllProjectsAsync();
            var aboutMeResult = await aboutMeService.GetAboutMeAsync();
            var personalInfoResult = await personalInfoService.GetPersonalInfoAsync();
            var blogPostResult = await blogPostService.GetHomeBlogPostsAsync();

            if (educationsResult.IsSuccess)
            {
                model.Educations = Educations(educationsResult.Value);
            }
            if (experienceResult.IsSuccess)
            {
                model.Experiences = Experiences(experienceResult.Value);
            }
            if (projectResult.IsSuccess)
            {
                model.Projects = Projects(projectResult.Value);
            }
            if (aboutMeResult.IsSuccess)
            {
                model.AboutMe = AboutMe(aboutMeResult.Value);
            }
            else if (aboutMeResult.Status == ResultStatus.NotFound)
            {
                model.AboutMe = new();
            }
            if (personalInfoResult.IsSuccess)
            {
                model.PersonalInfo = PersonalInfo(personalInfoResult.Value);
            }
            else if (personalInfoResult.Status == ResultStatus.NotFound)
            {
                model.PersonalInfo = new();
            }
            if (blogPostResult.IsSuccess)
            {
                model.BlogPosts = BlogPosts(blogPostResult.Value);
            }

            return Result<HomeIndexViewModel>.Success(model);
        }
        catch (Exception)
        {
            return Result<HomeIndexViewModel>.Error();
        }
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

		model.FullName = dto.FullName;
		model.Field = dto.Field;
		model.Introduction = dto.Introduction;
		model.ImageUrl1 = dto.ImageUrl1;
		model.ImageUrl2 = dto.ImageUrl2;

		return model;
	}
	private static PersonalInfoPortfolioViewModel PersonalInfo(PersonalInfoPortfolioDto dto)
	{
		var model = new PersonalInfoPortfolioViewModel();

		model.About = dto.About;
		model.Name = dto.Name;
		model.Surname = dto.Surname;
		model.BirthDate = dto.BirthDate;
		model.Email = dto.Email;
		model.Link = dto.Link;
		model.Adress = dto.Adress;
    
		return model;
	}
	private static List<HomeBlogPostsPortfolioViewModel> BlogPosts(List<BlogPostsPortfolioDto> dtos)
	{
		var models =new List<HomeBlogPostsPortfolioViewModel>();

		var filteredDtos = dtos.Take(3).ToList();

        foreach (var blogPost in filteredDtos)
        {
            var blogPostToAdd = new HomeBlogPostsPortfolioViewModel();

            blogPostToAdd.Title = blogPost.Title;
			blogPostToAdd.Content = blogPost.Content;
			blogPostToAdd.Id = blogPost.Id;
			blogPostToAdd.PublishDate = blogPost.PublishDate;
			blogPostToAdd.CommentsCount = blogPost.CommentsCount;

            models.Add(blogPostToAdd);
        }
        return models;
    }

	public Task<Result<string>> GetCvUrlAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<byte[]>> DownloadCvAsync()
	{
		try
		{
			var urlResponse = await DataApiClient.GetAsync("get-cv-url");

			if (!urlResponse.IsSuccessStatusCode)
			{
				return Result<byte[]>.Error();
			}

			var urlResult = await urlResponse.Content.ReadFromJsonAsync<Result<string>>();

			if (urlResult is null)
			{
				return Result<byte[]>.Error();
			}

			var response = await FileApiClient.GetAsync($"download?fileUrl={urlResult.Value}");

			if (!response.IsSuccessStatusCode)
			{
				return Result<byte[]>.Error();
			}

			var result = await response.Content.ReadAsByteArrayAsync(); // Dosya bytes olarak döndür

			if (result is null)
			{
				return Result<byte[]>.Error();
			}

			return Result<byte[]>.Success(result);
		}
		catch (Exception)
		{
			return Result<byte[]>.Error();
		}
	}
}
