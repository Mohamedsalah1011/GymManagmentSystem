using AutoMapper;
using GymManagmentBLL.Services.interfaces;
using GymManagmentBLL.ViewModels.PlanViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Class
{
    internal class PlaneService : IPlanService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PlaneService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlaneViewModel> GetAllPlans()
        {
           var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans == null || !Plans.Any()) return [];
         
            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlaneViewModel>>(Plans);

        }

        public PlaneViewModel? GetPlanDetials(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);   
            if (plan == null) return null;
            return _mapper.Map<Plan, PlaneViewModel>(plan);

        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
           var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan is null || plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;
         
            return _mapper.Map<Plan, UpdatePlanViewModel>(plan);

        }

        public bool UpdatePlanDetials(int PlanId, UpdatePlanViewModel UpdatedPlan)
        {
            try
            {
                var PlanRepo = _unitOfWork.GetRepository<Plan>();
                var plan = PlanRepo.GetById(PlanId);
                if (plan is null || HasActiveMemberShip(PlanId)) return false;
                (plan.Description, plan.DurationDays, plan.Price, plan.UpdatedAt) =
                    (UpdatedPlan.Description, UpdatedPlan.DurationDays, UpdatedPlan.Price, DateTime.Now);
                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool ToggleStatus(int PlanId)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = PlanRepo.GetById(PlanId);

            if (plan is null || HasActiveMemberShip(PlanId)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;
            try { 
                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Method
        private bool HasActiveMemberShip(int PlanId)
        {
            return _unitOfWork.GetRepository<MemberShip>()
                .GetAll(ms => ms.PlaneId == PlanId && ms.status =="Active").Any();
        }
        #endregion
    }
}
