using Domain.Contract;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class GenericRepo<TEntity> (SchoolDbContexts contexts):IGenericRepo<TEntity> where TEntity : class
    {
    }
}
