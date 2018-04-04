using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class PassportInfoViewModel
    {
        [Required(ErrorMessage = "ErrorTypeOfDocument")]
        [Display(Name = "TypeOfDocumentId")]
        public int TypeOfDocumentId { get; set; }

        [Required(ErrorMessage = "ErrorDateofExtraditionEmpty")]
        [Display(Name = "DateofExtradition")]
        public string DateofExtradition { get; set; }

        [Required(ErrorMessage = "ErrorNumberEmpty")]
        [Display(Name = "Number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "ErrorIssuedByEmpty")]
        [Display(Name = "IssuedBy")]
        public string IssuedBy { get; set; }

        [Required(ErrorMessage = "ErrorValidatyEmpty")]
        [Display(Name = "Validaty")]
        public string Validaty { get; set; }

        [Required(ErrorMessage = "ErrorSeriesEmpty")]
        [Display(Name = "Series")]
        public string Series { get; set; }

    }
}
