using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptionsTest.Model
{
    public class User 
    {

        public Guid  Id { get; set; }
        public string UserName { get; set; }
        public string Email  { get; set; }
        public bool IsPasswordChanged  { get; set; }
        public bool IsTwoFactorOn { get; set; }
        public int LoginAttemptsCount { get; set; }
        public string UserSendEmailToken { get; set; }
        public bool IsBlocked { get; set; }
        
    }
}
