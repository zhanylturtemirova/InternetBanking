using InternetBanking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models.SelectTable
{
    public class Limit : IPageble
    {
        public int Id { get; set; }
        public string LimitName { get; set; }
        public decimal LimitAmount { get; set; }
        int IPageble.GetPageSize()
        {
            return 3;
        }
    }
}
