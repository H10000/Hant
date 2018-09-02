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
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> fliter = null, Expression<Func<T, bool>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;
            if (fliter != null)
            {
                query = query.Where(fliter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includePropery in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropery);
                }
            }
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }
            return query;
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