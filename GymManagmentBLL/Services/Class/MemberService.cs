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
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
     
           _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();

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
            return memberViewModels;
        }

        public bool CreateMember(CreateMemberViewModel CreateMember)
        {
            try
            {
               
                if (IsEmailExists(CreateMember.Email) || IsPhoneExists(CreateMember.Phone)) return false;

                var Member = new Member()
                {
                    Name = CreateMember.Name,
                    Email = CreateMember.Email,
                    Phone = CreateMember.Phone,
                    DateOfBirtih = CreateMember.DateOfBirth,
                    Gender = CreateMember.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = CreateMember.BuildingNumber,
                        Street = CreateMember.Street,
                        City = CreateMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = CreateMember.HealthRecourdViewModel.Height,
                        Weight = CreateMember.HealthRecourdViewModel.Weight,
                        BloodType = CreateMember.HealthRecourdViewModel.BloodType,
                        Note = CreateMember.HealthRecourdViewModel.Note
                    }
                };
                 _unitOfWork.GetRepository<Member>().Add(Member);

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

            var memberViewModel = new MemberViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Phone = member.Phone,
                Email = member.Email,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirtih.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
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
            return new HealthRecourdViewModel()
            {
                Height = MemberHealthRecourd.Height,
                Weight = MemberHealthRecourd.Weight,
                BloodType = MemberHealthRecourd.BloodType,
                Note = MemberHealthRecourd.Note
            };

        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;
            return new MemberToUpdateViewModel()
            {
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
        }

        public bool UpdateMemberDetials(int Id, MemberToUpdateViewModel MemberToUpdate)
        {
            try
            {
                if (IsEmailExists(MemberToUpdate.Email) || IsPhoneExists(MemberToUpdate.Phone)) return false;
                var MemberRepo = _unitOfWork.GetRepository<Member>();
                var member =MemberRepo.GetById(Id);
                if (member == null) return false;

                member.Email = MemberToUpdate.Email;
                member.Phone = MemberToUpdate.Phone;
                member.Photo = MemberToUpdate.Photo;
                member.Address.BuildingNumber = MemberToUpdate.BuildingNumber;
                member.Address.Street = MemberToUpdate.Street;
                member.Address.City = MemberToUpdate.City;
                member.UpdatedAt = DateTime.Now;


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