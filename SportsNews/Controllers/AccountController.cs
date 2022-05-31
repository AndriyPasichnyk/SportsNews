using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SportsNews.Models;
using SportsNews.Data;
using System.IO;

namespace SportsNews.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LayoutViewModel<LoginViewModel>(new LoginViewModel(), "Log in to Sports News"));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LayoutViewModel<LoginViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var login = model.PageModel;
            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Login", "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View(new LayoutViewModel<RegisterViewModel>(new RegisterViewModel(), "Create Account"));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(LayoutViewModel<RegisterViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.PageModel.Email,
                Email = model.PageModel.Email
            };
            var claimFN = new Claim("First Name", model.PageModel.FirstName);
            var claimLN = new Claim("Last Name", model.PageModel.LastName);

            var result = await this.userManager.CreateAsync(user, model.PageModel.Password);

            if (result.Succeeded)
            {
                await this.userManager.AddClaimAsync(user, claimFN);
                await this.userManager.AddClaimAsync(user, claimLN);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegisterUser", error.Description);
                }
                return View(model);
            }
        }

        public ActionResult PersonalInfo()
        {
            //var membershipTypes = dbContext.

            var model = new UserInfoViewModel()
            {
                Email = User.Identity.Name,
                FirstName = User.Claims.FirstOrDefault(x => x.Type == "First Name")?.Value ?? String.Empty,
                LastName = User.Claims.FirstOrDefault(x => x.Type == "Last Name")?.Value ?? String.Empty
              //  Image = 
            };
            return View(new LayoutViewModel<UserInfoViewModel>(model, "Personal Info"));
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(LayoutViewModel<UserInfoViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string base64String;
            using (MemoryStream m = new MemoryStream())
            {
                await model.PageModel.ProfileImage.CopyToAsync(m);
                byte[] imageBytes = m.ToArray();

                base64String = Convert.ToBase64String(imageBytes);
            }

            var user = await this.userManager.GetUserAsync(User);
            if (user.Email != User.Identity.Name)
            {
                user.Email = User.Identity.Name;
            }

            var claimFN = new Claim("First Name", model.PageModel.FirstName);
            var claimLN = new Claim("Last Name", model.PageModel.LastName);
            var claimPP = new Claim("Profile Picture", base64String);

            var result = await this.userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var res1 = await this.userManager.ReplaceClaimAsync(user, User.Claims.FirstOrDefault(x => x.Type == "First Name"), claimFN);
                var res2 = await this.userManager.ReplaceClaimAsync(user, User.Claims.FirstOrDefault(x => x.Type == "Last Name"), claimLN);
                
                //var claimPicture = User.Claims.FirstOrDefault(x => x.Type == "Profile Picture");
                //if (claimPicture != null)
                //{ 
                //    var res3 = await this.userManager.ReplaceClaimAsync(user, claimPicture, claimPP); 
                //}
                //else
                //{
                //    await this.userManager.AddClaimAsync(user, claimPP);
                //}

                await this.signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("PersonalInfo", error.Description);
                }
                return View(model);
            }
        }

        public ActionResult UserPassword()
        {
            return View(new LayoutViewModel("Change Password"));
        }

        public ActionResult UserSurveys()
        {
            return View(new LayoutViewModel("My Surveys"));
        }

        public ActionResult UserTeamHub()
        {
            return View(new LayoutViewModel("Team Hub"));
        }
    }
}