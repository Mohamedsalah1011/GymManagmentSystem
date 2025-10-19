using GymManagmentDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configration
{
    internal class SessionConfigration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacity_CK", "Capacity Between 1 and 25");
                tb.HasCheckConstraint("SessionValidCK_EndDate", "EndDate > StartDate");
            });

            builder.HasOne(x => x.SessionCategory)
                   .WithMany(x => x.Sessions)
                   .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.SessionTrainer)
                  .WithMany(x => x.TrainerSessions)
                  .HasForeignKey(x => x.TrainerId);
        }
    }
}
