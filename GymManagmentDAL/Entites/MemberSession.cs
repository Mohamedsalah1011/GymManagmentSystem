namespace GymManagmentDAL.Entites
{
    public class MemberSession : BaseEntity
    {
        public bool IsAttended { get; set; }

        public int MemmberId { get; set; }
        public Member Member { get; set; } = null!;
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;



    }
}
