using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IGenericRepository<TSource, TDestination> where TDestination : class where TSource : class
    {
        #region Finding Records

        TDestination GetById(int id);
        IEnumerable<TDestination> GetAll();
        IEnumerable<TDestination> Get(Expression<Func<TSource, bool>> predicate);
        TDestination GetByExpression(Expression<Func<TSource, bool>> predicate);

        #endregion

        #region Adding Records

        void Add(TSource entity);
        void AddRange(IEnumerable<TDestination> entities);

        #endregion

        
    }
}
