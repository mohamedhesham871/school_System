using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public  interface IGenericRepo<TEntity,TKey> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        //Want to Make Dynamic Query Later 
        Task<IEnumerable<TEntity>> GetByConditionAsync(ISpecifications<TEntity> Spec);
        Task<TEntity> GetByIdAsyncSpecific( ISpecifications<TEntity> Spec);
        Task<T?> GetEntityWithCode<T>(string code) where T : class, IHasCode;
        void AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);

        //Counting 
        Task<int> CountAsync(ISpecifications<TEntity> Spec);

        Task<bool> ExistsAsync(ISpecifications<TEntity> spec);
    }
}
