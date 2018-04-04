using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels.Paging;
using InternetBanking.ViewModels;

namespace InternetBanking.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationContext context;
        private readonly IEmployeeService employeeService;
        private readonly IHomePagingService pagingService;
        private readonly IUserService userService;

        public EmployeeController(ApplicationContext context, IEmployeeService employeeService, IHomePagingService pagingService, 
            IUserService userService)
        {
            this.context = context;
            this.employeeService = employeeService;
            this.pagingService = pagingService;
            this.userService = userService;
        }

        public async Task<ActionResult> Index(int id, int page = 1)
        {
            IQueryable<EmployeeInfo> employees = employeeService.GetEmployeesByCompanyId(id).OrderBy(c => c.FirstName);
            PagedObject<EmployeeInfo> pagedObject = await pagingService.DoPage<EmployeeInfo>(employees, page);

            PagingViewModel<EmployeeInfo> EmployeePagingViewModel = new PagingViewModel<EmployeeInfo>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            ViewBag.CompanyId = id;

            return View(EmployeePagingViewModel);
        }

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        public async Task<ActionResult> Edit(int id)
        {
            EmployeeInfo employee = employeeService.FindEmployeeById(id);
            User user = await userService.FindUserById(employee.UserId);
            
            RegisterEmployeeViewModel model = new RegisterEmployeeViewModel
            {
                Email = user.Email,
                FirstName = employee.FirstName,
                SecondName = employee.SecondName,
                MiddleName = employee.MiddleName,
                Position = employee.Position,
                TwoFactorOn = user.IsTwoFactorOn,
                UserId = user.Id,
                CompanyId = employee.CompanyId

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RegisterEmployeeViewModel model)
        {
            try
            {
                User user = await userService.FindUserById(model.UserId);
                user.Email = model.Email;
                user.IsTwoFactorOn = model.TwoFactorOn;
                employeeService.UpdateEmployee(user, model);
                return RedirectToAction("Index", "Employee", new { id = model.CompanyId});
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult EmployeeInfo(int id)
        {
            EmployeeInfo employee = employeeService.FindEmployeeById(id);
 
            return View(employee);
        }
    }
}