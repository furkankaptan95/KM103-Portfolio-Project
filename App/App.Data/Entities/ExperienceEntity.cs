using App.Core.Entities;
namespace App.Data.Entities;
public class ExperienceEntity : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;

}
