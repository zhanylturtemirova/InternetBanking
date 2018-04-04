using InternetBanking.Models.SelectTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class EmployeeAccount
    {
        [Key]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }
        public EmployeeInfo Employee { get; set; }

        public int? UserId { get; set; }
        public UserInfo User { get; set; }

        public int? LimitId { get; set; }
        public Limit limit { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public bool RightOfConfirmation { get; set; }

        public bool RightOfCreate { get; set; }
    }
}
