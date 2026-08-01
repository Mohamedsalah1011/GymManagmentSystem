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
        #region GetMember Data
        public IActionResult MemberDetails(int id)
        { 
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }
               

            var member = _memberService.GetMemberDetials(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

                
            return View(member);
        }

        public IActionResult HealthRecourdDetials(int id) 
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }
               
            var healthRecourd = _memberService.GetMemberHealthRecourdDetials(id);
            if (healthRecourd is null)
            {
                TempData["ErrorMessage"] = "Health Record not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecourd);
        }

        #endregion
        #region Add Membre
        public ActionResult Create()
        {
            return View();
        }
        #endregion

    }
}
