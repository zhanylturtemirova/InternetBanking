using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Models
{
    public class AddressType
    {
        [Key]
        public int Id { get; set; }

        public string TypeName { get; set; }

        public static AddressType Create(AddressTypesEnum enumItem)
        {
            return new AddressType{TypeName = enumItem.ToString()};
        }

        public bool IsEqual(AddressTypesEnum enumItem)
        {
            return this.TypeName == enumItem.ToString();
        }
    }
}
