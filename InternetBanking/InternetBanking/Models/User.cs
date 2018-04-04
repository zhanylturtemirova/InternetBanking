using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Models
{
    public class User : IdentityUser
    {
        public bool IsPasswordChanged { get; set; }
        public bool IsTwoFactorOn { get; set; }
        public int LoginAttemptsCount { get; set; }
        public string userSendEmailToken { get; set; }
        public bool IsBlocked { get; set; }
      


    }
}
