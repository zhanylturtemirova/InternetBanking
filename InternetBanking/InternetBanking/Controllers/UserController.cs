using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EncryptStringSample;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1;

namespace InternetBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private ApplicationContext context;
        private readonly IUserService userService;
        private readonly ISelectListService selectListService;

        private readonly IGeneratePassword generatePasswordService;
        private readonly IAccountService accountService;
        private readonly IEmployeeService employeeService;
        private readonly EmailService emailService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly ICompanyService companyService;
        private readonly IValidationService validationService;
        private IXmlService xmlService;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager,
            ApplicationContext context, IUserService userService, ISelectListService selectListService,
            IAccountService accountService, IEmployeeService employeeService
            , IExchangeRateService exchangeRateService, ICompanyService companyService, IValidationService validationService,
            IXmlService xmlService)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
            this.userService = userService;
            this.selectListService = selectListService;
            this.accountService = accountService;
            generatePasswordService = new GeneratePasswordService();
            emailService = new EmailService();
            this.employeeService = employeeService;
            this.exchangeRateService = exchangeRateService;
            this.companyService = companyService;
            this.validationService = validationService;
            this.xmlService = xmlService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            xmlService = new XmlService();
            string str = xmlService.XmlToString();
            var list = xmlService.ConvertXmlToList(str);
            ViewBag.XmlList = list;

            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            if (User.Identity.IsAuthenticated)
            {
                UserIndexViewModel model = new UserIndexViewModel();

                string UserIdFromUsers = userService.FindUserByName(User.Identity.Name).Id;
                string userName = string.Empty;
                string logoCompany = string.Empty;
                int userId = 0;
                UserInfo userFromUserInfo = userService.FindUserByIdInUserInfo(UserIdFromUsers, ref userName, ref userId);
                if (userFromUserInfo == null)
                {
                    EmployeeInfo userFromEmployeeInfo = userService.FindUserByIdInCompany(UserIdFromUsers, ref userName, ref userId);
                    model.UserAccounts = accountService.GetCompanyAccounts(userId);
                    if (companyService.FindCompanyById(userId).Logo != null)
                    {
                        logoCompany = companyService.FindCompanyById(userId).Logo;
                    }
                }
                else
                {
                    model.UserAccounts = accountService.GetUserInfoAccounts(userId);
                }

                if (userId != 0)
                {
                    model.UserId = userId;
                    model.UserName = userName;
                }
                HttpContext.Session.SetString("FullName", userName);
                HttpContext.Session.SetString("Logo", logoCompany);
                HttpContext.Session.SetString("Email", userService.FindUserByName(User.Identity.Name).Email);

                model.NBKRRates = exchangeRateService.GetLastExchangeRatesByDate().Where(r => r.ExchangeRateTypeId == 1).ToList();
                model.MarketRates = exchangeRateService.GetLastExchangeRatesByDate().Where(r => r.ExchangeRateTypeId == 2).ToList();
                ViewBag.UserId = UserIdFromUsers;
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult RegisterPerson()
        {
            RegisterPersonViewModel registerPersonViewModel = new RegisterPersonViewModel
            {

            FactAddress = new FactAddressViewModel{Countries = selectListService.GetCountries()},
            PlaceOfBirth = new PlaceOfBirthViewModel{Countries = selectListService.GetCountries() },
            ContactInfo = new ContactInfoViewModel (),
            LegalAddress = new LegalAddressViewModel{Countries = selectListService.GetCountries()},
            UserInfo = new UserInfoViewModel{Countries = selectListService.GetCountries()} ,
           
            PassportInfo = new PassportInfoViewModel {TypeOfDocuments = selectListService.GetTypeOfDocuments()}
        };
         

            return View(registerPersonViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPerson(RegisterPersonViewModel model)
        {
            validationService.ValidateRegisterPersonViewModel(model, ModelState);            
            model.UserInfo.Password = generatePasswordService.CreatePassword();
            model.UserInfo.PasswordConfirm = model.UserInfo.Password;
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.UserInfo.Email, UserName = userService.CreateLogin(model.UserInfo), IsPasswordChanged = false, IsTwoFactorOn = true };
                var result = await userService.CreateUser(user, model.UserInfo);
                if (result.Succeeded)
                {
                    ContactInfo contactInfo = await userService.CreateContactInfo(model.ContactInfo);
                    PassportInfo passportInfo = await userService.CreatePassportInfo(model.PassportInfo);
                    UserInfo userInfo = await userService.CreateUserInfo(user,passportInfo,model.UserInfo, contactInfo);
                    userService.CreateAddress(userInfo, model.FactAddress);
                    userService.CreateAddress(userInfo, model.LegalAddress);
                    userService.CreateAddress(userInfo, model.PlaceOfBirth);
                    StringBuilder message = new StringBuilder("ваш логин:  " + user.UserName + "  ", 120);
                    message.AppendLine("ваш пароль:" + model.UserInfo.Password);
                    await SendMessage("Регистрация в интернет банкинге", message.ToString(), user);
                    return RedirectToAction("UserAccountCreate", "Account", new { userId = user.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            var countries = selectListService.GetCountries();
            model.UserInfo.Countries = countries;
            model.FactAddress.Countries = countries;
            model.LegalAddress.Countries = countries;
            model.PlaceOfBirth.Countries = countries;
            model.PassportInfo.TypeOfDocuments = selectListService.GetTypeOfDocuments();
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterEmployeeCompany(int companyId)
        {


            RegisterEmployeeViewModel employee = new RegisterEmployeeViewModel { CompanyId = companyId };
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployeeCompany(RegisterEmployeeViewModel model)
        {
            model.Password = generatePasswordService.CreatePassword();
            model.PasswordConfirm = model.Password;
            if (ModelState.IsValid)
            {



                User user = new User { Email = model.Email, UserName = userService.CreateLogin(model), IsPasswordChanged = false, IsTwoFactorOn = true };
                var result = await employeeService.CreateEmployee(user, model);

                if (result.Succeeded)
                {

                    employeeService.CreateEmployeeInfo(user, model);
                    StringBuilder message = new StringBuilder("ваш логин:  " + user.UserName + "  ",120);;
                    message.AppendLine(" ваш пароль:" + model.Password);
                    await SendMessage("Регистрация в интернет банкинге", message.ToString(), user);
                    return RedirectToAction("CompanyInfo", "Company", new {id = model.CompanyId});

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            HttpContext.Session.SetString("AnswerToSend","");
       

            if (ModelState.IsValid)
            {
                User userLogin = await _userManager.FindByNameAsync(model.Name);
               
                if (userLogin != null)
                {
                  

                    //var result = await _signInManager.CheckPasswordSignInAsync(userLogin, model.Password, false);
                    var result =  userService.CheckPasswordSignInAsync(userLogin, model.Password, false);
                    if (result.Succeeded)
                    {
                        userLogin.LoginAttemptsCount = 0;
                        await _userManager.UpdateAsync(userLogin);
                        string stringToEncrypt = StringCipher.Encrypt(model.Password); 
                        HttpContext.Session.SetString("EncryptingPassword", stringToEncrypt);
                        if (userLogin.IsTwoFactorOn)
                        {
                            Random rnd = new Random();
                            int code = rnd.Next(1000, 9999);
                            string subject = "Двухфакторная авторизация";
                            StringBuilder message = new StringBuilder("введите данный код для входа : " + code, 120);

                            await SendMessage(subject, message.ToString(), userLogin);
                            DateTime sendCodeTime = DateTime.Now;


                            ViewBag.UserId = userLogin.Id;
                            ViewBag.Code = code;
                            ViewBag.SendCodeTime = sendCodeTime;


                            return View("CodeConfirm");
                        }
                        else
                        {
                            return RedirectToAction("SignIn", "Manage",
                            new {userNameSignIn = userLogin.UserName});
                        }
                    }
                    if (result.IsLockedOut)
                    {
                        return Content("Ваш аккаунт заблокирован");
                    }
                    else
                    {
                        return RedirectToAction("LoginAttempts", new {userLoginEmail = model.Name});

                    }
                }
            }

            else
            {
                    ModelState.AddModelError(string.Empty,"Неправильный логин или пароль");
            }
            return View(model);
        }

        public async Task<IActionResult> SendMessage(string subject, string message, User user)
        {

            try
            {
                await emailService.SendEmailAsync(user.Email, subject, message.ToString());
                return Content("Сообщение отправлено");
            }
            catch (Exception e)
            {
                return Content("Невозможно отправить сообщение.Неверно указан email");
            };
        }

        public async Task<IActionResult> LoginAttempts(string userLoginEmail)
        {

            User userLogin = await _userManager.FindByNameAsync(userLoginEmail);
            userLogin.LoginAttemptsCount = userLogin.LoginAttemptsCount + 1;
            await _userManager.UpdateAsync(userLogin);
           
            if (userLogin.LoginAttemptsCount >= 3)
            {
                userLogin.IsBlocked = true;
                await _userManager.UpdateAsync(userLogin);
                return Content(
                    "Ваш аккаунт заблокирован.Для разблокировки обратитесь к администрации интернет-банкинга");

            }

            if (userLogin.LoginAttemptsCount >= 1)
            {
                string loginAttempMessage =
                    " Неверный пароль. Максимальное количество попыток входа 3. Осталось 2 попытки";
                HttpContext.Session.SetString("AnswerToSend", loginAttempMessage);
            }
            if (userLogin.LoginAttemptsCount >= 2)
            {
                string loginAttempMessage =
                    " Неверный пароль. Максимальное количество попыток входа 3. Осталась 1 попытка";
                HttpContext.Session.SetString("AnswerToSend", loginAttempMessage);
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            HttpContext.Session = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}

