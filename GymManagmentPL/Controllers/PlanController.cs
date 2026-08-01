using GymManagmentBLL.Services.interfaces;
using GymManagmentBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatePlan)
        {
            if ((!ModelState.IsValid))
            {
                ModelState.AddModelError("WrongData", "Chack Data Validation");
                return View(updatePlan);
            }
            var result = _planService.UpdatePlanDetials(id, updatePlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update plan.";

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Activate(int id) 
        {
            var result = _planService.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan status Change successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Change plan status.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
