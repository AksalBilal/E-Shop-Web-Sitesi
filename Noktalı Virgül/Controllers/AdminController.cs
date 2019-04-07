using Noktalı_Virgül.Models.VeriTabani;
using Noktalı_Virgül.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace Noktalı_Virgül.Controllers
{
    public class AdminController : Controller
    {
        HizliProjeEntities9 db = new HizliProjeEntities9();

        public ActionResult AnaSayfa()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length==2)
                {
                    DashBoardViewModel model = new DashBoardViewModel();
                    model = DashboardDoldur();
                    return View(model);
                }
            }
            return HttpNotFound();
        }
        public ActionResult KategoriIslemleri()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    var model = db.tblKitapTur.ToList();
                    return View(model);
                }
            }
            return HttpNotFound();
        }
        public ActionResult KategoriGuncelleme(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblKitapTur tur = new tblKitapTur();
                    if (id != 0)
                    {
                        tur = db.tblKitapTur.Find(id);
                    }
                    else
                    {
                        tur.TurID = 0;
                        tur.TurAdi = "";
                    }
                    return View(tur);
                }
            }
            return HttpNotFound();
           
        }
        public JsonResult Kaydet(int id,string ad)
        {
            List<tblKitapTur> turList = db.tblKitapTur.ToList();
            bool varMi = false;
            if (ad != "")
            {
                if (id != 0)
                {
                    foreach (var item in turList)
                    {
                        if (item.TurAdi.Equals(ad))
                        {
                            varMi = true;
                            break;
                        }
                    }
                    if (varMi == false)
                    {
                        tblKitapTur tur = db.tblKitapTur.Find(id);
                        tur.TurAdi = ad;
                        db.SaveChanges();
                        return Json("işlem tamam", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("var", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    foreach (var item in turList)
                    {
                        if (item.TurAdi.Equals(ad))
                        {
                            varMi = true;
                            break;
                        }
                    }
                    if (varMi == false)
                    {
                        tblKitapTur tur = new tblKitapTur();
                        tur.TurAdi = ad;
                        db.tblKitapTur.Add(tur);
                        db.SaveChanges();
                        return Json("işlem tamam", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("var", JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            else
            {
                return Json("bos", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult KategoriSil(int id)
        {
            tblKitap tblKitaplar = db.tblKitap.FirstOrDefault(x => x.TurID == id);
            tblKitapTur tblKitapTurleri = new tblKitapTur();
            if (tblKitaplar!=null)
            {
                return Json("var", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var promosyon = db.tblPromosyon.Where(x => x.KategoriId == id).ToList();
                if (promosyon != null)
                {
                    foreach (var item in promosyon)
                    {
                        db.tblPromosyon.Remove(item);
                        db.SaveChanges();
                    }
                }
                tblKitapTurleri = db.tblKitapTur.Find(id);
                db.tblKitapTur.Remove(tblKitapTurleri);
                db.SaveChanges();
                return Json("ıslem tamam", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UrunIslemleri()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    var model = db.tblKitap.Where(x=>x.SilindiMi==false).ToList();
                    return View(model);
                }
            }
            return HttpNotFound();
           
        }
        [HttpGet]
        public ActionResult UrunGuncelleme(AddProductViewModel model,int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblKitap kitap = new tblKitap();
                    if (id != 0)
                    {
                        kitap = db.tblKitap.Find(id);
                        model.tblKitap = db.tblKitap.Find(id);
                    }
                    else
                    {
                        kitap.KitapID = 0;
                        kitap.ISBNNo = "";
                        kitap.KapakResmi = "";
                        kitap.SayfaSayisi = 0;
                        kitap.Aciklama = "";
                        kitap.Adet = 0;
                        kitap.BasimYili = 0;
                        kitap.Fiyati = 0;
                        kitap.KitapAdi = "";
                        kitap.StokDurumu = false;
                        kitap.TurID = 0;
                        kitap.YayinEviID = 0;
                        kitap.Yazar = "";

                        model.tblKitap = kitap;
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunGuncelleme2(AddProductViewModel model, int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblKitap kitap = new tblKitap();
                    if (id != 0)
                    {
                        model.tblKitap = db.tblKitap.Find(id);
                    }
                    if (!ModelState.IsValid)
                    {
                        return View("UrunGuncelleme", model);
                    }
                    else if (id != 0)
                    {
                        tblKitap kitapguncellenecek = db.tblKitap.Find(id);
                        try
                        {
                            kitapguncellenecek.Aciklama = model.Aciklama;
                            kitapguncellenecek.Adet = Convert.ToInt32(model.Adet);
                            kitapguncellenecek.BasimYili = Convert.ToInt32(model.BasımYili);

                            var para = model.Fiyatı.Split('.');
                            if (para.Length == 2)
                            {

                                model.Fiyatı = para[0] + "," + para[1];
                            }

                            kitapguncellenecek.Fiyati = Convert.ToDecimal(model.Fiyatı);
                            kitapguncellenecek.ISBNNo = model.ISBNno;
                            kitapguncellenecek.KitapAdi = model.KitapAdi;
                            kitapguncellenecek.SayfaSayisi = Convert.ToInt32(model.SayfaSayısı);
                            if (Convert.ToInt32(model.Adet) > 0)
                            {
                                kitapguncellenecek.StokDurumu = true;
                            }
                            else
                            {
                                kitapguncellenecek.StokDurumu = false;
                            }
                            tblKitapTur tur = db.tblKitapTur.FirstOrDefault(x => x.TurAdi == model.TurAdi);
                            if (tur != null)
                            {
                                kitapguncellenecek.TurID = tur.TurID;
                            }
                            tblYayinEvi yayinEvi = db.tblYayinEvi.FirstOrDefault(x => x.YayinEviAdi == model.YayinEvi);
                            if (yayinEvi != null)
                            {
                                kitapguncellenecek.YayinEviID = yayinEvi.YayinEviID;
                            }
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            ViewBag.Mesaj = 1;
                        }
                    }
                    return RedirectToAction("UrunIslemleri", "Admin");
                }
            }
            return HttpNotFound();
           
        }
        public ActionResult UrunEkle(AddProductViewModel model, HttpPostedFileBase file)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblKitap kitap = new tblKitap();
                    if (!ModelState.IsValid)
                    {

                        kitap.KitapID = 0;
                        kitap.ISBNNo = "";
                        kitap.KapakResmi = "";
                        kitap.SayfaSayisi = 0;
                        kitap.Aciklama = "";
                        kitap.Adet = 0;
                        kitap.BasimYili = 0;
                        kitap.Fiyati = 0;
                        kitap.KitapAdi = "";
                        kitap.StokDurumu = false;
                        kitap.TurID = 0;
                        kitap.YayinEviID = 0;
                        kitap.Yazar = "";
                        
                        model.tblKitap = kitap;
                        return View("UrunGuncelleme", model);
                    }
                    tblKitap kitap2 = db.tblKitap.FirstOrDefault(x => x.ISBNNo == model.ISBNno);
                    try
                    {
                        if (kitap2 != null)
                        {
                            var yenıkıtap = db.tblKitap.Find(kitap2.KitapID);
                            if (yenıkıtap.SilindiMi == true)
                            {
                                yenıkıtap.Adet += Convert.ToInt32(model.Adet);
                                yenıkıtap.SilindiMi = false;
                                yenıkıtap.StokDurumu = true;
                                db.SaveChanges();
                                ViewBag.Mesaj = 2;
                            }
                            else
                            {
                                yenıkıtap.Adet += Convert.ToInt32(model.Adet);
                                db.SaveChanges();
                                ViewBag.Mesaj = 2;
                            }
                        }
                        else
                        {
                            tblKitap yeni = new tblKitap();
                            yeni.Aciklama = model.Aciklama;
                            yeni.Adet = Convert.ToInt32(model.SayfaSayısı);
                            yeni.BasimYili = Convert.ToInt32(model.BasımYili);

                            var para = model.Fiyatı.Split('.');
                            if (para.Length == 2)
                            {

                                model.Fiyatı = para[0] + "," + para[1];
                            }

                            yeni.Fiyati = Convert.ToDecimal(model.Fiyatı);
                            yeni.ISBNNo = model.ISBNno;
                            yeni.KitapAdi = model.KitapAdi;
                            yeni.SayfaSayisi = Convert.ToInt32(model.SayfaSayısı);
                            if (Convert.ToInt32(model.Adet) > 0)
                            {
                                yeni.StokDurumu = true;
                            }
                            else
                                yeni.StokDurumu = false;
                            yeni.Yazar = model.Yazar;
                            tblKitapTur tur = db.tblKitapTur.FirstOrDefault(x => x.TurAdi == model.TurAdi);
                            if (tur != null)
                            {
                                yeni.TurID = tur.TurID;
                            }
                            tblYayinEvi yayinEvi = db.tblYayinEvi.FirstOrDefault(x => x.YayinEviAdi == model.YayinEvi);
                            if (yayinEvi != null)
                            {
                                yeni.YayinEviID = yayinEvi.YayinEviID;
                            }
                            if (file != null)
                            {
                                string pic = System.IO.Path.GetFileName(file.FileName);
                                string path = System.IO.Path.Combine(
                                                       Server.MapPath("~/Content/images/kitaplar"), pic);
                                file.SaveAs(path);
                                yeni.KapakResmi = pic;
                            }
                            yeni.SilindiMi = false;
                            db.tblKitap.Add(yeni);
                            db.SaveChanges();
                        }
                        return RedirectToAction("UrunIslemleri");
                    }
                    catch (Exception)
                    {
                        ViewBag.Mesaj = 3;
                        return View("UrunGuncelleme", model);
                    }
                }
            }
            return HttpNotFound();
           
            
        }
        public ActionResult UrunSil(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    var sepet = db.tblSepet.Where(x => x.UrunId == id).ToList();
                    var favori = db.tblFavoriler.Where(x => x.UrunID == id).ToList();
                    var promosyon = db.tblPromosyon.Where(x => x.UrunId == id).ToList();
                    if (sepet != null)
                    {
                        foreach (var item in sepet)
                        {
                            db.tblSepet.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    if (favori != null)
                    {
                        foreach (var item in favori)
                        {
                            db.tblFavoriler.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    if (promosyon != null)
                    {
                        foreach (var item in promosyon)
                        {
                            db.tblPromosyon.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    tblKitap kitap = db.tblKitap.Find(id);
                    kitap.SilindiMi = true;
                    kitap.Adet = 0;
                    kitap.StokDurumu = false;
                    db.SaveChanges();
                    return RedirectToAction("UrunIslemleri");
                }
            }
            return HttpNotFound();
            
        }
        public DashBoardViewModel DashboardDoldur()
        {
            DashBoardViewModel dashboard = new DashBoardViewModel();
            var toplamsatis = db.ToplamKitapSatis();
            foreach (var toplam in toplamsatis)
            {

                dashboard.ToplamUrunSatisSayisi = (int)toplam;
            }

            DateTime dun = DateTime.Now.AddDays(-1);
            List<tblSatinAlinanlar> satilanList = db.tblSatinAlinanlar.Where(x => x.SatinAlmaTarihi > dun).ToList();
            foreach (var item in satilanList)
            {
                dashboard.ToplamUrunSatisSayisiGunluk += (int)item.SatinAlmaAdedi;
            }

            DateTime hafta = DateTime.Now.AddDays(-7);
            List<tblSatinAlinanlar> satilanListHafta = db.tblSatinAlinanlar.Where(x => x.SatinAlmaTarihi > hafta).ToList();
            foreach (var item in satilanListHafta)
            {
                dashboard.ToplamUrunSatisSayisiHaftalik += (int)item.SatinAlmaAdedi;
            }

            dashboard.encoksatisyapilanurunler = db.Encoksatisyapilanurunler().ToList();
            dashboard.encoksepeteeklenenurunler = db.EnCokSepeteEklenenUrunler().ToList();
            dashboard.EnCokSatisYapanKategori = db.EnCokSatisYapanKategori().ToList();
            var toplamuye = db.ToplamKullaniciKaydi();
            foreach (var uye in toplamuye)
            {
                dashboard.ToplamUyeKayitSayisi = (int)uye;
            }
            List<tblKullanici> kullanicisGunluk = db.tblKullanici.Where(x => x.KayitOlmaTarihi > dun).ToList();
            dashboard.ToplamUyeKayitSayisiGunluk = kullanicisGunluk.Count();

            List<tblKullanici> kullanicisHaftalik = db.tblKullanici.Where(x => x.KayitOlmaTarihi > hafta).ToList();
            dashboard.ToplamUyeKayitSayisiHaftalik = kullanicisHaftalik.Count();

            return dashboard;
        }
        public ActionResult MesajAl()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    MessageViewModel messages = new MessageViewModel();
                    messages.Mesajlar = db.tblMesaj.OrderBy(x => x.OkunduMu == true).ToList();
                    return View(messages);
                }
            }
            return HttpNotFound();
            
        }
        public ActionResult MesajSil(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblMesaj mesaj = db.tblMesaj.Find(id);
                    db.tblMesaj.Remove(mesaj);
                    db.SaveChanges();
                    return RedirectToAction("MesajAl", "Admin");
                }
            }
            return HttpNotFound();
            
        }
        public ActionResult BilgiDegistir(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {

                    tblMesaj mesaj = db.tblMesaj.Find(id);
                    if (mesaj.OkunduMu == true)
                        mesaj.OkunduMu = false;
                    else
                        mesaj.OkunduMu = true;

                    db.SaveChanges();
                    return RedirectToAction("MesajAl", "Admin");
                }
            }
            return HttpNotFound();
        }
        public ActionResult MesajGoster(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblMesaj mesaj = db.tblMesaj.Find(id);
                    mesaj.OkunduMu = true;
                    db.SaveChanges();
                    ViewBag.mesaj = mesaj;
                    return View();
                }
            }
            return HttpNotFound();
            
        }
        public ActionResult TopluPromosyonIslemleri()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    var promosyonlar = db.tblPromosyon.Where(x => x.TumUrunler == true).ToList();
                    ViewBag.promosyonlar = promosyonlar;
                    return View();
                }
            }
            return HttpNotFound();
            
        }
        public ActionResult PromosyonEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    ViewBag.id = id;
                    return View();
                }
            }
            return HttpNotFound();

            
        }
        public ActionResult PromosyonEkle2(int id,int promosyon,string str)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {

                    if (id == 0)
                    {
                        tblPromosyon tblPromosyon = new tblPromosyon();
                        tblPromosyon.TumUrunler = true;
                        tblPromosyon.IndirimOrani = promosyon;
                        db.tblPromosyon.Add(tblPromosyon);
                        db.SaveChanges();
                        return RedirectToAction("AnaSayfa", "Admin");
                    }
                    else if (id == 1)
                    {
                        tblKitapTur tur = db.tblKitapTur.FirstOrDefault(x => x.TurAdi == str);
                        if (tur != null)
                        {
                            tblPromosyon tblPromosyon = new tblPromosyon();
                            tblPromosyon.TumUrunler = false;
                            tblPromosyon.IndirimOrani = promosyon;
                            tblPromosyon.KategoriId = tur.TurID;
                            db.tblPromosyon.Add(tblPromosyon);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Mesaj = 1;
                        }

                    }
                    else if (id == 2)
                    {
                        tblKitap kitap = db.tblKitap.FirstOrDefault(x => x.ISBNNo == str);
                        if (kitap != null)
                        {
                            tblPromosyon tblPromosyon = new tblPromosyon();
                            tblPromosyon.TumUrunler = false;
                            tblPromosyon.IndirimOrani = promosyon;
                            tblPromosyon.UrunId = kitap.KitapID;
                            db.tblPromosyon.Add(tblPromosyon);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Mesaj = 2;
                        }

                    }
                    return HttpNotFound();
                }
            }
            return HttpNotFound();

        }
        public ActionResult PromosyonSil(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    tblPromosyon promosyon = db.tblPromosyon.Find(id);
                    db.tblPromosyon.Remove(promosyon);
                    db.SaveChanges();
                    return RedirectToAction("AnaSayfa", "Admin");
                }
            }
            return HttpNotFound();

            
        }
        public ActionResult KategoriPromosyonIslemleri()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    var promosyonlar = db.tblPromosyon.Where(x => x.TumUrunler == false && x.UrunId == null).ToList();
                    ViewBag.promosyonlar = promosyonlar;
                    return View();
                }
            }
            return HttpNotFound();

            
        }
        public ActionResult UrunBazliPromosyonIslemleri()
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                if (kullaniciadi.Length == 2)
                {
                    var promosyonlar = db.tblPromosyon.Where(x => x.TumUrunler == false && x.KategoriId == null).ToList();
                    ViewBag.promosyonlar = promosyonlar;
                    return View();
                }
            }
            return HttpNotFound();
        }
    }
}