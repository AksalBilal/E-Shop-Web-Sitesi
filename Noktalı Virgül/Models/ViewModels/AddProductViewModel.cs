using Noktalı_Virgül.Models.VeriTabani;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class AddProductViewModel
    {
        public tblKitap tblKitap { get; set; }

        [Display(Name = "ISBN No")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(13, ErrorMessage = "{0} {2} haneli olmalıdır.", MinimumLength = 13)]
        public string ISBNno { get; set; }

        [Display(Name = "Kitap Adı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string KitapAdi { get; set; }

        [Display(Name = "Yayın Evi")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string YayinEvi { get; set; }

        [Display(Name = "Yazar")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string Yazar { get; set; }

        [Display(Name = "Sayfa Sayısı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string SayfaSayısı { get; set; }
        
        [Display(Name = "Basım Yılı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(4, ErrorMessage = "{0} {2} haneli olmalıdır.", MinimumLength = 4)]
        public string BasımYili { get; set; }

        [Display(Name = "Fiyatı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string Fiyatı { get; set; }

        [Display(Name = "Adet")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string Adet { get; set; }
        
        [Display(Name = "Tur Adı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0} en fazla {1} haneli olmalıdır.")]
        public string TurAdi { get; set; }

        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        public string Aciklama { get; set; }





    }
}