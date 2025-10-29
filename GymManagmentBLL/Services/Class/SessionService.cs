using AutoMapper;
using GymManagmentBLL.Services.interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Class
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        

        public IEnumerable<SessionViewModel> GetAllSession()                    
        {                                                                       
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCatigory();

            if(Sessions is null || !Sessions.Any()) return [];

            var MappedSession = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            return MappedSession;
        }

        public SessionViewModel? GetSession(int id)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCatigory(id);
            if (Session is null) return null;

            #region Manual Mapping
            //return new SessionViewModel()
            //{
            //    Capacity = Session.Capacity,
            //    Description = Session.Description,
            //    EndDate = Session.EndDate,
            //    StartDate = Session.StartDate,
            //    CategoryName = Session.SessionCategory.CategoryName,
            //    TrainerName = Session.SessionTrainer.Name,
            //    AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id)
            //}; 
            #endregion

            #region Auto MApping
            var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            return MappedSession;
            #endregion
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCAtigoryExists(createSession.CategoryId)) return false;
                if (!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;

                var MappedSession = _mapper.Map<CreateSessionViewModel, Session>(createSession);
                _unitOfWork.SessionRepository.Add(MappedSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }
        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableToUpdate(session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session);

        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel sessionToUpdate)
        {
            try 
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableToUpdate(session!)) return false;
                if (!IsTrainerExists(sessionToUpdate.TrainerId)) return false;
                if (!IsValidDateRange(sessionToUpdate.StartDate, sessionToUpdate.EndDate)) return false;

                _mapper.Map(sessionToUpdate, session);
                session!.UpdatedAt = DateTime.Now;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);

                if (!IsSessionAvailableToDelete(session!)) return false;

                _unitOfWork.SessionRepository.Delete(sessionId);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }


        #region Helpers
        private bool IsTrainerExists(int TrainerId)
        {
          return  _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }
        private bool IsCAtigoryExists(int CatigoryId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(CatigoryId) is not null;
        }
        private bool IsValidDateRange(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate && StartDate > DateTime.Now;
        }
        private bool IsSessionAvailableToUpdate(Session session)
        {   
            if (session == null) return false;

            if (session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now) return false;

            var HasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            
            if (HasActiveBookings) return false;

            return true;


        }
        private bool IsSessionAvailableToDelete(Session session)
        {
            if (session == null) return false;

            if (session.StartDate >   DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            var HasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBookings) return false;

            return true;


        }

        #endregion
    }
}
