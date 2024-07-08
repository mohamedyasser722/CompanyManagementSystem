using CompanyManagementSystem.DAL.Models;
using CompanyManagementSystem.PL.Utilities.EmailHelper;
using CompanyManagementSystem.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = model.Email.Split('@')[0]; // Extract username from email

                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        #endregion


        #region LogIN/OUT
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email address you entered is not registered.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "The password you entered is incorrect.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        } 
        #endregion


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
				var user = await  _userManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
					ModelState.AddModelError(string.Empty, "The email address you entered is not registered.");
					return View("ForgetPassword", model);
				}

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var ResetPassWordLink = Url.Action("ResetPassword", "Account", new { email = model.Email,Token = token }, Request.Scheme);

                var email = new Email()
                {
                    Subject = "Reset Password",
                    To = model.Email,
                    Body = ResetPassWordLink!
				};
                //EmailSender.SendEmail(email);
                _emailService.SendEmail(email);
                return RedirectToAction(nameof(CheckYourInbox));
			}
            return View("ForgetPassword", model);

        }

        public IActionResult CheckYourInbox()
        {
			return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
		}
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
			if(ModelState.IsValid)
            {
                string email = TempData["Email"] as string;
                string token = TempData["Token"] as string;

                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if(result.Succeeded)
                {
					return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
					ModelState.AddModelError(string.Empty, error.Description);
                }
			}
			return View(model);    
        }


    }

    








}

