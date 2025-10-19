namespace GymManagmentDAL.Entites
{
    public class MemberShip : BaseEntity
    {
        public DateTime EndDate { get; set; }

        public string status {
            get
            {
                if (EndDate <= DateTime.Now)
                    return "expired";
                else
                    return "active"; 
            }
        }
        public int MemmberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlaneId { get; set; }
        public Plan Plane { get; set; } = null!;


    }
}
