using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberViewModel
{
    internal class HealthRecourdViewModel
    {
        [Required(ErrorMessage ="Height Is Required")]
        [Range(0.1,300,ErrorMessage ="Height Must Be Between 0.1 Cm And 300 Cm")]
        public Decimal Height { get; set; }
        [Required(ErrorMessage = "Weight Is Required")]
        [Range(0.1,500, ErrorMessage = "Weight Must Be Between 0.1 Kg And 500 Kg")]
        public Decimal Weight { get; set; }
        [Required(ErrorMessage = "Blood Type Is Required")]
        [StringLength(3,ErrorMessage ="Blood Type Must Be Between 3 Char or Less ")]
        [RegularExpression(@"^(A|B|AB|O)[+-]$", ErrorMessage = "Blood Type Must Be A+, A-, B+, B-, AB+, AB-, O+ or O-")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
