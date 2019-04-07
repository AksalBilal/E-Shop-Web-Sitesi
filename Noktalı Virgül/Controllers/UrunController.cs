using Noktalı_Virgül.Models;
using Noktalı_Virgül.Models.VeriTabani;
using Noktalı_Virgül.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
namespace Noktalı_Virgül.Controllers
{
    public class UrunController : Controller
    {
        HizliProjeEntities9 db = new HizliProjeEntities9();
        tblKullanici kullanici;
        int sepetAdet = 0, sepetId = 0;
       
        public static int ajaxverisi = 3;
        // GET: Urun
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UrunArama(string q, int Page=1)
        {
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                deneme = db.tblKitap.Where(x => x.SilindiMi == false).OrderByDescending(x => x.KitapID).ToPagedList(Page, 9)
            };
            if (!q.Equals("")&&!q.Equals(" "))
            {
                var urun = db.tblKitap.Where(x =>x.SilindiMi==false && (x.KitapAdi.Contains(q) || x.Yazar.Contains(q)));

                ViewBag.urun = urun.ToList();

            }
            return View(model);
        }
        public ActionResult UrunDetay(int id, int Page=1)
        {
            ViewBag.yayinEvi = db.tblYayinEvi.ToList();
            ViewBag.turList = db.tblKitapTur.ToList();
            ViewBag.kitap = db.tblKitap.Find(id);
            ViewBag.favoriList = db.tblFavoriler.ToList();
            ViewBag.kullanici = kullanici;
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                ister = db.ister(id).ToList()
            };
            return View(model);
        }
        public ActionResult Sepet()
        {
            var model = new HomeViewModel
            {
                YayinEvleri = db.tblYayinEvi.ToList(),
                KitapTurleri = db.tblKitapTur.ToList(),
                Kitaplar = db.tblKitap.Where(x => x.SilindiMi == false).ToList(),
                Favoriler = db.tblFavoriler.ToList(),
                Promosyonlar=db.tblPromosyon.ToList(),
                deneme = null,

            };
            if(User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name.Split(' ');
                int kullaniciId = Convert.ToInt32(kullaniciadi[3]);
                model.sepet = db.tblSepet.Where(x => x.KullaniciId == kullaniciId).ToList();

            }
            else
            {

                string macadres = MACAdresiAl();
                tblZiyaretci ziyaretci2 = db.tblZiyaretci.FirstOrDefault(x => x.ZiyaretciMAC == macadres);
                if(ziyaretci2 != null)
                {
                    model.sepet = db.tblSepet.Where(x => x.ZiyaretciId == ziyaretci2.ZiyaretciID).ToList();
                }
            }
            return View(model);
        }

        public void SepeteEkle(int id)
        {
            tblSepet sepet = new tblSepet();


            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = User.Identity.Name.Split(' ');
                int kullaniciId = Convert.ToInt32(kullaniciadi[3]);
                tblSepet sepetKullanici = db.tblSepet.FirstOrDefault(x => x.KullaniciId ==kullaniciId&&x.UrunId==id);
                if (sepetKullanici!=null)
                {
                    var GuncellenecekSepet = db.tblSepet.Find(sepetKullanici.SepetId);

                    GuncellenecekSepet.SepettekiAdet += 1;
                    db.SaveChanges();
                    
                }
                else
                {
                    sepet.KullaniciId = kullaniciId;
                    sepet.UrunId = id;
                    sepet.SepettekiAdet = 1;
                    db.tblSepet.Add(sepet);
                    db.SaveChanges();
                }
               
            }
            else
            {
                string macadres = MACAdresiAl();
                tblZiyaretci ziyaretci2 = db.tblZiyaretci.FirstOrDefault(x => x.ZiyaretciMAC == macadres);
                
                if (ziyaretci2!=null)
                {
                    
                    int ZiyaretcininId=ziyaretci2.ZiyaretciID;
                    sepet.ZiyaretciId = ZiyaretcininId;
                    sepet.UrunId = id;
                    if (SepetKontrol(sepet))
                    {
                        var guncellenecekSepet = db.tblSepet.Find(sepetId);
                        guncellenecekSepet.SepettekiAdet = sepetAdet + 1;
                        db.SaveChanges();
                    }
                    else
                    {
                        sepet.SepettekiAdet = 1;
                        db.tblSepet.Add(sepet);
                        db.SaveChanges();
                    }

                }
                else
                {

                    tblZiyaretci ziyaretci = new tblZiyaretci();
                    ziyaretci.ZiyaretciMAC = macadres;
                    db.tblZiyaretci.Add(ziyaretci);
                    db.SaveChanges();
                    tblZiyaretci ziyaretci3 = db.tblZiyaretci.FirstOrDefault(x => x.ZiyaretciMAC == macadres);

                    sepet.ZiyaretciId = ziyaretci3.ZiyaretciID;
                    sepet.UrunId = id;
                    sepet.SepettekiAdet = 1;
                    db.tblSepet.Add(sepet);
                    db.SaveChanges();
                }
            }
        }
        public void SepetGüncelle(int id,int islem)
        {
            tblSepet sepetim = db.tblSepet.FirstOrDefault(x => x.SepetId == id);
            if (islem==0)
            {
                sepetim.SepettekiAdet ++;
                //var guncellenecekSepet = db.tblSepet.Find(sepetId);
                //guncellenecekSepet.SepettekiAdet = sepetAdet + 1;
                db.SaveChanges();
            }
            else if(islem==1)
            {
                if (sepetim.SepettekiAdet==1)
                {
                    SepetSil(id);
                }
                else
                {
                    sepetim.SepettekiAdet--;
                    db.SaveChanges();
                }
            }
        }
        public void SepetSil(int id)
        {
            tblSepet sepetim = db.tblSepet.FirstOrDefault(x => x.SepetId == id);
            db.tblSepet.Remove(sepetim);
            db.SaveChanges();
        }
        public ActionResult Favorilerim(int id,int Page = 1)
        {
            //var a = SayfaBoyutlandir(id);
            var kullaniciAdi = User.Identity.Name.Split(' ');
            Boolean sonuc = kullaniciAdi[0].Equals("");
            if (!sonuc)
            {
                var KullaniciID = KullaniciIdGetir(kullaniciAdi[2]);
                int kID = Convert.ToInt16(KullaniciID[0]);
                var favoriler = db.tblFavoriler.Where(k => k.MusteriID == kID).Include(k => k.tblKullanici).Include(x => x.tblKitap);
                var model = new HomeViewModel
                {
                    YayinEvleri = db.tblYayinEvi.ToList(),
                    KitapTurleri = db.tblKitapTur.ToList(),
                    Kitaplar = db.tblKitap.ToList(),
                    Favoriler = db.tblFavoriler.ToList(),
                    deneme = db.tblKitap.OrderByDescending(x => x.KitapID).ToPagedList(Page, 9)
                };
                ViewBag.favoriList = favoriler.ToList();
                ViewBag.sayfaBoyutu = ajaxverisi;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public JsonResult FavorilereEkle(int id)
        {
            var kullaniciAdi = User.Identity.Name.Split(' ');
            Boolean sonuc = kullaniciAdi[0].Equals("");
            if (!sonuc)
            {
                var KullaniciID = KullaniciIdGetir(kullaniciAdi[2]);
                var favList = db.tblFavoriler.ToList();
                tblKitap urun = db.tblKitap.FirstOrDefault(x => x.KitapID == id);
                int kID = Convert.ToInt16(KullaniciID[0]);
                var BegenenKullanici = db.tblKullanici.FirstOrDefault(x => x.KullaniciID == kID);
                tblFavoriler FavoriyeEkle = new tblFavoriler();
                FavoriyeEkle.MusteriID = kID;
                FavoriyeEkle.UrunID = id;
                db.tblFavoriler.Add(FavoriyeEkle);
                db.SaveChanges();
                return Json("işlem tamam", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("hatalı", JsonRequestBehavior.AllowGet);
            }
        }
        
        public void FavorilerdenCikar(int id)
        {
            
            var favoridenCikarilicakUrun = db.tblKitap.Find(id);
            var kullaniciAdi = User.Identity.Name.Split(' ');
            var KullaniciID = KullaniciIdGetir(kullaniciAdi[2]);
            int kID = Convert.ToInt32(kullaniciAdi[3]), uID = favoridenCikarilicakUrun.KitapID;
            List<tblFavoriler> favList = db.tblFavoriler.Where(x => x.MusteriID == kID).ToList();
            tblFavoriler f = null;
            if (favList != null)
            {
                foreach (var fav in favList)
                {
                    if (fav.UrunID == uID)
                    {
                        f = fav;
                        break;
                    }
                }
            }
            if (f != null)
            {
                db.tblFavoriler.Remove(f);
                db.SaveChanges();
            }
        }
        
        public JsonResult SatinAl(int id,string adres,string fiyat)
        {
            int sayac = 0;
            if (User.Identity.IsAuthenticated)
            {
                bool onay = true;
                HomeViewModel model = new HomeViewModel();
                model.sepet = db.tblSepet.Where(x => x.KullaniciId == id).ToList();
                foreach (var item in model.sepet)
                {
                    if (item.SepettekiAdet > item.tblKitap.Adet)
                    {
                        onay = false;
                        break;
                    }
                }
                if (onay == true)
                {
                    string[] dizi = fiyat.Split(' ');
                    if (dizi != null)
                    {
                        foreach (var item in model.sepet)
                        {
                            if (item.SepettekiAdet < item.tblKitap.Adet)
                            {
                                tblSatinAlinanlar satilan = new tblSatinAlinanlar();
                                satilan.UrunID = item.UrunId;
                                satilan.MusteriID = Convert.ToInt32(item.KullaniciId);
                                satilan.SatinAlmaAdedi = item.SepettekiAdet;
                                satilan.SatinAlmaTarihi = DateTime.Now;
                                satilan.Adres = adres;
                                satilan.UrunBirimFiyati = Convert.ToDecimal(dizi[sayac]);
                                satilan.ToplamFiyat = Convert.ToDecimal(dizi[sayac]) * item.SepettekiAdet;
                                db.tblSatinAlinanlar.Add(satilan);
                                db.SaveChanges();
                                tblKitap guncellenecekKitap = db.tblKitap.Find(item.UrunId);
                                guncellenecekKitap.Adet -= item.SepettekiAdet;
                                db.SaveChanges();
                                tblSepet silinecekSepet = db.tblSepet.Find(item.SepetId);
                                db.tblSepet.Remove(silinecekSepet);
                                db.SaveChanges();

                            }
                            else if (item.SepettekiAdet == item.tblKitap.Adet)
                            {
                                tblSatinAlinanlar satilan = new tblSatinAlinanlar();
                                satilan.UrunID = item.UrunId;
                                satilan.MusteriID = Convert.ToInt32(item.KullaniciId);
                                satilan.SatinAlmaAdedi = item.SepettekiAdet;
                                satilan.UrunBirimFiyati = Convert.ToDecimal(dizi[sayac]);
                                satilan.ToplamFiyat = Convert.ToDecimal(dizi[sayac]) * item.SepettekiAdet;
                                db.tblSatinAlinanlar.Add(satilan);
                                db.SaveChanges();
                                tblKitap guncellenecekKitap = db.tblKitap.Find(item.UrunId);
                                guncellenecekKitap.Adet -= item.SepettekiAdet;
                                guncellenecekKitap.StokDurumu = false;
                                db.SaveChanges();
                                tblSepet silinecekSepet = db.tblSepet.Find(item.SepetId);
                                db.tblSepet.Remove(silinecekSepet);
                                db.SaveChanges();
                            }
                            sayac++;
                        }
                    }
                }
                else
                {

                    return Json("stokyok", JsonRequestBehavior.AllowGet);
                }
                AdresiGuncelle(id, adres);
                return Json("işlem tamam", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("hatalı", JsonRequestBehavior.AllowGet);
            }
            
        }
        public JsonResult AdresiGuncelle(int id,string adres)
        {
            if (!adres.Equals(""))
            {
                tblKullanici kullanicim = db.tblKullanici.FirstOrDefault(x => x.KullaniciID == id);
                kullanicim.Adres = adres;
                db.SaveChanges();
                return Json("işlem tamam", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("hatalı", JsonRequestBehavior.AllowGet);
            }
        }
        public string[] KullaniciIdGetir(string username)
        {
            var kullanici = db.tblKullanici.FirstOrDefault(x => x.Mail == username);
            return new string[] { kullanici.KullaniciID.ToString() };
        }
        
        public void SayfaBoyutlandir(string gelenurl, int id)
        {
            string deneme = gelenurl;
            ajaxverisi = id;
            string[] dizi = deneme.Split('/');
            int uzunluk = dizi.Length;
            if (dizi[1].Equals("Home") || dizi[1].Equals(""))
            {
                if (uzunluk==2)
                {
                    HomeController.urlKontol(null, null, id);
                }
                else if (uzunluk >2&&uzunluk<4)
                {
                    HomeController.urlKontol(dizi[2],null,id);

                }
                else if (uzunluk == 4)
                {
                    HomeController.urlKontol(dizi[2],dizi[3],id);
                }
            }else if (dizi[1].Equals("Urun"))
            {
                if (dizi[2].Equals("Favorilerim"))
                {
                    Favorilerim(ajaxverisi);
                    ViewBag.sayfaBoyutu = ajaxverisi;
                    
                }
            }
        }
        public  bool SepetKontrol(tblSepet sepet)
        {
            tblSepet sepet2 = db.tblSepet.FirstOrDefault(x => x.ZiyaretciId == sepet.ZiyaretciId && x.UrunId == sepet.UrunId);
            
            if (sepet2!=null)
            {
                sepetId = sepet2.SepetId;
                sepetAdet = sepet2.SepettekiAdet;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string MACAdresiAl()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty) sMacAddress = adapter.GetPhysicalAddress().ToString();
            }
            return sMacAddress;
        }
    }
}