using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hant.Web.API.DAL
{
    /// <summary>
    /// 泛型接口仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> fliter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int page = 0, int limit = 0);
        int Num(Expression<Func<T, bool>> fliter = null);
        T Find(int id);
        void Insert(T t);
        /// <summary>
        /// 如果是new的新实例，不需要更新所有列，请将需要更新的列放到propertys参数里
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertys"></param>
        void Update(T t, string[] propertys = null);
        void Delete(int id);
    }
}