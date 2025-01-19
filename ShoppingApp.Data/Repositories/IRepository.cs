using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
    }
}
