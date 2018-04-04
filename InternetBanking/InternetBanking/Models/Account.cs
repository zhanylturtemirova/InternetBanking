using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string Number { get; set; }

        public bool Locked { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public int? UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }  

        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
