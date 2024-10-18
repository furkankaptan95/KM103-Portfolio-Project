namespace App.DTOs.ExperienceDtos.Portfolio;
public class AllExperiencesPortfolioDto
{
    public string Title { get; set; }
    public string Company { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
}
