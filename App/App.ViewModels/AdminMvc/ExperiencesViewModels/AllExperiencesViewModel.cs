﻿namespace App.ViewModels.AdminMvc.ExperiencesViewModels;
public class AllExperiencesViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
}