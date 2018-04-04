using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Services;

namespace InternetBanking.Models
{
    
    public class Bank : IPageble
    {
        public Bank(int id, string bik, int bankInfoId)
        {
            Id = id;
            BIK = bik;
            BankInfoId = bankInfoId;
        }

        public Bank()
        { 
        }

        [Key]
        public int Id { get; set; }
        public string BIK { get; set; }
        public int BankInfoId { get; set; }
        public BankInfo BankInfo { get; set; }

        int IPageble.GetPageSize()
        {
            return 3;
        }
    }
}
