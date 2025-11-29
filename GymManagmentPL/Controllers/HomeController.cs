using GymManagmentBLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        //Actions
        //BaseUrl/Home/Index
        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public ViewResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();

            return View(Data);
        }

        
    }
}
