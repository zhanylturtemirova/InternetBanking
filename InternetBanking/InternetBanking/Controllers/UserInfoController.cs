using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Paging;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
    public class UserInfoController : Controller
    { 
        private IHomePagingService pagingService;
        private readonly IUserService userService;
        private readonly IAccountService accountService;
        private readonly ISelectListService selectListService;
      



        public UserInfoController(IHomePagingService pagingService, IUserService userService, ISelectListService selectListService, IAccountService accountService)
        {
            this.pagingService = pagingService;
            this.userService = userService;
            this.selectListService = selectListService;
            this.accountService = accountService;
           
        }
        public async Task<IActionResult> Index(int page = 1)
        {
           
            IQueryable<UserInfo> users = userService.GetUserList().OrderBy(n => n.User.UserName);
            PagedObject<UserInfo> pagedObject = await pagingService.DoPage<UserInfo>(users, page);

            PagingViewModel<UserInfo> UsersPagingViewModel = new PagingViewModel<UserInfo>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            return View(UsersPagingViewModel);
        }

        public IActionResult UserInfo(int userId)
        {
            UserEditViewModel userInfo = userService.GetUserInfoEdit(userId);
            userInfo.User.Accounts= accountService.GetUserAccountsWithoutBalance(userId);
            userInfo.User.Addresses = userService.GetUserAdressList(userInfo.User);
            return View(userInfo);
        }


        [HttpGet]
        public IActionResult Edit(int userId)
        {
            UserEditViewModel userEdit = userService.GetUserInfoEdit(userId);
            return View(userEdit);
        }


        [HttpPost]
        public IActionResult Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                userService.PassportEdit(model.User.PassportInfo, model.PassportInfo);
                userService.ContactInfoEdit(model.User.ContactInfo, model.ContactInfo);
                userService.AddressEdit(model.User, model.FactAddress);
                userService.AddressEdit(model.User, model.LegalAddress);
                userService.AddressEdit(model.User, model.PlaceOfBirth);
                userService.UserEdit(model.User, model.UserInfo);

                return RedirectToAction("UserInfo", "UserInfo", new { userId = model.User.Id});
            }
            
                model.LegalAddress.Countries= selectListService.GetCountries();
                model.PlaceOfBirth.Countries= selectListService.GetCountries();
                model.FactAddress.Countries= selectListService.GetCountries();
                model.UserInfo.Countries = selectListService.GetCountries();
                model.PassportInfo.TypeOfDocuments = selectListService.GetTypeOfDocuments();
                return View(model);

        }

    }
}