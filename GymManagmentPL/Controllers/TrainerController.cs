using GymManagmentBLL.Services.interfaces;
using GymManagmentBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region GetAllTrainers
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        #endregion

        #region GetTrainer Data
        public IActionResult Details(int id)
        {
          
            var trainer = _trainerService.GetTrainerDetials(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        #endregion

        #region Add Trainer
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(CreatTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = _trainerService.CreateTrainer(model);
            if (result)
            {
                TempData["SucssesMessage"] = "Trainer Created Successfully!";
                return RedirectToAction(nameof(Index));
            }
            else {        
                TempData["ErrorMessage"] = "Email or Phone already exists.";
                return View(model);

            }
        }
        #endregion

        #region Edit Trainer
        public IActionResult Edit(int id)
        {
            

            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit(int id, TrainerToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = _trainerService.UpdateTrainerDetials(id, model);
            if (success)
            {
                TempData["SucssesMessage"] = "Trainer details updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update trainer details.";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        public IActionResult Delete(int id)
        {
            

            var trainer = _trainerService.GetTrainerDetials(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var success = _trainerService.DeleteTrainer(id);
            if (success)
            {
                TempData["SucssesMessage"] = "Trainer deleted successfully";
               
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete trainer. ";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
