using System;
using System.Collections.Generic;
using System.IO;
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
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected SQLiteConnection con;

        internal Repository(SQLiteConnection con)
        {
            if (con == null)
                throw new ArgumentNullException("Null SQLiteConnection");
            this.con = con;
        }

        public List<T> GetAll() =>
             con.Table<T>().ToList();

        public T GetById(int id) =>
             con.Find<T>(id);

        public List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = con.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>> predicate) =>
            con.Find<T>(predicate);

        public TableQuery<T> AsQueryable() =>
            con.Table<T>();

        public int Insert(T entity) {
            int i = con.Insert(entity);
            return i;
        }

        public int Update(T entity) =>
             con.Update(entity);

        public int Delete(T entity) =>
             con.Delete(entity);

        public int Delete(int id) =>
             con.Delete(id);
    }
}