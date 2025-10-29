using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Class
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository) 
        { 
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        private readonly Dictionary<Type, object> _repositorys = new();

        public ISessionRepository SessionRepository { get;}

        //key : Type => member,trainer , value : object => GenaricRepository<Member>, GenaricRepository<Trainer>

        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //لو عملناها كدا هيعمل اوبجكت جديد كل مرة نطلب فيها الريبو
            //return new  GenaricRepository<TEntity>(_dbContext);

            var EntityType = typeof(TEntity);
            if (_repositorys.ContainsKey(EntityType))
                return (IGenaricRepository<TEntity>)_repositorys[EntityType];

            var NewRepo = new GenaricRepository<TEntity>(_dbContext);
            _repositorys[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
