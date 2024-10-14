using App.Core.Entities;
namespace App.Data.Entities;
public class EducationEntity : BaseEntity<int>
{
    public string Degree { get; set; }
    public string School { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsVisible { get; set; } = true;
}
