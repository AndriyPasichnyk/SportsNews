using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SportsNews.Models;
using SportsNews.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using SportsNews.Data.Models;

namespace SportsNews.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext applicationDbContext;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
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

        [HttpGet]
        public ActionResult PersonalInfo()
        {
            var model = new UserInfoViewModel()
            {
                Email = User.Identity.Name,
                FirstName = User.Claims.FirstOrDefault(x => x.Type == "First Name")?.Value ?? String.Empty,
                LastName = User.Claims.FirstOrDefault(x => x.Type == "Last Name")?.Value ?? String.Empty,
                Image = UserInfoHelper.GetUserImage(applicationDbContext, User)
            };
            return View(new LayoutViewModel<UserInfoViewModel>(model, "Personal Info", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(LayoutViewModel<UserInfoViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string base64String = await ConvertFileToString(model.PageModel.ProfileImage);

            var user = await this.userManager.GetUserAsync(User);
            if (user.Email != User.Identity.Name)
            {
                user.Email = User.Identity.Name;
            }

            var claimFirstName = new Claim("First Name", model.PageModel.FirstName);
            var claimLastName = new Claim("Last Name", model.PageModel.LastName);

            var result = await this.userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var res1 = await CreateOrReplaceClaim(user, "First Name", claimFirstName);
                var res2 = await CreateOrReplaceClaim(user, "Last Name", claimLastName);
                SaveUserImage(base64String);

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


        [HttpGet]
        public ActionResult UserPassword()
        {
            return View(new LayoutViewModel("Change Password", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }

        [HttpGet]
        public ActionResult UserSurveys()
        {
            return View(new LayoutViewModel("My Surveys", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }

        [HttpGet]
        public ActionResult UserTeamHub()
        {
            return View(new LayoutViewModel("Team Hub", false, UserInfoHelper.GetUserImage(applicationDbContext, User)));
        }



        private async Task<IdentityResult> CreateOrReplaceClaim(IdentityUser user, string claimName, Claim newClaim)
        {
            IdentityResult result;
            var claim = User.Claims.FirstOrDefault(x => x.Type == claimName);
            if (claim != null)
            {
                result = await this.userManager.ReplaceClaimAsync(user, claim, newClaim);
            }
            else
            {
                result = await this.userManager.AddClaimAsync(user, newClaim);
            }
            return result;
        }
        private async Task<string> ConvertFileToString(IFormFile file)
        {
            string base64String = String.Empty;
            if (file != null)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    await file.CopyToAsync(m);
                    byte[] imageBytes = m.ToArray();

                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            return base64String;
        }
        private void SaveUserImage(string picture)
        {
            var userId = this.userManager.GetUserId(User);
            var userPhoto = this.applicationDbContext.UserPhotos.FirstOrDefault(u => u.UserId == userId);

            if (userPhoto != null)
            {
                userPhoto.ProfilePicture = picture;
            }
            else
            {
                this.applicationDbContext.UserPhotos.Add(new UserPhoto
                {
                    UserId = userId,
                    ProfilePicture = picture
                });
            }
            this.applicationDbContext.SaveChanges();
        }

    }
}