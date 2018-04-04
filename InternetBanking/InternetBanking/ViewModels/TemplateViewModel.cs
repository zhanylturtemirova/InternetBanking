using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class TemplateViewModel
    {
        public int TemplateId { get; set; }

        [Required(ErrorMessage = "ErrorTempalteName")]
        [Display(Name = "TempalteName")]
        public string TempalteName { get; set; }

        [Required(ErrorMessage = "ErrorTemplateDiscription")]
        [Display(Name = "TemplateDiscription")]
        public string TemplateDiscription { get; set; }

        [Display(Name = "IsSetSchedule")]
        public bool IsSetSchedule { get; set; }

        public PaymentScheduleViewModel PaymentScheduleViewModel { get; set; }

    }
}
