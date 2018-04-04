using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class PaymentScheduleViewModel
    {
        public string TemplateName { get; set; }
        public int TemplateId { get; set; }

        [Display(Name = "IntervalTypeName")]
        public int? IntervalCode { get; set; }

        public SelectList IntervalTypes { get; set; }

        [Required(ErrorMessage = "ErrorStartDate")]
        [Display(Name = "DateStartName")]
        public DateTime DateStart { get; set; }

        [Display(Name = "DateEndName")]
        public string DateEnd { get; set; }
    }
}
