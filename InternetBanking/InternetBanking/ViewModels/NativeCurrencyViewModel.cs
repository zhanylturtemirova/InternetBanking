using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class NativeCurrencyViewModel
    {
        public SelectList Currencies { get; set; }
        public int CurrencyId { get; set; }
    
        public String  NativeCurrencyName { get; set; }

    }
}
