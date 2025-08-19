using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public  interface IGenricRepo<TEntiy,TKey> where TEntiy : class
    {
        Task<TEntiy> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntiy>> GetAllAsync();
        //Want to Make Dynaimc Query Later 
        Task<IEnumerable<TEntiy>> GetByConditionAsync(ISpecifications<TEntiy> Spec);
        Task<TEntiy>GetByIdAsyncSpecifc( ISpecifications<TEntiy> Spec);

        Task AddAsync(TEntiy entity);
        Task UpdateAsync(TEntiy entity);
        Task DeleteAsync(TEntiy entiy);

    }
}
