using Microsoft.EntityFrameworkCore;

namespace SecureWebApp.Database
{

    public class MainDbContext : DbContext
    {

        public MainDbContext(DbContextOptions<MainDbContext> AOptions) : base(AOptions)
        {
        }

        public MainDbContext() 
        {        
        }

        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<SigninHistory> SigninHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder AModelBuilder)
        {

            AModelBuilder.Entity<Cities>(AEntity =>
            {
                AEntity.Property(ACities => ACities.CityName)
                    .IsRequired()
                    .HasMaxLength(255);

                AEntity.HasOne(ACities => ACities.Country)
                    .WithMany(ACountries => ACountries.Cities)
                    .HasForeignKey(ACities => ACities.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CountryId__Cities");
            });

            AModelBuilder.Entity<Countries>(AEntity =>
            {
                AEntity.Property(ACountries => ACountries.CountryCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                AEntity.Property(ACountries => ACountries.CountryName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            AModelBuilder.Entity<SigninHistory>(AEntity =>
            {

                AEntity.Property(ASigninHistory => ASigninHistory.LoggedAt).HasColumnType("datetime");

                AEntity.HasOne(ASigninHistory => ASigninHistory.User)
                    .WithMany(AUsers => AUsers.SigninHistory)
                    .HasForeignKey(ASigninHistory => ASigninHistory.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserId__Users");
            });

            AModelBuilder.Entity<Users>(AEntity =>
            {
                AEntity.HasIndex(AUsers => AUsers.EmailAddr)
                    .HasName("UQ__EmailAddr__Users")
                    .IsUnique();

                AEntity.Property(AUsers => AUsers.CreatedAt).HasColumnType("datetime");

                AEntity.Property(AUsers => AUsers.EmailAddr)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                AEntity.Property(AUsers => AUsers.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                AEntity.Property(AUsers => AUsers.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                AEntity.Property(AUsers => AUsers.NickName)
                    .IsRequired()
                    .HasMaxLength(255);

                AEntity.Property(AUsers => AUsers.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                AEntity.Property(AUsers => AUsers.PhoneNum)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                AEntity.HasOne(AUsers => AUsers.City)
                    .WithMany(ACities => ACities.Users)
                    .HasForeignKey(AUsers => AUsers.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CityId__Users");

                AEntity.HasOne(AUsers => AUsers.Country)
                    .WithMany(ACountries => ACountries.Users)
                    .HasForeignKey(AUsers => AUsers.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CountryId__Users");
            });
       
        }

    }

}
