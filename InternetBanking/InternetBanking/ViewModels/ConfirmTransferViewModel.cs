using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;

namespace InternetBanking.ViewModels
{
    public class ConfirmTransferViewModel : IPageble
    {
        public int TransferId { get; set; }

        public InnerTransfer Transfer { get; set; }

        public bool IsUserHaveRightOfConfirm { get; set; }

        public int GetPageSize()
        {
            return 20;
        }
    }
}
