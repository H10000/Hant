using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hant.Web.API.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AccountContext db;
        protected DbSet<T> DbSet;
        public GenericRepository(AccountContext context)
        {
            db = context;
            DbSet = db.Set<T>();
        }
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> fliter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int page = 0, int limit = 0)
        {
            IQueryable<T> query = DbSet;
            if (fliter != null)
            {
                query = query.Where(fliter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (page != 0 && limit != 0)
            {
                query = query.Skip((page - 1) * limit).Take(limit);
            }
            return query.ToList();
        }
        public virtual int Num(Expression<Func<T, bool>> fliter = null)
        {
            IQueryable<T> query = DbSet;
            if (fliter != null)
            {
                query = query.Where(fliter);
            }
            return query.Count();
        }
        public virtual T Find(int id)
        {
            return DbSet.Find(id);
        }
        public virtual void Insert(T t)
        {
            DbSet.Add(t);
        }
        public virtual void Update(T t, string[] propertys = null)
        {
            if (propertys != null)
            {
                DbSet.Attach(t);
                foreach (var property in propertys)
                {
                    db.Entry(t).Property(property).IsModified = true;
                }
            }
        }
        public virtual void Delete(int id)
        {
            T entity = DbSet.Find(id);
            DbSet.Remove(entity);
        }
    }
}