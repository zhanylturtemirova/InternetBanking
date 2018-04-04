using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class BlackList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorNameEmpty")]
        [Display(Name = "BlackListedPassword")]
        public string BlackListedPassword { get; set; }

        public BlackList()
        {

        }
    }
}
