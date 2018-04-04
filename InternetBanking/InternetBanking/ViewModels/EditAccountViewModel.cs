using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class EditAccountViewModel
    {
        public int AccountId { get; set; }
        public List<EmployeeAccountViewModel> UserAccounts { get; set; }
        
     
    }
}
