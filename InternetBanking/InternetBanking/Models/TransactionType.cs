using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Models
{
    public class TransactionType 
    {
        [Key]
        public int Id { get; set; }

        public string TypeName { get; set; }

        public static TransactionType Create(TransactionTypesEnum enumItem)
        {
            return new TransactionType{ TypeName = enumItem.ToString() };
        }
        

        public bool IsEqual(TransactionTypesEnum enumItem)
        {
            return this.TypeName == enumItem.ToString();
        }
    }
}
