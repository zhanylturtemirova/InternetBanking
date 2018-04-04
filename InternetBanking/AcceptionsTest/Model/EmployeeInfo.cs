using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcceptionsTest.Model
{
    public class EmployeeInfo
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? ContactInfoId { get; set; }
        [ForeignKey("ContactInfoId")]
        public ContactInfo ContactInfo { get; set; }

        public bool Chief { get; set; }

       
    }
}