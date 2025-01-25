using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Repositories
{
    // Genel amaçlı bir depo (repository) arayüzü
    public interface IRepository<TEntity>
        where TEntity : class // TEntity bir sınıf olmalı
    {
        // Belirli bir koşula uyan tek bir varlığı getirir
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
    }
}