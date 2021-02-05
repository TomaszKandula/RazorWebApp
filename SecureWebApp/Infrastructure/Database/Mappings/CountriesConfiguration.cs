using Microsoft.EntityFrameworkCore;
using SecureWebApp.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecureWebApp.Infrastructure.Database.Mappings
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
