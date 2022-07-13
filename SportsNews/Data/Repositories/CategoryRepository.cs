using Microsoft.EntityFrameworkCore;
using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class CategoryRepository : IItemRepository<Category>
    {
        internal ApplicationDbContext applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Category> GetItems()
        {
            return applicationDbContext.Categories.Include("Subcategories.Teams").ToList();
        }

        public Category GetItemByID(int id)
        {
            return this.applicationDbContext.Categories.Include("Subcategories.Teams").FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<Category> GetItemsByID(int id)
        {
            return this.applicationDbContext.Categories.Where(u => u.Id == id).Include("Subcategories.Teams").ToList();
        }

        public void InsertItem(Category item)
        {
            this.applicationDbContext.Categories.Add(item);
        }

        public void UpdateItem(Category item)
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
            this.applicationDbContext.Categories.Remove(item);
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
