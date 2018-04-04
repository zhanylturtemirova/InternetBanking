using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Cache;
using System.Security.AccessControl;
using System.Threading.Tasks;
using InternetBanking.Models.SelectTable;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class CreateUserAccountViewModel
    {
        [Required]
        public string UserId { get; set; }
        
        public SelectList Currencies { get; set; }

        [Required(ErrorMessage = "ErrorEmptyCarrency")]
        [Display(Name = "SelectCarrency")]
        public int?  CurrencyId { get; set; }

        [Required(ErrorMessage = "ErrorEmptyLimit")]
        public int? LimitId { get; set; }
        public SelectList Limits { get; set; }
  
    }
}
