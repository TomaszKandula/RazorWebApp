using Microsoft.EntityFrameworkCore;
using RazorWebApp.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RazorWebApp.Infrastructure.Database.Mappings
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> AEntityBuilder)
        {
            AEntityBuilder
                .HasIndex(AUsers => AUsers.EmailAddr)
                .HasName("UQ__EmailAddr__Users")
                .IsUnique();

            AEntityBuilder
                .Property(AUsers => AUsers.CreatedAt)
                .HasColumnType("datetime");

            AEntityBuilder
                .Property(AUsers => AUsers.EmailAddr)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            AEntityBuilder
                .Property(AUsers => AUsers.FirstName)
                .IsRequired()
                .HasMaxLength(255);

            AEntityBuilder
                .Property(AUsers => AUsers.LastName)
                .IsRequired()
                .HasMaxLength(255);

            AEntityBuilder
                .Property(AUsers => AUsers.NickName)
                .IsRequired()
                .HasMaxLength(255);

            AEntityBuilder
                .Property(AUsers => AUsers.Password)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            AEntityBuilder
                .Property(AUsers => AUsers.PhoneNum)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength();

            AEntityBuilder
                .HasOne(AUsers => AUsers.City)
                .WithMany(ACities => ACities.Users)
                .HasForeignKey(AUsers => AUsers.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CityId__Users");

            AEntityBuilder
                .HasOne(AUsers => AUsers.Country)
                .WithMany(ACountries => ACountries.Users)
                .HasForeignKey(AUsers => AUsers.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CountryId__Users");
        }
    }
}
