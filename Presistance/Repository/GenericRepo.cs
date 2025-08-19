using Domain.Contract;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class GenericRepo<TEntity, Tkey>(SchoolDbContexts contexts) : IGenericRepo<TEntity, Tkey> where TEntity : class
    {
        public void AddAsync(TEntity entity)
        {
            contexts.Set<TEntity>().AddAsync(entity);
        }

        public void DeleteAsync(TEntity entiy)
        {
            
            contexts.Set<TEntity>().Remove(entiy);
        }
        public void  UpdateAsync(TEntity entity)
        {
            contexts.Set<TEntity>().Update(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await contexts.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Tkey id)
        {
            return await contexts.Set<TEntity>().FindAsync(id) ?? throw new NotFoundException($"Entity of type {typeof(TEntity).Name} with id {id} not found.");
        }
        //making Dynamic Query
        public async Task<IEnumerable<TEntity>> GetByConditionAsync(ISpecifications<TEntity> Spec)
        {
            var SpecificQuery = contex
        }

        public Task<TEntity> GetByIdAsyncSpecifc(ISpecifications<TEntity> Spec)
        {
            throw new NotImplementedException();
        }

       
    }
}
