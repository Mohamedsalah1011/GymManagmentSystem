using GymManagmentDAL.Entites.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberViewModel
{
    internal class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [StringLength(50,MinimumLength =2,ErrorMessage ="Name Must Be Between 2 and 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$",ErrorMessage ="Namme Can Contain only Letters And Spaces")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email Is Required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5,ErrorMessage = "Email Must NBe Between 5 and 100 Char")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]       
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone Number Is Required")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage ="Invalid Phone Number Format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must Be In Egiption Phone Number Format")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Date Of Birth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1,1000,ErrorMessage ="Building Number Id Must Be Between 1 And 1000")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 and 30 Char")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 and 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain only Letters And Spaces")]
        public string City { get; set; } = null!;

        public HealthRecourdViewModel HealthRecourdViewModel { get; set; } = null!;

    }
}
