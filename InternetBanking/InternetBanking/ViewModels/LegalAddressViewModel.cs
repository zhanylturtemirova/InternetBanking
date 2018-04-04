using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class LegalAddressViewModel
    {



        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public SelectList Countries { set; get; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "PostCode")]
        public string PostCode { get; set; }
        
        [Display(Name = "Street")]
        public string Street { get; set; }
       
        [Display(Name = "HouseAddress")]
        public string HouseAddress { get; set; }


    }
}
