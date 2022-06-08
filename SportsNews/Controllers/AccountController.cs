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
        private readonly UserPhotoUnitOfWork userPhotoUnitOfWork;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, UserPhotoUnitOfWork userPhotoUnitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userPhotoUnitOfWork = userPhotoUnitOfWork;
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
            var userId = Guid.Parse(this.userManager.GetUserId(User));
            var model = new UserInfoViewModel()
            {
                Email = User.Identity.Name,
                FirstName = User.Claims.FirstOrDefault(x => x.Type == "First Name")?.Value ?? String.Empty,
                LastName = User.Claims.FirstOrDefault(x => x.Type == "Last Name")?.Value ?? String.Empty,
                Image = userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserId(userId)?.ProfilePicture ?? Array.Empty<byte>()
            };
            return View(new LayoutViewModel<UserInfoViewModel>(model, "Personal Info", false, model.Image));
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(LayoutViewModel<UserInfoViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] imageBytes = await ConvertFileToByteArray(model.PageModel.ProfileImage);

            var user = await this.userManager.GetUserAsync(User);
            if (user.Email != model.PageModel.Email)
            {
                user.Email = model.PageModel.Email;
                user.UserName = model.PageModel.Email;
            }

            var claimFirstName = new Claim("First Name", model.PageModel.FirstName);
            var claimLastName = new Claim("Last Name", model.PageModel.LastName);

            var result = await this.userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var res1 = await CreateOrReplaceClaim(user, claimFirstName);
                var res2 = await CreateOrReplaceClaim(user, claimLastName);
                this.userPhotoUnitOfWork.UserPhotos.UpdateUserPhoto(new UserPhoto
                {
                    UserId = Guid.Parse(user.Id),
                    ProfilePicture = imageBytes
                });
                await this.userPhotoUnitOfWork.SaveAsync();

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
            var userId = Guid.Parse(this.userManager.GetUserId(User));
            return View(new LayoutViewModel("Change Password", false, userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserId(userId)?.ProfilePicture));
        }

        [HttpGet]
        public ActionResult UserSurveys()
        {
            var userId = Guid.Parse(this.userManager.GetUserId(User));
            return View(new LayoutViewModel("My Surveys", false, userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserId(userId)?.ProfilePicture));
        }

        [HttpGet]
        public ActionResult UserTeamHub()
        {
            var userId = Guid.Parse(this.userManager.GetUserId(User));
            return View(new LayoutViewModel("Team Hub", false, userPhotoUnitOfWork.UserPhotos.GetUserPhotoByUserId(userId)?.ProfilePicture));
        }



        private async Task<IdentityResult> CreateOrReplaceClaim(IdentityUser user, Claim claim)
        {
            IdentityResult result;
            var claimforReplace = User.Claims.FirstOrDefault(x => x.Type == claim.Type);
            if (claim != null)
            {
                result = await this.userManager.ReplaceClaimAsync(user, claimforReplace, claim);
            }
            else
            {
                result = await this.userManager.AddClaimAsync(user, claim);
            }
            return result;
        }

        private async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            string base64String = String.Empty;
            if (file != null)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    await file.CopyToAsync(m);
                    byte[] imageBytes = m.ToArray();
                    return imageBytes;
                }
            }
            return Array.Empty<byte>();
        }
    }
}