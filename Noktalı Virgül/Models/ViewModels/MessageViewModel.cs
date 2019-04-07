using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Noktalı_Virgül.Models.VeriTabani;
namespace Noktalı_Virgül.Models.ViewModels
{
    public class MessageViewModel
    {
        public IList<tblMesaj> Mesajlar { get; set; }
    }
}