using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class HomeController : Controller
    {
        //Actions
        //BaseUrl/Home/Index
        public ViewResult Index()
        {
          
            return View();
        }

        public JsonResult Trainers() 
        {
            var trainers = new[]
            {
                new { Name = "John Doe", Specialty = "Strength Training" },
                new { Name = "Jane Smith", Specialty = "Yoga" },
                new { Name = "Mike Johnson", Specialty = "Cardio" }
            };
            return Json(trainers);
        }
    }
}
