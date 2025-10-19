using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : Entites.BaseEntity, new();

        int SaveChanges();
    }
}
