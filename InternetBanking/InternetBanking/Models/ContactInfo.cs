using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }
        public string MobilePhone { get; set; }
        public string CityPhone { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<UserInfo> UserInfos { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }

        public ContactInfo()
        {
            UserInfos = new List<UserInfo>();
            Companies = new List<Company>();
            EmployeeInfos = new List<EmployeeInfo>();
        }
    }
}
