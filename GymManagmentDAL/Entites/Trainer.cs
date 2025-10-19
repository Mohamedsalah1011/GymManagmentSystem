using GymManagmentDAL.Entites.Enums;

namespace GymManagmentDAL.Entites
{
    public class Trainer : GymUser
    {
        // HireDate == CreatedAt
        public Specialties Specialties { get; set; }
        public ICollection<Session> TrainerSessions { get; set; } = null!;

    }
}
