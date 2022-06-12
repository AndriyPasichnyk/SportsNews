using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class TeamRepository : IItemRepository<Team>
    {
        internal ApplicationDbContext applicationDbContext;

        public TeamRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Team> GetItems()
        {
            return this.applicationDbContext.Teams.ToList();
        }

        public IEnumerable<Team> GetItemsBySubCategoryId(int id)
        {
            return this.applicationDbContext.Teams.Where(t => t.SubCategoryId == id).ToList();
        }

        public Team GetItemByID(int id)
        {
            return this.applicationDbContext.Teams.FirstOrDefault(u => u.Id == id);
        }

        public void InsertItem(Team item)
        {
            this.applicationDbContext.Teams.Add(item);
        }

        public void UpdateItem(Team item)
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
            this.applicationDbContext.Teams.Remove(item);
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
