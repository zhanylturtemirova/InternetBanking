using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Models.SelectTable
{
    public class TypeOfTransfer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public static TypeOfTransfer Create(TypeOfTransfersEnum enumItem)
        {
            return new TypeOfTransfer { Name = enumItem.ToString() };
        }

        public bool IsEqual(TypeOfTransfersEnum enumItem)
        {
            return this.Name == enumItem.ToString();
        }
    }
}
