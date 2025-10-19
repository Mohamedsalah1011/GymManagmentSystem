using GymManagmentDAL.Entites.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentDAL.Entites
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirtih { get; set; }
        public Gender Gender { get; init; }
        public Address Address { get; set; } = null!;
    }
    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
