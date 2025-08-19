using Domain.Contract;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class UnitOfWork(SchoolDbContexts contexts):IUnitOfWork
    {
        private readonly Dictionary<string, object> _Repository = [];

        public IGenericRepo<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : class
        {
            if (_Repository.ContainsKey(typeof(TEntity).Name))
            {
                return (IGenericRepo<TEntity, Tkey>)_Repository[typeof(TEntity).Name];
            }
            else
            {     // Create New Object and Add to Dictionry
                var repo = new GenericRepo<TEntity, Tkey>(contexts);
                _Repository.Add(typeof(TEntity).Name, repo);
                return repo;
            }
        }

        public Task<int> SaveChanges()
        {
          return contexts.SaveChangesAsync();
        }
    }
}
