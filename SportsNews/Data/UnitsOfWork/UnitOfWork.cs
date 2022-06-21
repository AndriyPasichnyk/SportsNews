using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext applicationDbContext;

        private CategoryRepository categoryRepository;
        private SubCategoryRepository subCategoryRepository;
        private TeamRepository teamRepository;
        private IUserPhotoRepository userPhotoRepository;
        private AdminMenuRepository adminMenuRepository;
        private LanguageRepository languageRepository;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public AdminMenuRepository AdminMenu
        {
            get
            {
                if (this.adminMenuRepository == null)
                {
                    this.adminMenuRepository = new AdminMenuRepository(this.applicationDbContext);
                }
                return this.adminMenuRepository;
            }
        }

        public IUserPhotoRepository UsersPhoto
        {
            get
            {
                if (this.userPhotoRepository == null)
                {
                    this.userPhotoRepository = new UserPhotoRepository(this.applicationDbContext);
                }
                return this.userPhotoRepository;
            }
        }

        public CategoryRepository Categories
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new CategoryRepository(applicationDbContext);
                }
                return categoryRepository;
            }
        }

        public SubCategoryRepository SubCategories
        {
            get
            {
                if (this.subCategoryRepository == null)
                {
                    this.subCategoryRepository = new SubCategoryRepository(applicationDbContext);
                }
                return subCategoryRepository;
            }
        }

        public TeamRepository Teams
        {
            get
            {
                if (this.teamRepository == null)
                {
                    this.teamRepository = new TeamRepository(applicationDbContext);
                }
                return teamRepository;
            }
        }

        public LanguageRepository Languages
        {
            get
            {
                if (this.languageRepository == null)
                {
                    this.languageRepository = new LanguageRepository(applicationDbContext);
                }
                return languageRepository;
            }
        }

        public void Save()
        {
            this.applicationDbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await this.applicationDbContext.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            this.applicationDbContext.Dispose();
        }
    }
}
