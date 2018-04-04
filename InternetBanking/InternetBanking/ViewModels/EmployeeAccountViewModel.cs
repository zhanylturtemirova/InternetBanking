using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class EmployeeAccountViewModel
    {
        public Account Account { get; set; }

        public int? EmployeeId { get; set; }
        public EmployeeInfo Employee { get; set; }


        [Required(ErrorMessage = "*Укажите лимит!")]
        public int? LimitId { get; set; }
        public SelectList Limits { get; set; }

        public int? UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        public bool RightOfConfirmation { get; set; }
        public bool RightOfCreate { get; set; }
    }
}
