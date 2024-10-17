using App.Core.Entities;
namespace App.Data.Entities;
public class ExperienceEntity : BaseEntity<int>
{
    public string Title { get; set; }
    public string Company { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
    public bool IsVisible { get; set; } = true;

}
