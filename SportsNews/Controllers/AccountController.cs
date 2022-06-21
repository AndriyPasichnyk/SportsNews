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
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace SportsNews.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService sender;
        private readonly IEnumerable<Language> languages;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, IEmailService sender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.sender = sender;
            this.languages = this.unitOfWork.Languages.GetItems().ToList();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LayoutViewModel<LoginViewModel>(new LoginViewModel(), "LogInAccount"));
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
            return View(new LayoutViewModel<RegisterViewModel>(new RegisterViewModel(), "CreateAccount"));
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
            var claimFN = new Claim(Claims.FirstName, model.PageModel.FirstName);
            var claimLN = new Claim(Claims.LastName, model.PageModel.LastName);

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
            var innerModel = new UserInfoViewModel()
            {
                Email = User.Identity.Name,
                FirstName = User.Claims.FirstOrDefault(x => x.Type == Claims.FirstName)?.Value ?? String.Empty,
                LastName = User.Claims.FirstOrDefault(x => x.Type == Claims.LastName)?.Value ?? String.Empty,
                Image = unitOfWork.UsersPhoto.GetUserPhotoByUserId(userId)?.ProfilePicture ?? Array.Empty<byte>()
            };
            var model = new LayoutViewModel<UserInfoViewModel>(innerModel, "Personal Info", false, innerModel.Image)
            {
                Languages = this.languages
            };

            return View(model);
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

            var claimFirstName = new Claim(Claims.FirstName, model.PageModel.FirstName);
            var claimLastName = new Claim(Claims.LastName, model.PageModel.LastName);

            var result = await this.userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var res1 = await CreateOrReplaceClaim(user, claimFirstName);
                var res2 = await CreateOrReplaceClaim(user, claimLastName);
                this.unitOfWork.UsersPhoto.UpdateUserPhoto(new UserPhoto
                {
                    UserId = Guid.Parse(user.Id),
                    ProfilePicture = imageBytes
                });
                await this.unitOfWork.SaveAsync();

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
            return View(new LayoutViewModel("Change Password", false, unitOfWork.UsersPhoto.GetUserPhotoByUserId(userId)?.ProfilePicture));
        }

        [HttpGet]
        public ActionResult UserSurveys()
        {
            var userId = Guid.Parse(this.userManager.GetUserId(User));
            return View(new LayoutViewModel("My Surveys", false, unitOfWork.UsersPhoto.GetUserPhotoByUserId(userId)?.ProfilePicture));
        }

        [HttpGet]
        public ActionResult UserTeamHub()
        {
            var userId = Guid.Parse(this.userManager.GetUserId(User));
            return View(new LayoutViewModel("Team Hub", false, unitOfWork.UsersPhoto.GetUserPhotoByUserId(userId)?.ProfilePicture));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new LayoutViewModel<ForgotPasswordViewModel>(new ForgotPasswordViewModel(), "ForgotPassword"));
        }       

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(LayoutViewModel<ForgotPasswordViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = this.userManager.FindByEmailAsync(model.PageModel.Email).Result;
            if (user == null)
            {
                ModelState.AddModelError("ForgotPassword", "There are no user with this email!!!");
                return View(model);
            }

            //TODO: Snnd e-mail
            var userId = await this.userManager.GetUserIdAsync(user);
            var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);
            await sender.SendEmailAsync(model.PageModel.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View(new LayoutViewModel<ForgotPasswordViewModel>(new ForgotPasswordViewModel(), "ForgotPasswordConfirmation"));
        }

        [HttpGet]
        public ActionResult ResetPassword(string userId, string code)
        {
            if (code == null)
            {
                return View("Error");  
            }

            return View(new LayoutViewModel<ResetPasswordViewModel>(new ResetPasswordViewModel() { 
                UserId = Guid.Parse(userId),
                Code = code
            }, "ResetPassword"));
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(LayoutViewModel<ResetPasswordViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(model.PageModel.UserId.ToString());
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("RegisterUser", "Account");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.PageModel.Code));
            var result = await userManager.ResetPasswordAsync(user, code, model.PageModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
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