using App.Core.Entities;
namespace App.Data.Entities;

public class AboutMeEntity : BaseEntity<int>
{
    public string Introduction { get; set; }
    public string FullName { get; set; }
    public string Field { get; set; }
    public string ImageUrl1 { get; set; }
    public string ImageUrl2 { get; set;}

}
