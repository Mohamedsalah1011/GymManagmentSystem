using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.TrainerViewModel
{
    internal class TrainerViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } =null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string Specialty { get; set; } = null!;

    }
}
