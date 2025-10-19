using GymManagmentBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.interfaces
{
    internal interface IPlanService
    {
        IEnumerable<PlaneViewModel> GetAllPlans();
        PlaneViewModel? GetPlanDetials(int PlanId);
        UpdatePlanViewModel? GetPlanToUpdate(int PlanId);
        bool UpdatePlanDetials(int PlanId, UpdatePlanViewModel UpdatedPlan);
        bool ToggleStatus(int PlanId);
    }
}
