using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.PersonalInfoViewModels;
public class PersonalInfoViewModel
{
    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string About { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}
