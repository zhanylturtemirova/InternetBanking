using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class PlaceOfBirthViewModel
    {
        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public SelectList Countries { set; get; }

        [Display(Name = "City")]
        public string City { get; set; }
    }
}
