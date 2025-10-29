using GymManagmentBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSession();
        SessionViewModel? GetSession(int id);
        bool CreateSession(CreateSessionViewModel createSession);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(int sessionId, UpdateSessionViewModel sessionToUpdate);
        bool DeleteSession(int sessionId);


    }
}
