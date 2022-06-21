using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public interface IUnitOfWork : IDisposable
    {
        AdminMenuRepository AdminMenu { get; }
        IUserPhotoRepository UsersPhoto { get; }

        CategoryRepository Categories { get; }
        SubCategoryRepository SubCategories { get; }
        TeamRepository Teams { get; }

        LanguageRepository Languages { get; }

        void Save();
        Task SaveAsync();
    }
}
