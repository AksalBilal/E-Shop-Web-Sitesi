using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class PasswordVerification
    {
        [Display(Name = "Eski Şifre")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [StringLength(16, ErrorMessage = "{0} en az {2} en fazla {1} olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string EskiSifre { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [StringLength(16, ErrorMessage = "{0} en az {2} en fazla {1} olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Sifre Dogrulama")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [StringLength(16, ErrorMessage = "{0} en az {2} en fazla {1} olmalıdır.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Sifre ile aynı girmek zorundasınız")]
        [DataType(DataType.Password)]
        public string SifreDogrulama { get; set; }
    }
}