using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configration
{
    internal class MemberShipConfigration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(x => x.CreatedAt)
                  .HasColumnName("StartDate")
                  .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.MemmberId, x.PlaneId });

            builder.Ignore(x => x.Id);
        }
    }
}
