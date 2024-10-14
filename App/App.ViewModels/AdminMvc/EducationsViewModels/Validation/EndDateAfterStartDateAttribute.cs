using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels.AdminMvc.EducationsViewModels.Validation;
public class EndDateAfterStartDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (dynamic)validationContext.ObjectInstance;

        // Eğer EndDate null değilse ve başlangıç tarihinden önce ise hata ver
        if (model.EndDate is not null && model.EndDate < model.StartDate)
        {
            return new ValidationResult("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
        }

        return ValidationResult.Success; // Geçerli ise başarı döndür
    }
}
