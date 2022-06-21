using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class LanguageRepository : IItemRepository<Language>
    {
        private readonly ApplicationDbContext applicationDbContext;

        public LanguageRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Language> GetItems()
        {
            return this.applicationDbContext.Languages.ToList();
        }

        public Language GetItemByID(int id)
        {
            return this.applicationDbContext.Languages.FirstOrDefault(l => l.Id == id);
        }

        public void InsertItem(Language item)
        {
            this.applicationDbContext.Languages.Add(item);
        }

        public void UpdateItem(Language item)
        {
            var tItem = GetItemByID(item.Id);

            if (tItem != null)
            {
                tItem.Name = item.Name;
                tItem.Abbreviation = item.Abbreviation;
                tItem.IsEnabled = item.IsEnabled;
            }
            else
            {
                InsertItem(item);
            }
        }

        public void DeleteItem(int id)
        {
            var item = GetItemByID(id);
            this.applicationDbContext.Languages.Remove(item);
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
