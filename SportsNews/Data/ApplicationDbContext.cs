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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seeding definition by Configuration - need migration for this
            //builder.ApplyConfiguration(new LanguageConfiguration());

        }

        // for User Profile images
        public DbSet<UserPhoto> UserPhotos { get; set; }
        // common
        public DbSet<Language> Languages { get; set; }

        // for Articles
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<TeamBadge> TeamBadges { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TeamLocation> TeamLocations { get; set; }

        // for Menus
        public DbSet<AdminMenu> AdminMenuItems { get; set; }



        //        public DbSet<SocialNetwork> SocialNetworks { get; set; }

    }
}
