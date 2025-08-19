using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
        IGenericRepo<TEntity, Tkey> GetRepository<TEntity,Tkey>() where TEntity : class;
    }
}
