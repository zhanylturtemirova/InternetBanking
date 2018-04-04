using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class FactAddressViewModel
    {


        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
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
