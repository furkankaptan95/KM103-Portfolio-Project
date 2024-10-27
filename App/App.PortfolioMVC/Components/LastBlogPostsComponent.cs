using Microsoft.AspNetCore.Mvc;

namespace App.PortfolioMVC.Components;
public class LastBlogPostsComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{

		return View(); // Görünümü döndürün.
	}
}
