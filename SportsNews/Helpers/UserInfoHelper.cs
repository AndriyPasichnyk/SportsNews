using Microsoft.AspNetCore.Identity;
using SportsNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportsNews
{
    public static class UserInfoHelper
    {

        public static byte[] GetUserImage(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)  
        {
            if (user.Identity.IsAuthenticated)
            {
                var userId = applicationDbContext.Users.FirstOrDefault(u => u.UserName == user.Identity.Name)?.Id ?? String.Empty;
                var picture = applicationDbContext.UserPhotos.FirstOrDefault(u => u.UserId == Guid.Parse(userId))?.ProfilePicture;

                return picture!=null ? picture : Array.Empty<byte>();
            }
            else
            {
                return Array.Empty<byte>();
            }
        }
    }
}
