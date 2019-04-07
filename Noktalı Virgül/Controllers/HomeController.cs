using Noktalı_Virgül.Models.VeriTabani;
using Noktalı_Virgül.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Security;

namespace Noktalı_Virgül.Controllers
{
    public class HomeController : Controller
    {
        HizliProjeEntities9 db = new HizliProjeEntities9();

        public static  int ajaxgelenveri=3;
        // GET: Home
        public ActionResult Index(int Page=1)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    return RedirectToAction("AnaSayfa","Admin");
                }
            }
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                Promosyonlar = db.tblPromosyon.ToList(),
                deneme = db.tblKitap.Where(x => x.SilindiMi == false).OrderByDescending(x => x.KitapID).ToPagedList(Page, 9)

            };
           
            return View(model);
        }
        public ActionResult Tur(int id, int Page = 1)
        { 
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                deneme = db.tblKitap.Where(x => x.TurID == x.tblKitapTur.TurID && x.TurID == id && x.SilindiMi==false).OrderByDescending(x => x.KitapID).ToPagedList(Page, 9)
            };
            ViewBag.ID = id;
            
            return View(model);
        }

        public ActionResult YayinEvi(int id,int Page = 1)
        {
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                deneme = db.tblKitap.Where(x => x.YayinEviID == x.tblYayinEvi.YayinEviID && x.YayinEviID == id && x.SilindiMi==false).OrderByDescending(x => x.KitapID).ToPagedList(Page, 9)
            };
            ViewBag.ID = id;
            
            return View(model);
        }

        public ActionResult Fiyat(int id,int Page = 1)
        {
            decimal deger = 0, deger1 = 0;
            switch (id)
            {
                case 1:
                    deger = 0;
                    deger1 = 25;
                    break;
                case 2:
                    deger = 25;
                    deger1 = 50;
                    break;
                case 3:
                    deger = 50;
                    deger1 = 100;
                    break;
                case 4:
                    deger = 100;
                    deger1 = 250;
                    break;
                case 5:
                    deger = 250;
                    deger1 = 500;
                    break;
                case 6:
                    deger = 500;
                    deger1= 2147483648;
                    break;
            }
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                deneme = db.tblKitap.Where(x => x.Fiyati >= deger && x.Fiyati <= deger1 && x.SilindiMi==false).OrderByDescending(x => x.KitapID).ToPagedList(Page, 9)
            };
            ViewBag.deger = deger;
         
            return View(model);
        }
      
        [HttpGet]
        public ActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Iletisim(ContactViewModel model)
        {
            tblMesaj gonderilenMesaj = new tblMesaj
            {
                AdSoyad = model.AdSoyad,
                Email = model.Email,
                Konu = model.Konu,
                Mesaj = model.Mesaj,
                OkunduMu = false
                
            };
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                    var body = new StringBuilder();
                    body.AppendLine("E-Mail: " + model.Email);
                    body.AppendLine("Konu: " + model.Konu);
                    body.AppendLine(value: "Mesaj: " + model.Mesaj);
                if (User.Identity.IsAuthenticated)
                {
                    gonderilenMesaj.GonderciTipi = true;
                    var kullaniciadi = User.Identity.Name.Split(' ');
                    body.AppendLine("Gönderici Ad Soyad :"+ kullaniciadi[0]+"  "+kullaniciadi[1]);
                    body.AppendLine("Göndericinin Siteye Kayıtlı olduğu mail : "+ kullaniciadi[2]);
                }
                else
                {
                    gonderilenMesaj.GonderciTipi = false;
                    body.AppendLine("Gönderici tipi:Ziyaretci");
                    body.AppendLine("Ziyaretcinin Adı ve Soyadı :"+model.AdSoyad);
                }
                db.tblMesaj.Add(gonderilenMesaj);
                db.SaveChanges();
                MailSender(body.ToString());
                    ViewBag.Success = true;
                return View();
            }
            
        }
        public static void MailSender(string body)
        {
            var fromAddress = new MailAddress("noktalivirgul45.2@gmail.com");//gönderici maili
            var fromPassword = "Noktali123";
            var toAddress = new MailAddress("noktalivirgul45@gmail.com");//alıcı maili
            const string subject = "Magic Books | Sitenizden Gelen İletişim Formu";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }
        }

        public static void urlKontol(string urls,string sayi,int sayfaBoyutu)
        {
            ajaxgelenveri = sayfaBoyutu;
        }
    }
}