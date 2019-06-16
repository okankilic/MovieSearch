using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MovieSearch.Domain.Data.Impls
{
    public abstract class GenericRepository<TEntity>
        where TEntity : class
    {
        internal DbContext dbContext;
        internal DbSet<TEntity> dbSetEntity;

        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;

            dbSetEntity = this.dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
            {
                return dbSetEntity;
            }

            return dbSetEntity.Where(expression);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression != null)
            {
                return dbSetEntity.Count(expression);
            }

            return dbSetEntity.Count();
        }

        public virtual TEntity GetByID(int id)
        {
            var t = typeof(TEntity);

            var parameter = Expression.Parameter(t, "x");
            var property = Expression.Property(parameter, "ID");
            var constant = Expression.Constant(id);
            var body = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return dbSetEntity.Single(lambda);
        }

        public virtual void Create(TEntity entity)
        {
            dbSetEntity.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSetEntity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            dbSetEntity.Remove(entity);
        }
    }
}
