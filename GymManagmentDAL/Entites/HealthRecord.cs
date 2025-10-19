namespace GymManagmentDAL.Entites
{
    public class HealthRecord : BaseEntity
    {
        public Decimal Height { get; set; }
        public Decimal Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
