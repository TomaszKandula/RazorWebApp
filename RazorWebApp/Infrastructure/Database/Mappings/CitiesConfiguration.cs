using Microsoft.EntityFrameworkCore;
using RazorWebApp.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RazorWebApp.Infrastructure.Database.Mappings
{
    public class CitiesConfiguration : IEntityTypeConfiguration<Cities>
    {
        public void Configure(EntityTypeBuilder<Cities> AEntityBuilder)
        {
            AEntityBuilder
                .Property(ACities => ACities.CityName)
                .IsRequired()
                .HasMaxLength(255);

            AEntityBuilder
                .HasOne(ACities => ACities.Country)
                .WithMany(ACountries => ACountries.Cities)
                .HasForeignKey(ACities => ACities.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CountryId__Cities");
        }
    }
}
