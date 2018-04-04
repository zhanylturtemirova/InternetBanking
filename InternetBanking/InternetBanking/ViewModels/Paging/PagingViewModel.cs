using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels.Paging
{
    public class PagingViewModel<T>
    {
        public PageViewModel PageViewModel { get; set; }
        public List<T> Objects { get; set; }
    }
}
