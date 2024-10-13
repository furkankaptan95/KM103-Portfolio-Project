using App.DTOs.AboutMeDtos;
using App.DTOs.BlogPostDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;
using FluentValidation;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.AdminMVC.Services;
public class BlogPostService : IBlogPostService
{
    private readonly IHttpClientFactory _factory;
    private readonly IValidator<AddBlogPostDto> _addValidator;
    public BlogPostService(IHttpClientFactory factory, IValidator<AddBlogPostDto> addValidator)
    {
        _factory = factory;
        _addValidator = addValidator;
    }

    private HttpClient DataApiClient => _factory.CreateClient("dataApi");
    public async Task<Result> AddBlogPostAsync(AddBlogPostDto dto)
    {
        var validationResult = await _addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Invalid(new ValidationError(errorMessage));
        }

        var apiResponse = await DataApiClient.PostAsJsonAsync("add-blog-post", dto);

        return await apiResponse.Content.ReadFromJsonAsync<Result>();

    }

    public Task<Result> ChangeBlogPostVisibilityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteBlogPostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AllBlogPostsDto>>> GetAllBlogPostsAsync()
    {

        var apiResponse = await DataApiClient.GetAsync("all-blog-posts");

        return await apiResponse.Content.ReadFromJsonAsync<Result<List<AllBlogPostsDto>>>();

    }

    public Task<Result> UpdateBlogPostAsync(UpdateBlogPostDto dto)
    {
        throw new NotImplementedException();
    }
}
