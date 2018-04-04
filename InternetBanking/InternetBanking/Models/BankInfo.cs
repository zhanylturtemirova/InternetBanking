using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class BankInfo
    {
        [Key]
        public int Id { get; set; }
        public string BankName { get; set; }
        public string Email { get; set; }
    }
}
