using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class InfoArchitectureUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext applicationDbContext;
        private CategoryRepository categoryRepository;
        private SubCategoryRepository subCategoryRepository;
        private TeamRepository teamRepository;

        public InfoArchitectureUnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;

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
