using App.Core.Entities;
namespace App.Data.Entities;
public class EducationEntity : BaseEntity<int>
{
    public string Degree { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}
