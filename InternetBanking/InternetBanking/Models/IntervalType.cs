using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Models
{
    public class IntervalType
    {
        [Key]
        public int Id { get; set; }

        public int IntervalCode { get; set; }
        public string IntervalName { get; set; }

        


        public static IntervalType Create(IntervalTypesEnum enumItem)
        {
            IntervalType type = new IntervalType
            {
                IntervalCode = (int)enumItem,
                IntervalName = enumItem.ToString(),
            };
            return type;
        }

        public bool IsEqual(IntervalTypesEnum enumItem)
        {
            return this.IntervalName == enumItem.ToString();
        }
    }
}
