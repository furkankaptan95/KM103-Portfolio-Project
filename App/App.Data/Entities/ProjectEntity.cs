using App.Core.Entities;
namespace App.Data.Entities;
public class ProjectEntity : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;

}
