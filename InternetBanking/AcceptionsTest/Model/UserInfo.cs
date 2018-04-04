using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AcceptionsTest.Model;


namespace AcceptionsTest.Model
{
    public class UserInfo 
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }

        // public string PhoneNumber { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "неверный формат E-mail")]
        public string Email { get; set; }

        public string Gender { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public string Inn { get; set; }

        public int? CitezenshipId { get; set; }
        [ForeignKey("CitezenshipId")]
        public Country Country { get; set; }

        public DateTime BirthDay { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public int PassportInfoId { get; set; }
        [ForeignKey("PassportInfoId")]
        public PassportInfo PassportInfo { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? ContactInfoId { get; set; }
        [ForeignKey("ContactInfoId")]
        public ContactInfo ContactInfo { get; set; }

        public UserInfo()
        {
            Accounts = new List<Account>();
            Addresses = new List<Address>();
        }
        public int GetPageSize()
        {
            return 3;
        }
    }
   
}
