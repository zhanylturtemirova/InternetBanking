using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class PlaceOfBirthViewModel
    {
        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }
    }
}
