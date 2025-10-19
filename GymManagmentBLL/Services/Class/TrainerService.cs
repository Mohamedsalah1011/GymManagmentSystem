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
        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers == null || !trainers.Any()) return [];
            return trainers.Select(x => new TrainerViewModel
            {
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Specialty = x.Specialties.ToString()
            });

        }
        public bool CreateTrainer(CreatTrainerViewModel CreateTrainer)
        {
            try
            {
                if (IsEmailExists(CreateTrainer.Email) || IsPhoneExists(CreateTrainer.Phone)) return false;

                var trainer = new Trainer
                {
                    Name = CreateTrainer.Name,
                    Email = CreateTrainer.Email,
                    Phone = CreateTrainer.Phone,
                    DateOfBirtih = CreateTrainer.DateOfBirth,
                    Gender = CreateTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = CreateTrainer.BuildingNumber,
                        Street = CreateTrainer.Street,
                        City = CreateTrainer.City
                    },
                    Specialties = CreateTrainer.Specialty,
                    CreatedAt = DateTime.Now

                };
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
            return new TrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirtih.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
                Specialty = trainer.Specialties.ToString()
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;
            return new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialty = trainer.Specialties
            };
        }
        public bool UpdateTrainerDetials(int Id, TrainerToUpdateViewModel TrainerToUpdate)
        {
            
            try
            {
                if (IsEmailExists(TrainerToUpdate.Email) || IsPhoneExists(TrainerToUpdate.Phone)) return false;
                var trainerrepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = trainerrepo.GetById(Id);
                if (trainer is null) return false;
                    
                trainer.Email = TrainerToUpdate.Email;
                trainer.Phone = TrainerToUpdate.Phone;
                trainer.Address.BuildingNumber = TrainerToUpdate.BuildingNumber;
                trainer.Address.Street = TrainerToUpdate.Street;
                trainer.Address.City = TrainerToUpdate.City;
                trainer.Specialties = TrainerToUpdate.Specialty;
                trainer.UpdatedAt = DateTime.Now;

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
