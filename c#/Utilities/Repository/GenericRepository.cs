using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class GenericRepository<TSource, TDestination> : IGenericRepository<TSource, TDestination> where TSource : class where TDestination : class
    {
        protected readonly DbContext Context;

        public GenericRepository(DbContext context)
        {
            Context = context;
        }
        public TDestination GetById(int id)
        {
            var getRecord = Context.Set<TSource>().Find(id);
            return getRecord == null ? null : Mapper.Map<TSource, TDestination>(getRecord);
        }

        public IEnumerable<TDestination> GetAll()
        {
            var getRecords = Context.Set<TSource>().ToList()
                .Select(Mapper.Map<TSource, TDestination>);
            var allRecords = getRecords as TDestination[] ?? getRecords.ToArray();
            return !allRecords.Any() ? null : allRecords;
        }

        public IEnumerable<TDestination> Get(Expression<Func<TSource, bool>> predicate)
        {
            var getRecords = Context.Set<TSource>().Where(predicate).ToList()
                .Select(Mapper.Map<TSource, TDestination>);
            var allRecords = getRecords as TDestination[] ?? getRecords.ToArray();
            return !allRecords.Any() ? null : allRecords;
        }

        public TDestination GetByExpression(Expression<Func<TSource, bool>> predicate)
        {
            var record = Context.Set<TSource>().Where(predicate).FirstOrDefault();
            return record == null ? null : Mapper.Map<TSource, TDestination>(record);
        }

        public void Add(TSource entity)
        {
            
            Context.Set<TSource>().Add(entity);
        }

        public void AddRange(IEnumerable<TDestination> entities)
        {
            var destinations = entities as IList<TDestination> ?? entities.ToList();
            var entitiesToDb = destinations.ToList().Select(Mapper.Map<TDestination, TSource>);
            Context.Set<TSource>().AddRange(entitiesToDb);
        }
    }
}