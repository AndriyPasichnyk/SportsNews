using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class TeamLocationsRepository: IItemRepository<Location>
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TeamLocationsRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Location> GetItems()
        {
            return this.applicationDbContext.Locations.ToList();
        }

        public Location GetItemByID(int id)
        {
            return this.applicationDbContext.Locations.Where(b => b.Id == id).FirstOrDefault();
        }

        public Location FindItemByName(string name)
        {
            return this.applicationDbContext.Locations.Where(n => n.FullName == name).FirstOrDefault();
        }

        public void InsertItem(Location item)
        {
            this.applicationDbContext.Locations.Add(item);
        }

        public void UpdateItem(Location item)
        {
            var tItem = GetItemByID(item.Id);

            if (tItem != null)
            {
                tItem.FullName = item.FullName;
            }
            else
            {
                InsertItem(item);
            }
        }

        public void DeleteItem(int id)
        {
            var item = GetItemByID(id);
            this.applicationDbContext.Locations.Remove(item);
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

