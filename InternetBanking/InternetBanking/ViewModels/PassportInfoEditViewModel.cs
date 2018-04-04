using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class PassportInfoEditViewModel
    {
        [Required(ErrorMessage = "ErrorTypeOfDocument")]
        [Display(Name = "TypeOfDocumentId")]
        public int TypeOfDocumentId { get; set; }
        public SelectList TypeOfDocuments { get; set; }

        [Required(ErrorMessage = "ErrorDateofExtraditionEmpty")]
        [Display(Name = "DateofExtradition")]
        public DateTime DateofExtradition { get; set; }

        [Required(ErrorMessage = "ErrorNumberEmpty")]
        [Display(Name = "Number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "ErrorIssuedByEmpty")]
        [Display(Name = "IssuedBy")]
        public string IssuedBy { get; set; }

        [Required(ErrorMessage = "ErrorValidatyEmpty")]
        [Display(Name = "Validaty")]
        public DateTime Validaty { get; set; }

        [Required(ErrorMessage = "ErrorSeriesEmpty")]
        [Display(Name = "Series")]
        public string Series { get; set; }
    }
}
