using Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Specifications<TEntity> : ISpecifications<TEntity> where TEntity : class
    {

        public Specifications()
        {
            
        }
        public Specifications(Expression<Func<TEntity,bool>>?criteria)
        {
            Criteria = criteria;

        }
        public Expression<Func<TEntity, bool>>? Criteria { get; private set;   }

        public List<Expression<Func<TEntity, object>>> Includes { get; private set; } = [];

        public Expression<Func<TEntity, object>>? OrderBy { get;private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get;private set; }

        public int Take { get; private set; } = 10;

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; } = true;

        //Crearte Function For Adding Includes
        public void AddInclude(Expression<Func<TEntity,object>> IncludeExpression)
        {
            Includes.Add(IncludeExpression);
        }
        //Create Function For Adding OrderBy
        public void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        //Create Function For Adding OrderByDescending
        public void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        //Create Function For Adding Paging
        public void ApplyPaging(int IndexPage, int PageSize)
        {
            IsPagingEnabled = true;
            Skip = (IndexPage-1) * PageSize;
            Take = PageSize;
        }
    }
}
