using App.Core.Entities;
namespace App.Data.Entities;
public class ProjectEntity : BaseEntity<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool IsVisible { get; set; } = true;

}
