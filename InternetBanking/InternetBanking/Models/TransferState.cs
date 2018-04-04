using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels.Enums;
namespace InternetBanking.Models
{
    public class TransferState
    {
        [Key]
        public int Id { get; set; }

        public string StateName { get; set; }

        public static TransferState Create(TransferStatesEnum enumItem)
        {
            return new TransferState{StateName = enumItem.ToString()};
        }

        public bool IsEqual(TransferStatesEnum enumItem)
        {
            return this.StateName == enumItem.ToString();
        }
    }
}
