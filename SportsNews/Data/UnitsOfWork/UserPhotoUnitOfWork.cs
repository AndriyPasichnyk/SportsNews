using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class UserPhotoUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext applicationDbContext;
        private IUserPhotoRepository userPhotoRepository;

        public UserPhotoUnitOfWork(ApplicationDbContext applicationDbContext, IUserPhotoRepository userPhotoRepository)
        {
            this.applicationDbContext = applicationDbContext;
            this.userPhotoRepository = userPhotoRepository;
        }

        public IUserPhotoRepository UserPhotos
        {
            get
            {
                return this.userPhotoRepository;
            }
        }

        public void Dispose()
        {
            this.applicationDbContext.Dispose();
        }

        public void Save()
        {
            this.applicationDbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
