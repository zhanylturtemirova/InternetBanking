using InternetBanking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models.SelectTable
{
    public class PaymentСode : IPageble
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string PaymentCodeName { get; set; }

        int IPageble.GetPageSize()
        {
            return 3;
        }
    }
}
