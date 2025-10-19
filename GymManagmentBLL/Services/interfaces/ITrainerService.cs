using GymManagmentBLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreatTrainerViewModel CreateTrainer);
        TrainerViewModel? GetTrainerDetials(int TrainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId);
        bool UpdateTrainerDetials(int Id, TrainerToUpdateViewModel TrainerToUpdate);
        bool DeleteTrainer(int TrainerId);
    }
}
