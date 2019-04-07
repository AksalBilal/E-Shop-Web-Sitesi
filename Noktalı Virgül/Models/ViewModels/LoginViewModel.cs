using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "E-mail Adresi")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir {0} giriniz")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [StringLength(16, ErrorMessage = "{0} en az {2} en fazla {1} olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}