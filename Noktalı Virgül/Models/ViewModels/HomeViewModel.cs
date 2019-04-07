using Noktalı_Virgül.Models.VeriTabani;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noktalı_Virgül.Models.ViewModels
{
    public class HomeViewModel
    {
        // liste döner
        public IList<tblYayinEvi> YayinEvleri { get; set; }
        public IList<tblKitapTur> KitapTurleri { get; set; }
        public IList<tblKitap> Kitaplar { get; set; }
        public IList<tblFavoriler> Favoriler { get; set; }
        public IList<tblPromosyon> Promosyonlar { get; set; }
        public IPagedList<tblKitap> deneme;

        public IList<tblSepet> sepet { get; set; }
        public IList<ister_Result> ister { get; set; }
        // tek obje döner
        // public tblKitap Kitap { get; set; }
    }
}