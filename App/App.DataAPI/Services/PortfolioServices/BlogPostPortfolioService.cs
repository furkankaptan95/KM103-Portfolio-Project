using App.Data.DbContexts;
using App.DTOs.BlogPostDtos.Porfolio;
using App.DTOs.CommentDtos.Portfolio;
using App.Services.PortfolioServices.Abstract;
using Ardalis.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.DataAPI.Services.PortfolioServices;
public class BlogPostPortfolioService(DataApiDbContext dataApiDb,ICommentPortfolioService commentPortfolioService) : IBlogPostPortfolioService
{
	public async Task<Result<List<HomeBlogPostsPortfolioDto>>> GetHomeBlogPostsAsync()
	{
		try
        {
            var dtos = new List<HomeBlogPostsPortfolioDto>();

            var entities = await dataApiDb.BlogPosts.Where(bp=>bp.IsVisible == true).Include(b=>b.Comments).ToListAsync();

            if (entities is null)
            {
                return Result<List<HomeBlogPostsPortfolioDto>>.Success(dtos);
            }

            dtos = entities
           .Select(item => new HomeBlogPostsPortfolioDto
           {
               Id = item.Id,
               Title = item.Title,
               Content = item.Content,
               PublishDate = item.PublishDate,
			   CommentsCount = item.Comments.Count,
           })
           .ToList();

            return Result<List<HomeBlogPostsPortfolioDto>>.Success(dtos);
        }
        catch (SqlException sqlEx)
        {
            return Result<List<HomeBlogPostsPortfolioDto>>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            return Result<List<HomeBlogPostsPortfolioDto>>.Error("Bir hata oluştu: " + ex.Message);
        }
	}

	public async Task<Result<SingleBlogPostDto>> GetBlogPostById(int id)
	{
		try
		{
			var entity = await dataApiDb.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

			if (entity is null)
			{
				return Result<SingleBlogPostDto>.NotFound();
			}

			var commentsResult = await commentPortfolioService.GetBlogPostCommentsAsync(id);

			List<BlogPostCommentsPortfolioDto> commentDtos = null;

			if (commentsResult.IsSuccess)
			{
				commentDtos = commentsResult.Value;
			}

			var dto = new SingleBlogPostDto
			{
				Id = id,
				Title = entity.Title,
				Content = entity.Content,
				PublishDate = entity.PublishDate,
				Comments = commentDtos,
			};

			return Result<SingleBlogPostDto>.Success(dto);
		}
		catch (SqlException sqlEx)
		{
			return Result<SingleBlogPostDto>.Error("Veritabanı bağlantı hatası: " + sqlEx.Message);
		}
		catch (Exception ex)
		{
			return Result<SingleBlogPostDto>.Error("Bir hata oluştu: " + ex.Message);
		}
	}
}
