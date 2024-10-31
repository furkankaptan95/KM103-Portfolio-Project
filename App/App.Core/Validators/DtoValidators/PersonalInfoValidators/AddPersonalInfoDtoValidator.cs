using App.DTOs.PersonalInfoDtos.Admin;
using FluentValidation;

namespace App.Core.Validators.DtoValidators.PersonalInfoValidators;
public class AddPersonalInfoDtoValidator : AbstractValidator<AddPersonalInfoDto>
{
    public AddPersonalInfoDtoValidator()
    {
        RuleFor(x => x.About)
          .NotEmpty().WithMessage("Hakkımda kısmı boş olamaz.")
          .MaximumLength(300).WithMessage("Hakkımda kısmı maksimum 300 karakter olabilir.");

        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("İsim kısmı boş olamaz.")
           .MaximumLength(50).WithMessage("İsim maksimum 50 karakter olabilir.");

        RuleFor(x => x.Surname)
           .NotEmpty().WithMessage("Soyisim kısmı boş olamaz.")
           .MaximumLength(50).WithMessage("Soyisim maksimum 50 karakter olabilir.");

        RuleFor(x => x.Adress)
           .NotEmpty().WithMessage("Adres kısmı boş olamaz.")
          .MaximumLength(50).WithMessage("Adres maksimum 50 karakter olabilir.");


        RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email kısmı boş olamaz.")
           .MaximumLength(100).WithMessage("Email maksimum 100 karakter olabilir.")
           .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin.");
        RuleFor(x => x.Link)
                   .NotEmpty().WithMessage("Link kısmı boş olamaz.")
                   .MaximumLength(255).WithMessage("Link maksimum 255 karakter olabilir.");

        RuleFor(x => x.BirthDate)
           .NotEmpty().WithMessage("Doğum tarihi gerekli.")
           .Must(BeAValidDate).WithMessage("Geçerli bir tarih giriniz.");
    }
    private bool BeAValidDate(DateTime date)
    {
        return date != default;
    }
}
