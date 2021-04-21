using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace StrictlyStatsDataLayer
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);

        List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        T Get(Expression<Func<T, bool>> predicate);
        TableQuery<T> AsQueryable();

        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Delete(int id);
    }

}