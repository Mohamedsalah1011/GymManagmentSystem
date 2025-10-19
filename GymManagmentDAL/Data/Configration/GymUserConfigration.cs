using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configration
{
    internal class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(x => x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.Property(x => x.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);


            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidCK_Email", "Email LIKE '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserValidCK_Phone", "Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%'");
            });

            //ununiqe non clasterd index 
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();


            builder.OwnsOne(x => x.Address, ab =>
            {
                ab.Property(a => a.BuildingNumber)
               .HasColumnName("BuildingNumber");

                ab.Property(a => a.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                ab.Property(a => a.City)
                .HasColumnName("City")
                .HasColumnType("varchar")
                .HasMaxLength(30);
            });
        }
    }
}
