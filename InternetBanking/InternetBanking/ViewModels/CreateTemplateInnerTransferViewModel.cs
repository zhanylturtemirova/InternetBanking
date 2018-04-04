using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class CreateTemplateInnerTransferViewModel
    {
        public TemplateViewModel Template { get; set; }
        public InnerTransferViewModel Transfer { get; set; }
    }
}
