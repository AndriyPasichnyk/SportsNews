using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace SportsNews.Data
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager, configuration);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(Roles.Administrator).Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = Roles.Administrator
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Roles.User).Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = Roles.User
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            if (configuration.GetSection(AdminConfig.Name).Exists())
            {
                if (userManager.FindByNameAsync(configuration[AdminConfig.UserEmail]).Result == null)
                {
                    IdentityUser user = new IdentityUser()
                    {
                        UserName = configuration[AdminConfig.UserEmail],
                        Email = configuration[AdminConfig.UserEmail]
                    };

                    IdentityResult result = userManager.CreateAsync(user, configuration[AdminConfig.Password]).Result;
 
                    if (result.Succeeded)
                    {
                        userManager.AddClaimsAsync(user, new List<Claim> {
                            new Claim(Claims.FirstName, configuration[Claims.FirstNameConfig]),
                            new Claim(Claims.LastName, configuration[Claims.LastNameConfig])
                        }).Wait(); 
                        userManager.AddToRoleAsync(user, Roles.Administrator).Wait();
                    }
                }
            }
        }
    }
}
