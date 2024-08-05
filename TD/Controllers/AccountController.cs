using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TD.Models;
using System.Threading.Tasks;
using TD.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TD.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<TDUser> _userManager;
        private readonly SignInManager<TDUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<TDUser> userManager, SignInManager<TDUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var result = await model.RegisterAsync(_userManager, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            {
                var result = await model.LoginAsync(_signInManager);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SendEmail()
     
        {
            await _emailSender.SendEmailAsync("example@example.com", "Subject", "Message");
            return View();
        }
    }
}
