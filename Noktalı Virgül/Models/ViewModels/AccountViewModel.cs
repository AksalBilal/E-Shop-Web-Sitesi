using Noktalı_Virgül.Models.VeriTabani;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class AccountViewModel
    {
        public tblKullanici Kullanici { get; set; }

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string Soyad { get; set; }

        [Display(Name = "E-mail Adresi")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir {0} giriniz")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} olmalıdır.")]
        public string Email { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        public string Adres { get; set; }
    }
}