﻿using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.AdminMvc.AboutMeViewModels;
public class UpdateAboutMeViewModel
{
    [Required(ErrorMessage = "Giriş kısmı zorunludur.")]
    [MinLength(10, ErrorMessage = "Giriş kısmı en az 10 karakter olmalıdır.")]
    public string Introduction { get; set; } = string.Empty;
}