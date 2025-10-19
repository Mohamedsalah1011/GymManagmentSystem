using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Class
{
    public class GenaricRepository<TEntity> : Interfaces.IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbontext;
        public GenaricRepository(GymDbContext dbontext)
        {
            _dbontext = dbontext;
        }
        public void Add(TEntity entity) => _dbontext.Set<TEntity>().Add(entity);


        public void Delete(int id) => _dbontext.Set<TEntity>().Remove(new TEntity { Id = id });






        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
           if (condition == null)
            {
                return _dbontext.Set<TEntity>().AsNoTracking().ToList();
            }
            else
            {
                return _dbontext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            }
        }

        public TEntity? GetById(int id) => _dbontext.Set<TEntity>().Find(id);


        public void Update(TEntity entity) => _dbontext.Set<TEntity>().Update(entity);

    }
}
