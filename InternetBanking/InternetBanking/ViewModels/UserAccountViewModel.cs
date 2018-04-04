using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class UserAccountViewModel
    {
        public List<AccountWithBalance> Accounts { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
