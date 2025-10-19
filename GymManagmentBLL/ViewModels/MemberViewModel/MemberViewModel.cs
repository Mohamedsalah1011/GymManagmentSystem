using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberViewModel
{
    internal class MemberViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? PlaneName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? MemberShipStartDate { get; set; }
        public string? MemberShipEndDate { get; set; }
        public string? Address { get; set; }



    }
}
