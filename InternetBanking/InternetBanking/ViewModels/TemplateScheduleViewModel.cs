using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;

namespace InternetBanking.ViewModels
{
    public class TemplateScheduleViewModel
    {
       public Template Template { get; set; }
        public bool IsScheduleExist { get; set; }
    }
}
