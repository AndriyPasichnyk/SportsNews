using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class AdminMenuRepository : IItemRepository<AdminMenu>
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdminMenuRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<AdminMenu> GetItems()
        {
            return this.applicationDbContext.AdminMenuItems.ToList();
        }

        public AdminMenu GetItemByID(int id)
        {
            return this.applicationDbContext.AdminMenuItems.FirstOrDefault(u => u.Id == id);
        }

        public void InsertItem(AdminMenu item)
        {
            this.applicationDbContext.AdminMenuItems.Add(item);
        }

        public void UpdateItem(AdminMenu item)
        {
            var tItem = GetItemByID(item.Id);

            if (tItem != null)
            {
                tItem.LanguageId = item.LanguageId;
                tItem.Title = item.Title;
                tItem.Action = item.Action;
            }
            else
            {
                InsertItem(item);
            }
        }

        public void DeleteItem(int id)
        {
            var item = GetItemByID(id);
            this.applicationDbContext.AdminMenuItems.Remove(item);
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