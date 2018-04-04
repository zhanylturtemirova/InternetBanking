using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models.SelectTable;

namespace InternetBanking.Models
{
    public class RegistrationData
    {
        [Key]
        public int Id { get; set; }

        public string RegistrationAuthority { get; set; }

   
        public DateTime DateOfRegistrationMinistryJustice { get; set; }

      
        public string IssuedBy { get; set; }

      
        public DateTime DateOfInitialRegistration { get; set;}


       
        public int? TaxInspectionId { get; set; }
        public TaxInspection TaxInspection { get; set; }

    }
}
