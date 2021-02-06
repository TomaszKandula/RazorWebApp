using Microsoft.EntityFrameworkCore;
using RazorWebApp.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RazorWebApp.Infrastructure.Database.Mappings
{
    public class CountriesConfiguration : IEntityTypeConfiguration<Countries>
    {
        public void Configure(EntityTypeBuilder<Countries> AEntityBuilder)
        {
            AEntityBuilder
                .Property(ACountries => ACountries.CountryCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            AEntityBuilder
                .Property(ACountries => ACountries.CountryName)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
