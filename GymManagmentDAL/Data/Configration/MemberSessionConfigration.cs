using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configration
{
    internal class MemberSessionConfigration : IEntityTypeConfiguration<MemberSession>

    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(x => x.CreatedAt)
                 .HasColumnName("BokingDate")
                 .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.MemmberId, x.SessionId });

            builder.Ignore(x => x.Id);
        }
    }
}
