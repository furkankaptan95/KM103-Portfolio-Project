namespace App.ViewModels.AdminMvc.ProjectsViewModels;
public class AllProjectsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsVisible { get; set; }
}
