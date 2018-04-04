using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<User> userManager;
        private ApplicationContext context;

        public EmployeeService(UserManager<User> userManager, ApplicationContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public async Task<IdentityResult> CreateEmployee(User user, RegisterEmployeeViewModel model)
        {

            return await userManager.CreateAsync(user, model.Password);
        }

        public EmployeeInfo CreateEmployeeInfo(User user, RegisterEmployeeViewModel model)
        {
            bool chief = context.EmployeeInfos.FirstOrDefault(ei => ei.CompanyId == model.CompanyId && ei.Chief) == null;
            EmployeeInfo employee = new EmployeeInfo
            {
                UserId = user.Id,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                MiddleName = model.MiddleName,
                Position = model.Position,
                CompanyId = model.CompanyId,
                Chief = chief
            };
            context.EmployeeInfos.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public IQueryable<EmployeeInfo> GetEmployeesByCompanyId(int id)
        {
            IQueryable<EmployeeInfo> employees = context.EmployeeInfos.Include(e => e.Company).Where(e => e.CompanyId == id);

            return employees;
        }

        public EmployeeInfo FindEmployeeById(int id)
        {
            EmployeeInfo employee = context.EmployeeInfos.Include(e => e.Company).Include(e => e.User).FirstOrDefault(e => e.Id == id);

            return employee;
        }

        public void UpdateEmployee(User user, RegisterEmployeeViewModel model)
        {
            EmployeeInfo employeeInfo = context.EmployeeInfos.FirstOrDefault(e => e.UserId == model.UserId);
            employeeInfo.FirstName = model.FirstName;
            employeeInfo.SecondName = model.SecondName;
            employeeInfo.MiddleName = model.MiddleName;
            employeeInfo.Position = model.Position;        
            context.EmployeeInfos.Update(employeeInfo);
            context.Users.Update(user);
            context.SaveChanges();
        }

        public async Task<EmployeeInfo> GetEmployeeInfoByUserId(string userId)
        {
            
            EmployeeInfo employee = await context.EmployeeInfos.FirstOrDefaultAsync(e=>e.UserId == userId);
            return employee;
        }

        public async Task<Company> GetCompanyByEmployeeId(int employeeId)
        {
            EmployeeInfo employee = FindEmployeeById(employeeId);
            Company company = await context.Companies.FirstOrDefaultAsync(c => c.Id == employee.CompanyId);
            return company;
        }
    }
}
