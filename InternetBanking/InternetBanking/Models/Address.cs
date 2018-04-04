using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models.SelectTable;

namespace InternetBanking.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string HouseAddress { get; set; }

        public int? UserInfoId { get; set; }
        [InverseProperty("Addresses")]
        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int AddressTypeId { get; set; }
        public AddressType AddressType { get; set; }

      
    }
}
