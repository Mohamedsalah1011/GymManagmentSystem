using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configration
{
    internal class HealthRecordConfigration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                    .HasKey(x => x.Id);

            builder.HasOne<Member>()
                   .WithOne(x => x.HealthRecord)
                   .HasForeignKey<HealthRecord>(x => x.Id);
            
            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);

        }
    }
}
