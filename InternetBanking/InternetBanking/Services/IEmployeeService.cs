using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Services
{
    public interface IEmployeeService
    {
        EmployeeInfo CreateEmployeeInfo(User user, RegisterEmployeeViewModel model);
        Task<IdentityResult> CreateEmployee(User user, RegisterEmployeeViewModel model);
        IQueryable<EmployeeInfo> GetEmployeesByCompanyId(int id);
        EmployeeInfo FindEmployeeById(int id);
        void UpdateEmployee(User user, RegisterEmployeeViewModel model);
        Task<EmployeeInfo> GetEmployeeInfoByUserId(string userId);
        Task<Company> GetCompanyByEmployeeId(int employeeId);
    }
}
