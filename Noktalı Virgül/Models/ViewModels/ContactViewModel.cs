using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "AdSoyad")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string AdSoyad { get; set; }

        [Display(Name = "E-mail Adresi")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir {0} giriniz")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string Email { get; set; }

        [Display(Name = "Konu")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string Konu { get; set; }

        [Display(Name = "Mesaj")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        public string Mesaj { get; set; }

    }
}