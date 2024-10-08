using App.Core.Entities;
namespace App.Data.Entities;

public class AboutMeEntity : BaseEntity<int>
{
    public string Introduction { get; set; } = string.Empty;
    public string ImageUrl1 { get; set; } = string.Empty;
    public string ImageUrl2 { get; set;} = string.Empty;

}
