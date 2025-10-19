using GymManagmentBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Serveces.interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel CreateMember);
        MemberViewModel? GetMemberDetials(int MemberId);
        HealthRecourdViewModel? GetMemberHealthRecourdDetials(int MemberId);
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetials(int Id, MemberToUpdateViewModel MemberToUpdate);
        bool DeleteMember(int MemberId);

    }
}
