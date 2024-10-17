using System.ComponentModel.DataAnnotations;


namespace App.ViewModels.AdminMvc.Validation;
public class EndDateAfterStartDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (dynamic)validationContext.ObjectInstance;

        // Eğer EndDate null değilse ve başlangıç tarihinden önce ise hata ver
        if (model.EndDate is not null && model.EndDate < model.StartDate&&model.StartDate is not null)
        {
            return new ValidationResult("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
        }

        return ValidationResult.Success; // Geçerli ise başarı döndür
    }
}
