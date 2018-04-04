using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels.Enums
{
    public enum TransferStatesEnum
    {
        Confirmed = 1,
        NotConfirmed = 2,
        Canceled = 3,
        BalanceNotEnough = 4,
        AccountIsLocked = 5
    }
}
