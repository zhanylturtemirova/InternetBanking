using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.EntityFrameworkCore;


namespace InternetBanking.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService emailService;
        private ApplicationContext context;
        private readonly GeneratePasswordService generatePasswordService;
      
        public ManageController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            this.context = context;
            _signInManager = signInManager;
            emailService = new EmailService();
            generatePasswordService = new GeneratePasswordService();
        }



        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!IsInBlacklist(model.NewPassword))
            {

                if (ModelState.IsValid)
                 {

                
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        IdentityResult result =
                            await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            user.IsPasswordChanged = true;
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ненадежный пароль");
                }
            }
            return View(model);
        }

        [AllowAnonymous]

        public ActionResult CheckPasswordBlackList(string NewPassword)
        {
            try
            {
                return Json(!IsInBlacklist(NewPassword));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        private bool IsInBlacklist(string NewPassword)
        {
            BlackList blackList = context.BlackListedPasswords.FirstOrDefault(u=>u.BlackListedPassword==NewPassword);
            if (blackList==null)
            {
                return false;
            }
            return true;
        }
      

        [HttpPost]
        public async Task<IActionResult> TwoFactorAuth(TwoFactorAuthViewModel model)
        {

          
            if (ModelState.IsValid)
            {
                if (model.Code == model.CodeConfirm)
                {
                    if (DateTime.Now.Minute - model.SendCodeTime.Minute > 5)
                    {
                        string codeTimeOverMessage = "Время действия кода истекло.";
                        HttpContext.Session.SetString("AnswerToSend", codeTimeOverMessage);
                        return RedirectToAction("Login", "User");

                    }
                    User user = await _userManager.FindByIdAsync(model.UserId);
                    return RedirectToAction("SignIn", "Manage", new { UserNameSignIn = user.UserName });

                }
                else
                {
                    string codeTimeOverMessage = "Неверный код";
                    HttpContext.Session.SetString("AnswerToSend", codeTimeOverMessage);
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Неверный код");

            }
            return View(model);
  
        }



        public async Task<IActionResult> SendMessage(string subject,string message, User user)
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


        public async Task<IActionResult> SignIn(string userNameSignIn)
        {
            string encryptedPassword = HttpContext.Session.GetString("EncryptingPassword");
            string stringToDencrypt = StringCipher.Decrypt(encryptedPassword);
            User userSignIn = await _userManager.FindByNameAsync(userNameSignIn);
            var result =
                await _signInManager.PasswordSignInAsync(userNameSignIn, stringToDencrypt, false, false);
            if (result.Succeeded)
            {
                if (userSignIn.IsBlocked == true)
                {
                    return Content("Ваш аккаунт заблокирован. Обратитесь к администрации сайта.");
                }
                userSignIn.LoginAttemptsCount = 0;
                await _userManager.UpdateAsync(userSignIn);
                if (!userSignIn.IsPasswordChanged)
                {
                    return RedirectToAction("ChangePassword", "Manage", new { id = userSignIn.Id });
                }
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Login", "User");

        }

        public static string MakeExpiryHash(DateTime expiry)
        {
            const string salt = "some random bytes";
            byte[] bytes = Encoding.UTF8.GetBytes(salt + expiry.ToString("s"));
            using (var sha = System.Security.Cryptography.SHA1.Create())
                return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2"))).Substring(8);
        }

        public async Task<IActionResult> SendMessageToChangePassword(string userEmail)
        {
           // string referer = Request.Headers["Referer"].ToString();
            User user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {

                //DateTime expires = DateTime.Now + TimeSpan.FromDays(1);
                //string hash = MakeExpiryHash(expires);
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                user.userSendEmailToken = token;
                await _userManager.UpdateAsync(user);
                var callbackUrl = Url.Action(nameof(ChangePasswordIfForgot), "Manage", new { userId = user.Id, sentToken=token}, protocol: HttpContext.Request.Scheme);
                string message = "Для смены пароля пройдите  <a href=\"" + callbackUrl + "\">по ссылке </a>";
                string subject = "Смена пароля";
                await SendMessage(subject, message, user);
                
                    
                string answerMessage = "Сообщение отправлено";

                HttpContext.Session.SetString("AnswerToSend", answerMessage);
                return RedirectToAction("Login", "User");
            }
            else
            {
                string answerMessage = "Сообщение не отправлено";
                HttpContext.Session.SetString("AnswerToSend", answerMessage);
                return RedirectToAction("Login", "User");
            }
          
        }

        public async Task<IActionResult> AdminSendMessageToChangePassword(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            user.userSendEmailToken = token;
            await _userManager.UpdateAsync(user);

            string referer = Request.Headers["Referer"].ToString();
            if (user != null)
            {
                var callbackUrl = Url.Action(nameof(ChangePasswordIfForgot), "Manage", new { userId = user.Id , sentToken = token}, protocol: HttpContext.Request.Scheme);
              

                string message = "Ваш логин:" + user.UserName + "\n Для смены пароля пройдите  <a href=\"" + callbackUrl + "\">по ссылке </a>";
                string subject = "Смена пароля";
                await SendMessage(subject, message, user);
                string answerMessage = "Сообщение отправлено";

                TempData["AnswerToSend"] = answerMessage;
                return Redirect(referer);
            }
            else
            {
                string answerMessage = "Неверный Электронный адрес";
                TempData["AnswerToSend"] = answerMessage;
                return Redirect(referer);
            }
           
        }
        public async Task<IActionResult> ChangePasswordIfForgot(string userId,string sentToken)
        {
            
            User user = await _userManager.FindByIdAsync(userId);
           
            if (user == null)
            {
                return NotFound();
            }
            if (user.userSendEmailToken == sentToken)
            {
                ForgotPasswordViewModel model = new ForgotPasswordViewModel {Id = user.Id};
                user.userSendEmailToken = string.Empty;
                await _userManager.UpdateAsync(user);
                return View(model);

            }
            else
            {
                return Content("ссылка более не действительна");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordIfForgot(ForgotPasswordViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);

            if (!IsInBlacklist(model.NewPassword))
            {
                if (ModelState.IsValid)
                  {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    IdentityResult passwordChangeResult =
                        await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
                    if (passwordChangeResult.Succeeded)
                    {
                        user.IsPasswordChanged = true;
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пароли не совпадают");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ненадежный пароль");
            }
            return View(model);
        }

        public async Task<IActionResult> CheckEmail ()
        {
            CheckEmailViewModel model = new CheckEmailViewModel();
          
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> CheckEmail(CheckEmailViewModel model)
        {
            User user = await _userManager.FindByEmailAsync(model.EmailConfirm);
            if (user == null)
            {
                return NotFound("");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("SendMessageToChangePassword",new{userEmail=model.EmailConfirm });
                }
                else
                {
                   ModelState.AddModelError(string.Empty, "Неверный Email");
                }
            }
            
            return View(model);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}