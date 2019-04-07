using System.ComponentModel.DataAnnotations;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class AddUserViewModel
    {


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

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [StringLength(16, ErrorMessage = "{0} en fazla {1} olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Sifre Dogrulama")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [StringLength(16, ErrorMessage = "{0} en fazla {1} olmalıdır.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Sifre ile aynı girmek zorundasınız")]
        [DataType(DataType.Password)]
        public string SifreDogrulama { get; set; }



    }
}