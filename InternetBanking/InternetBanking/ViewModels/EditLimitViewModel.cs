using InternetBanking.Models.SelectTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class EditLimitViewModel
    {
        public Limit Limit { get; set; }
        public LimitInfoViewModel LimitInfo { get; set; }
    }
}
