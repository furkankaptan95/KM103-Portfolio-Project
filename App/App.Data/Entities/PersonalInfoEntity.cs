using App.Core.Entities;
namespace App.Data.Entities;
public class PersonalInfoEntity : BaseEntity<int>
{
    public string About { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public string Link { get; set; }

}
