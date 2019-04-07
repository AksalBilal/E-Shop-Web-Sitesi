using Noktalı_Virgül.Models.VeriTabani;
using Noktalı_Virgül.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Noktalı_Virgül.Controllers;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Noktalı_Virgül.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        HizliProjeEntities9 db = new HizliProjeEntities9();
        public int sepetinId;
      
      //  GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel kullanici)
        {
            if (!ModelState.IsValid)
            {
                return View(kullanici);
            }

            string sifre = Sifrele(kullanici.Password);

            var kullaniciDB = db.tblKullanici.FirstOrDefault(x => x.Mail == kullanici.Email && x.Sifre == sifre);

            if (kullaniciDB != null)
            {

                FormsAuthentication.SetAuthCookie($"{kullaniciDB.Ad} {kullaniciDB.Soyad} {kullaniciDB.Mail} {kullaniciDB.KullaniciID}", true);
                var kullaniciadi = User.Identity.Name.Split(' ');

                string macadres = UrunController.MACAdresiAl();
                tblZiyaretci ziyaretci2 = db.tblZiyaretci.FirstOrDefault(x => x.ZiyaretciMAC == macadres);

                if (ziyaretci2 != null)
                {

                    int ZiyaretcininId = ziyaretci2.ZiyaretciID;

                    HomeViewModel model = new HomeViewModel();
                    model.sepet = db.tblSepet.Where(x => x.ZiyaretciId == ZiyaretcininId).ToList();
                    IList<tblSepet> sepet2 = db.tblSepet.Where(x => x.KullaniciId == kullaniciDB.KullaniciID).ToList();
                    bool onay = true;
                    if (model.sepet != null)
                    {
                        foreach (var item2 in sepet2)
                        {
                            onay = true;
                            tblSepet guncelsepet = db.tblSepet.Find(item2.SepetId);
                            foreach (var item in model.sepet)
                            {
                                tblSepet temp = db.tblSepet.FirstOrDefault(x => x.UrunId == item.UrunId && x.KullaniciId == item2.KullaniciId);
                                if (temp == null)
                                {
                                    guncelsepet.KullaniciId = kullaniciDB.KullaniciID;
                                    guncelsepet.ZiyaretciId = null;
                                    guncelsepet.SepettekiAdet = item.SepettekiAdet;
                                    guncelsepet.UrunId = item.UrunId;
                                    db.tblSepet.Add(guncelsepet);
                                    db.SaveChanges();
                                }
                                else if (item2.UrunId == item.UrunId)
                                {
                                    guncelsepet.SepettekiAdet = item2.SepettekiAdet + item.SepettekiAdet;
                                    db.SaveChanges();
                                    onay = false;

                                }
                            }
                        }
                        if (sepet2.Count == 0)
                        {
                            foreach (var item in model.sepet)
                            {
                                tblSepet yeniSepet = db.tblSepet.Find(item.SepetId);
                                yeniSepet.KullaniciId = kullaniciDB.KullaniciID;
                                yeniSepet.ZiyaretciId = null;
                                db.SaveChanges();
                            }
                        }
                        foreach (var item in model.sepet)
                        {
                            tblSepet silinecekSepet = db.tblSepet.Find(item.SepetId);
                            if (silinecekSepet.KullaniciId == null)
                            {
                                db.tblSepet.Remove(silinecekSepet);
                                db.SaveChanges();
                            }
                        }

                    }


                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
                var adminDB = db.tblAdmin.FirstOrDefault(x => x.Mail == kullanici.Email && x.Sifre == sifre);

                if (adminDB != null)
                {
                    FormsAuthentication.SetAuthCookie($"{adminDB.Ad} {adminDB.Soyad}", true);
                    return RedirectToAction("Anasayfa", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "EMail veya şifre hatalı!");
                    return View();
                }
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult AddUser()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(AddUserViewModel Kullanici)
        {
            
            if (!ModelState.IsValid)
            {
                return View(Kullanici);
            }
            string sifre2 = Sifrele(Kullanici.Password);
            tblKullanici Eklenenkullanici = new tblKullanici
            {
                Ad = Kullanici.Ad,
                Soyad = Kullanici.Soyad,
                Mail = Kullanici.Email,
                Sifre = sifre2,
            };
            db.tblKullanici.Add(Eklenenkullanici);
            db.SaveChanges();
            FormsAuthentication.SetAuthCookie($"{Kullanici.Ad} {Kullanici.Soyad} {Kullanici.Email} {Eklenenkullanici.KullaniciID}", true);

            string macadres = UrunController.MACAdresiAl();
            tblZiyaretci ziyaretci2 = db.tblZiyaretci.FirstOrDefault(x => x.ZiyaretciMAC == macadres);

            if (ziyaretci2 != null)
            {

                int ZiyaretcininId = ziyaretci2.ZiyaretciID;

                HomeViewModel model = new HomeViewModel();
                model.sepet = db.tblSepet.Where(x => x.ZiyaretciId == ZiyaretcininId).ToList();
                if (model.sepet != null)
                {
                    foreach (var item in model.sepet)
                    {
                        tblSepet guncelsepet = db.tblSepet.Find(item.SepetId);
                        guncelsepet.KullaniciId = Eklenenkullanici.KullaniciID;
                        guncelsepet.ZiyaretciId = null;
                        db.SaveChanges();
                    }

                }


            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifremiUnuttum(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                tblKullanici kullanici = db.tblKullanici.FirstOrDefault(x => x.Mail == model.Email);
                string sifre = SifreCoz(kullanici.Sifre);
                if (kullanici != null)
                {
                    var body = new StringBuilder();
                    body.AppendLine(value: "Şifreniz: " +sifre);
                    
                    MailSender(body.ToString(),model.Email);
                    ViewBag.Success = true;
                    return View();
                }
            }
            return View();
        }
        public static void MailSender(string body,string aliciMail)
        {
            var fromAddress = new MailAddress("noktalivirgul45.2@gmail.com");//gönderici maili
            var fromPassword = "Noktali123";
            var toAddress = new MailAddress(aliciMail);//alıcı maili
            const string subject = "Magic Books | Şifre Hatırlatma";
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

        public bool SepetKontrol(tblSepet sepet)
        {
            tblSepet sepet2 = db.tblSepet.FirstOrDefault(x => x.ZiyaretciId == sepet.ZiyaretciId);

            if (sepet2 != null)
            {
                sepetinId = sepet2.SepetId;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Sifrele(string data)
        {
            byte[] tempDizi = System.Text.ASCIIEncoding.ASCII.GetBytes(data);// şifrelenecek veri byte dizisine çevrilir
            string finalData = System.Convert.ToBase64String(tempDizi);//Base64 ile şifrelenir
            return finalData;
        }
        public static string SifreCoz(string data)
        {
            byte[] tempDizi = System.Convert.FromBase64String(data);
            string finalData = System.Text.ASCIIEncoding.ASCII.GetString(tempDizi);
            return finalData;
        }

    }

}
