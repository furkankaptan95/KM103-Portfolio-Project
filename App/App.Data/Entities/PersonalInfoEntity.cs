using App.Core.Entities;
namespace App.Data.Entities;
public class PersonalInfoEntity : BaseEntity<int>
{
    public string About { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

}
