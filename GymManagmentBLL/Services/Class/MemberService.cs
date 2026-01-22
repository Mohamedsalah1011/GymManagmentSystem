using AutoMapper;
using GymManagmentBLL.Serveces.interfaces;
using GymManagmentBLL.ViewModels.MemberViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Repositories.Class;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Class
{
    public class MemberService : IMemberService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork, IMapper Mapper)
        {
            _mapper = Mapper;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Repo = _unitOfWork.GetRepository<Member>();

            var members =Repo.GetAll();

            if (members == null || !members.Any()) return [];


            #region ManualMapping

            #region MApping way 1

            //var memberViewModels = new List<MemberViewModel>();
            //foreach (var member in members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        ID = member.Id,
            //        Name = member.Name,
            //        Photo = member.Photo,
            //        Phone = member.Phone,
            //        Email = member.Email,
            //        Gender = member.Gender.ToString()
            //    };
            //    memberViewModels.Add(memberViewModel);
            //}
            #endregion
            #region MApping way 2

            var memberViewModels = members.Select(x => new MemberViewModel
            {

                Name = x.Name,
                Photo = x.Photo,
                Phone = x.Phone,
                Email = x.Email,
                Gender = x.Gender.ToString()
            });
            #endregion
            #endregion

            #region auto mapping
            var mappedMemberViewModels = _mapper.Map< IEnumerable<MemberViewModel>>(members);

            #endregion
            return memberViewModels;
        }

        public bool CreateMember(CreateMemberViewModel CreateMember)
        {
            try
            {
               
                if (IsEmailExists(CreateMember.Email) || IsPhoneExists(CreateMember.Phone)) return false;

                var MappedMember = _mapper.Map<CreateMemberViewModel,Member>(CreateMember);
                _unitOfWork.GetRepository<Member>().Add(MappedMember);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;


            }
       
        }

        public MemberViewModel? GetMemberDetials(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            var memberViewModel = _mapper.Map<Member, MemberViewModel>(member);
            var ActivemembewrShip = _unitOfWork.GetRepository<MemberShip>()
                        .GetAll(ms => ms.MemmberId == MemberId && ms.status == "Active").FirstOrDefault();
            if (ActivemembewrShip is not null)
            {
                memberViewModel.MemberShipStartDate = ActivemembewrShip.CreatedAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = ActivemembewrShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActivemembewrShip.PlaneId);
                memberViewModel.PlaneName = plan?.Name;
            }
            return memberViewModel;
        }

        public HealthRecourdViewModel? GetMemberHealthRecourdDetials(int MemberId)
        {
           var MemberHealthRecourd = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecourd == null) return null;
            return _mapper.Map<HealthRecord, HealthRecourdViewModel>(MemberHealthRecourd);

        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;
            return _mapper.Map<MemberToUpdateViewModel>(member);

        }

        public bool UpdateMemberDetials(int Id, MemberToUpdateViewModel MemberToUpdate)
        {
            try
            {
                if (IsEmailExists(MemberToUpdate.Email) || IsPhoneExists(MemberToUpdate.Phone)) return false;
                var MemberRepo = _unitOfWork.GetRepository<Member>();
                var member =MemberRepo.GetById(Id);
                if (member == null) return false;

                _mapper.Map(MemberToUpdate, member);


                MemberRepo.Update(member);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch

            {
                return false;
            }
        }

        public bool DeleteMember(int MemberId)
        {
            try
            {
                var MemberRepo= _unitOfWork.GetRepository<Member>();
                var member = MemberRepo.GetById(MemberId);
                if (member == null) return false;

                var ActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(ms => ms.MemmberId == MemberId && ms.Session.StartDate > DateTime.Now).Any();
                if (ActiveMemberSessions) return false;

                var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();
                var memberShips = MemberShipRepo.GetAll(ms => ms.MemmberId == MemberId);

                if (memberShips.Any())
                {
                    foreach (var memberShip in memberShips)
                        MemberShipRepo.Delete(memberShip.Id);
                    
                }

                MemberRepo.Delete(MemberId);   

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
         return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }

        
        #endregion
    }
}