using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCatigory();
        Session? GetSessionByIdWithTrainerAndCatigory(int Id);

        int GetCountOfBookedSlots(int SessionId);
    }
}
