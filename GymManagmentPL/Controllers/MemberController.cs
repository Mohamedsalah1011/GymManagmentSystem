using GymManagmentBLL.Serveces.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;

        }
        #region GetAllMembers
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        } 
        #endregion

    }
}
