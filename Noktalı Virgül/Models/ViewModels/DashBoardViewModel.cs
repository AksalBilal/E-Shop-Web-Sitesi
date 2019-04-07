using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Noktalı_Virgül.Models.VeriTabani;
namespace Noktalı_Virgül.Models.ViewModels
{
    public class DashBoardViewModel
    {

        public int ToplamUrunSatisSayisi { get; set; }
        public int ToplamUrunSatisSayisiGunluk { get; set; }
        public int ToplamUrunSatisSayisiHaftalik { get; set; }
        public int ToplamUyeKayitSayisi { get; set; }
        public int ToplamUyeKayitSayisiGunluk { get; set; }
        public int ToplamUyeKayitSayisiHaftalik { get; set; }
        public IList<Encoksatisyapilanurunler_Result> encoksatisyapilanurunler { get; set; }
        public IList<EnCokSepeteEklenenUrunler_Result> encoksepeteeklenenurunler { get; set; }
        public IList<EnCokSatisYapanKategori_Result> EnCokSatisYapanKategori { get; set; }
    }
}