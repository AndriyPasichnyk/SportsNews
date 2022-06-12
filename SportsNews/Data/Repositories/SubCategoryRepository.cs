using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class SubCategoryRepository : IItemRepository<SubCategory>
    {
        internal ApplicationDbContext applicationDbContext;

        public SubCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<SubCategory> GetItems()
        {
            return this.applicationDbContext.SubCategories.ToList();
        }
        public IEnumerable<SubCategory> GetItemsByCategoryId(int id)
        {
            return this.applicationDbContext.SubCategories.Where(s => s.CategoryId == id).ToList();
        }

        public SubCategory GetItemByID(int id)
        {
            return this.applicationDbContext.SubCategories.FirstOrDefault(u => u.Id == id);
        }


        public void InsertItem(SubCategory item)
        {
            this.applicationDbContext.SubCategories.Add(item);
        }

        public void UpdateItem(SubCategory item)
        {
            var tItem = GetItemByID(item.Id);

            if (tItem != null)
            {
                tItem.Name = item.Name;
                tItem.IsVisible = item.IsVisible;
            }
            else
            {
                InsertItem(item);
            }
        }

        public void DeleteItem(int id)
        {
            var item = GetItemByID(id);
            this.applicationDbContext.SubCategories.Remove(item);
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
