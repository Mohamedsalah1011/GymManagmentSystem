using AutoMapper;
using GymManagmentBLL.Services.interfaces;
using GymManagmentBLL.ViewModels.TrainerViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Class
{
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainerService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers == null || !trainers.Any()) return [];
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);  


        }
        public bool CreateTrainer(CreatTrainerViewModel CreateTrainer)
        {
            try
            {
                if (IsEmailExists(CreateTrainer.Email) || IsPhoneExists(CreateTrainer.Phone)) return false;

                var trainer =_mapper.Map<CreatTrainerViewModel, Trainer>(CreateTrainer);
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public TrainerViewModel? GetTrainerDetials(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;
            return _mapper.Map<Trainer, TrainerViewModel>(trainer); 

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;
            return _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer); 

        }
        public bool UpdateTrainerDetials(int Id, TrainerToUpdateViewModel TrainerToUpdate)
        {
            
            try
            {
                if (IsEmailExists(TrainerToUpdate.Email) || IsPhoneExists(TrainerToUpdate.Phone)) return false;
                var trainerrepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = trainerrepo.GetById(Id);
                if (trainer is null) return false;
                    
                _mapper.Map(TrainerToUpdate, trainer);
                trainerrepo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteTrainer(int TrainerId)
        {
            try
            {
                var trainerrepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = trainerrepo.GetById(TrainerId);
                if (trainer is null || HasActiveSession(TrainerId)) return false;
               

                trainerrepo.Delete(TrainerId);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Method
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }
        private bool HasActiveSession(int TrainerId)
        {
            return _unitOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == TrainerId && s.StartDate > DateTime.Now).Any();
        }

        #endregion
    }
}
