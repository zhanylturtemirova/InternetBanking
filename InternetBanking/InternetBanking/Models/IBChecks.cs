using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class IBChecks
    {
        public bool UserExistenceCheck(IQueryable<User> users, string email)
        {
            bool IsExist = users.Any(u => u.Email == email);
            return IsExist;
        }
    }
}
