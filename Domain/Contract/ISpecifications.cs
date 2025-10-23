using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public  interface ISpecifications<TEnity> where TEnity : class
    {

        Expression<Func<TEnity, bool>> Criteria { get; }
        List<Expression<Func<TEnity, object>>> Includes { get; }
        Expression<Func<TEnity, object>> OrderBy { get; }
        Expression<Func<TEnity, object>> OrderByDescending { get; }


        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
