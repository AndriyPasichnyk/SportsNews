using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsNews.Data.Models;

namespace SportsNews.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // for Articles
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Article> Articles { get; set; }


        // common
        public DbSet<Language> Languages { get; set; }


        public DbSet<SocialNetwork> SocialNetworks { get; set; }
    }
}
