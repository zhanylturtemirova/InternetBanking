using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;

namespace InternetBanking.Tests
{
    public class UserServiceTests
    {
        public ApplicationContext context;
        private readonly UserManager<User> userManager;
        public IUserService userService;

        public UserServiceTests()
        {

            TestServicesProvider.GetModelTestData().FillData();
            context = TestServicesProvider.GetContext();

        }

        [Fact]
        public void CreatingLoginTest()
        {
            UserInfoViewModel model = new UserInfoViewModel {FirstName = "Isa", SecondName = "Baigaziev"};
            string login = userService.CreateLogin(model);
            string login1 = userService.CreateLogin(model);
            string login2 = userService.CreateLogin(model);
            string login3 = userService.CreateLogin(model);
            Assert.Equal("iBaigaziev", login);
        }
        //[Fact]
        //public void CreatingNewUserTest()
        //{
        //    IUserService userService = new UserService(userManager, context);
        //    User user = new User { Email = "islam13@m.ru", UserName = "islam13@m.ru" , IsPasswordChanged = false, IsTwoFactorOn = true };
        //    UserInfoViewModel userInfoView = new UserInfoViewModel {Password = "I123s@", PasswordConfirm = "I123s@" };
        //    userService.CreateUser(user, userInfoView);
        //    Task<User> user1 = userManager.Users.FirstAsync(c=>c.Email == user.Email);
        //    Task <User> userTest = userService.IsAccountExistTest(user.UserName);
        //    Assert.NotNull(userTest);
        //}

        //[Fact]
        //public void CreatingNewUserInfoTest()
        //{
        //    IUserService userService = new UserService();
        //    User user = new User { Email = "islam13@m.ru", UserName = "islam13@m.ru" };
        //    userService.CreateUserTest(user, "I123s@");
        //    Task<User> userTest = userService.IsAccountExistTest(user.UserName);
        //    DateTime time = DateTime.Now;
        //    PassportInfo passportInfo = new PassportInfo{ IssuedBy = "Lenin" , DateofExtradition = time, Number = 4561, Series = "54621sad", Validaty = time};
        //    userService.CreatePassportInfoTest(passportInfo);
        //    UserInfoViewModel userInfoView = new UserInfoViewModel{BirthDay = time, Email = user.Email, Inn = "65546", Citizenship = "Kyr", FirstName = "islma", SecondName = "bob", MiddleName = "glob", Gender = "male"};

        //    userService.CreateUserInfoTest(user, passportInfo, userInfoView);
        //    Task<UserInfo> userInfo = userService.FindUserInfoByUserId(user.Id);
        //    Assert.NotNull(userInfo);
        //}

        [Fact]
        public void CreatingAddressesTest()
        {
            //IUserService userService = new UserService();
            //User user = new User { Email = "islam13@m.ru", UserName = "islam13@m.ru" };
            //userService.CreateUserTest(user, "I123s@");
            //Task<User> userTest = userService.IsAccountExistTest(user.UserName);
            //DateTime time = DateTime.Now;
            //PassportInfo passportInfo = new PassportInfo { IssuedBy = "Lenin", DateofExtradition = time, Number = 4561, Series = "54621sad", Validaty = time };
            //userService.CreatePassportInfoTest(passportInfo);
            //UserInfoViewModel userInfoView = new UserInfoViewModel { BirthDay = time, Email = user.Email, Inn = "65546", Citizenship = "Kyr", FirstName = "islma", SecondName = "bob", MiddleName = "glob", Gender = "male" };
            //userService.CreateUserInfoTest(user, passportInfo, userInfoView);
            //userService.CreateUserInfoTest(user, passportInfo, userI`nfoView);
            //Task<UserInfo> userInfo = userService.FindUserInfoByUserId(user.Id);
            //FactAddressViewModel address = new FactAddressViewModel();
            //LegalAddressViewModel address1 = new LegalAddressViewModel(); 
            //PlaceOfBirthViewModel address2 = new PlaceOfBirthViewModel();
            //userService.CreateAddress(userInfo, address);
            //Assert.NotNull(userInfo);
           
        }
    }
}
