using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class CreateTemplateInterBankTransferViewModel
    {
        public TemplateViewModel Template { get; set; }
        public InterBankTransferViewModel Transfer { get; set; }
    }
}
