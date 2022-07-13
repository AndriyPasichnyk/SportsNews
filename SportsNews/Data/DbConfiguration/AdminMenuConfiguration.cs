using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsNews.Data.Models;

namespace SportsNews.Data.DbConfiguration
{
    public class AdminMenuConfiguration : IEntityTypeConfiguration<AdminMenu>
    {
        public void Configure(EntityTypeBuilder<AdminMenu> builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
