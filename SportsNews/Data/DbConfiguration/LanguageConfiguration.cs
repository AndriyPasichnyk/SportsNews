using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsNews.Data.Models;

namespace SportsNews.Data
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasData(
                    new Language() { Id = 1, Name = "English", Abbreviation = "EN", IsEnabled = true }
                );
        }
    }
}
