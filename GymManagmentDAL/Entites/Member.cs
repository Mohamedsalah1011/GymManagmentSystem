namespace GymManagmentDAL.Entites
{
    public class Member : GymUser
    {
        // join date == created at
        public string? Photo { get; set; }

        #region RelationShips

        #region Member - HealthReacord
        public HealthRecord HealthRecord { get; set; } = null!;


        #endregion

        #region Member - MemberShip
        public ICollection<MemberShip> MemberShips { get; set; }

        #endregion

        #region Member - MemberSession

        public ICollection<MemberSession> MemberSessions { get; set; }
        #endregion
        #endregion
    }
}
