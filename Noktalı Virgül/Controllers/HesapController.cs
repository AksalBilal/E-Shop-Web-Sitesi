using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Noktalı_Virgül.Models.VeriTabani;
using Noktalı_Virgül.Models.ViewModels;
namespace Noktalı_Virgül.Controllers
{
    public class HesapController : Controller
    {
        HizliProjeEntities9 db = new HizliProjeEntities9();
        string EskiSifre = "";
        [HttpGet]
        public ActionResult SifreDegistir()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifreDegistir(PasswordVerification model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            var kullaniciadi = User.Identity.Name.Split(' ');
            int kullaniciId = Convert.ToInt32(kullaniciadi[3]);

            tblKullanici kullanici = db.tblKullanici.FirstOrDefault(x => x.KullaniciID == kullaniciId);
            EskiSifre = Sifrele(model.EskiSifre);
            if (kullanici.Sifre==EskiSifre)
            {
                if (model.EskiSifre!=model.Password)
                {
                    tblKullanici guncelkullanici = db.tblKullanici.Find(kullaniciId);
                    guncelkullanici.Sifre = Sifrele(model.Password);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Mesaj = "Eski Şifre ile Yeni Şifre aynı olamaz";
                    return View(model);
                }
            }
            else
            {
                ViewBag.Mesaj = "Lütfen Şifrenizi Doğru giriniz!";
                return View(model);
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SiparisGoruntule()
        {

            var kullaniciadi = User.Identity.Name.Split(' ');
            int kullaniciId = Convert.ToInt32(kullaniciadi[3]);

            IList<tblSatinAlinanlar> Sonsatinalinanlar = db.tblSatinAlinanlar.Where(x => x.MusteriID == kullaniciId).OrderByDescending(x=>x.SatinAlmaTarihi).ToList();
            ViewBag.Sonsatinalinanlar = Sonsatinalinanlar;
            return View();
        }
       [HttpGet]
        public ActionResult KisiselBilgiler()
        {
            AccountViewModel model = new AccountViewModel();
            var kullaniciadi = User.Identity.Name.Split(' ');
            int kullaniciId = Convert.ToInt32(kullaniciadi[3]);

            model.Kullanici = db.tblKullanici.FirstOrDefault(x => x.KullaniciID == kullaniciId);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KisiselBilgiler(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var kullaniciadi2 = User.Identity.Name.Split(' ');
                int kullaniciId2 = Convert.ToInt32(kullaniciadi2[3]);
                model.Kullanici = db.tblKullanici.FirstOrDefault(x => x.KullaniciID == kullaniciId2);
                return View(model);
            }
            var kullaniciadi = User.Identity.Name.Split(' ');
            int kullaniciId = Convert.ToInt32(kullaniciadi[3]);
            tblKullanici GuncellenecekKullanici = db.tblKullanici.Find(kullaniciId);
            GuncellenecekKullanici.Ad = model.Ad;
            GuncellenecekKullanici.Soyad = model.Soyad;
            GuncellenecekKullanici.Mail = model.Email;
            GuncellenecekKullanici.Adres = model.Adres;
            db.SaveChanges();
            FormsAuthentication.SetAuthCookie($"{model.Ad} {model.Soyad} {model.Email} {kullaniciId}", true);

            

            model.Kullanici = db.tblKullanici.FirstOrDefault(x => x.KullaniciID == kullaniciId);
            return RedirectToAction("Index","Home");
        }

        public static string Sifrele(string data)
        {
            byte[] tempDizi = System.Text.ASCIIEncoding.ASCII.GetBytes(data);// şifrelenecek veri byte dizisine çevrilir
            string finalData = System.Convert.ToBase64String(tempDizi);//Base64 ile şifrelenir
            return finalData;
        }

    }
}