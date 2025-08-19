using Domain.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public  class SpecificationEvaluator
    {
        public static IQueryable<T> CreateQuery<T>(IQueryable<T>InputQuery,ISpecifications<T> specifications) where T : class
        {
            //1-
            if (specifications.Criteria is not null)
            {
                InputQuery.Where(specifications.Criteria);
            }
            //2
            //Can Use Aggregate Function or Using Loop for Include
            InputQuery =specifications.Includes.Aggregate(InputQuery ,(current, include) => current.Include(include));
            //3
            if (specifications.OrderBy is not null)
            {
                InputQuery.OrderBy(specifications.OrderBy);
            }
            //4
            if(specifications.OrderByDescending is not null)
            {
                InputQuery.OrderByDescending(specifications.OrderByDescending);
            }
            //5
            if (specifications.IsPagingEnabled)
            {
                InputQuery = InputQuery.Skip(specifications.Skip).Take(specifications.Take);
            }
            return InputQuery;
        }
    }
}
