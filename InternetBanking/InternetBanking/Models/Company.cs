using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models.SelectTable;
using InternetBanking.Services;

namespace InternetBanking.Models
{
    public class Company : IPageble
    {
       [Key]
        public int Id {get; set;}

        public string NameCompany { get; set;}

        public int? LegalFormId { get; set; }
        public LegalForm LegalForm { get; set; }

        public int? PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }

        public string INN {get; set;}

        public string CodeOKPO { get; set; }

        public string RegistrationNumberSocialFund { get; set; }

        public int? ResidencyId { get; set; }
        public Residency Residency { get; set;}

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int RegistrationDataId { get; set; }
        public RegistrationData RegistrationData { get; set; }

        public int? ContactInfoId { get; set; }
        [ForeignKey("ContactInfoId")]
        public ContactInfo ContactInfo { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }

        public int NumberOfEmployees { get; set; }

        public string Logo { get; set; }

        public Company()
        {
            Accounts = new List<Account>();
            EmployeeInfos = new List<EmployeeInfo>();
            Addresses = new List<Address>();
        }

        public int GetPageSize()
        {
            return 3;
        }
    }
}
