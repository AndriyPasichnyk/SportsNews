using Microsoft.EntityFrameworkCore;
using SportsNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    public class TeamBadgesRepository : IItemRepository<TeamBadge>
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TeamBadgesRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<TeamBadge> GetItems()
        {
            return this.applicationDbContext.TeamBadges.ToList();
        }

        public TeamBadge GetItemByID(int id)
        {
            return this.applicationDbContext.TeamBadges.Where(b => b.Id == id).FirstOrDefault();
        }

        public TeamBadge FindItemByID(int id)
        {
            return this.applicationDbContext.TeamBadges.Include("Team").Where(b => b.Team.Id == id).FirstOrDefault();
        }

        public void InsertItem(TeamBadge item)
        {
            this.applicationDbContext.TeamBadges.Add(item);
        }

        public void UpdateItem(TeamBadge item)
        {
            var tItem = GetItemByID(item.Id);

            if (tItem != null)
            {
                tItem.Badge = item.Badge;
            }
            else
            {
                InsertItem(item);
            }
        }

        public void DeleteItem(int id)
        {
            var item = GetItemByID(id);
            this.applicationDbContext.TeamBadges.Remove(item);
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
