using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class UserPhotoRepository : IUserPhotoRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserPhotoRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<UserPhoto> GetUserPhotos()
        {
            return this.applicationDbContext.UserPhotos.ToList();
        }

        public UserPhoto GetUserPhotoByID(int userPhotoId)
        {
            return this.applicationDbContext.UserPhotos.FirstOrDefault(u => u.Id == userPhotoId);
        }
        
        public UserPhoto GetUserPhotoByUserId(Guid userId)
        {
            return this.applicationDbContext.UserPhotos.FirstOrDefault(u => u.UserId == userId);
        }

        public UserPhoto GetUserPhotoByUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = this.applicationDbContext.Users.FirstOrDefault(u => u.UserName == userName);
                return this.applicationDbContext.UserPhotos.FirstOrDefault(u => u.UserId == Guid.Parse(user.Id));
            }
            return null;
        }

        public void InsertUserPhoto(UserPhoto userPhoto)
        {
            this.applicationDbContext.UserPhotos.Add(userPhoto);
        }

        public void UpdateUserPhoto(UserPhoto userPhoto)
        {
            var photo = GetUserPhotoByUserId(userPhoto.UserId);

            if (photo != null)
            {
                photo.ProfilePicture = userPhoto.ProfilePicture;
            }
            else
            {
                InsertUserPhoto(userPhoto);
            }
        }

        public void DeleteUserPhoto(int userPhotoID)
        {
            UserPhoto userPhoto = this.applicationDbContext.UserPhotos.Find(userPhotoID);
            this.applicationDbContext.UserPhotos.Remove(userPhoto);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.applicationDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
