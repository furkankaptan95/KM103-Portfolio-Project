namespace App.ViewModels.AdminMvc.EducationsViewModels;
public class AllEducationsViewModel
{
    public int Id { get; set; }
    public string Degree { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsVisible { get; set; }
}
