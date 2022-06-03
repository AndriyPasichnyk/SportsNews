using SportsNews.Data.Models;
using System;
using System.Collections.Generic;

namespace SportsNews.Data
{
    public interface IUserPhotoRepository : IDisposable
    {
        IEnumerable<UserPhoto> GetUserPhotos();
        UserPhoto GetUserPhotoByID(int userPhotoId);
        UserPhoto GetUserPhotoByUserId(Guid userId);
        UserPhoto GetUserPhotoByUserName(string userName);

        void InsertUserPhoto(UserPhoto UserPhoto);
        void UpdateUserPhoto(UserPhoto userPhoto);
        void DeleteUserPhoto(int userPhotoID);
    }
}
