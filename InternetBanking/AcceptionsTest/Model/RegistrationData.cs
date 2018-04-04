using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class RegistrationData
    {
        [Key]
        public int Id { get; set; }

        public string RegistrationAuthority { get; set; }


        public DateTime DateOfRegistrationMinistryJustice { get; set; }


        public string IssuedBy { get; set; }


        public DateTime DateOfInitialRegistration { get; set; }



        public int? TaxInspectionId { get; set; }
        public TaxInspection TaxInspection { get; set; }
    }
    public class TaxInspection
    {
        [Key]
        public int Id { get; set; }
        public string TaxInspectionName { get; set; }
    }
}