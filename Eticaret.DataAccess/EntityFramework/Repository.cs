using Eticaret.Common;
using Eticaret.Core.DataAccess;
using Eticaret.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Eticaret.DataAccess.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        private DbSet<T> objeSet;
        public Repository()
        {
            objeSet = _databaseContext.Set<T>();
        }
        public List<T> List()
        {
            return objeSet.ToList();
        }
        public IQueryable<T> ListQueryable()
        {
            return objeSet.AsQueryable<T>();
        }
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return objeSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            objeSet.Add(obj);
            if (obj is MyEntityBase)
            {
                MyEntityBase myEntityBase = obj as MyEntityBase;
                DateTime dateTimeCreatedOn = DateTime.Now;
                myEntityBase.CreateOn = dateTimeCreatedOn;
                myEntityBase.ModifiedOn = dateTimeCreatedOn;
                myEntityBase.ModifiedUserName = App.Common.GetUserName(); 
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase myEntityBase = obj as MyEntityBase;
                myEntityBase.ModifiedOn = DateTime.Now;
                myEntityBase.ModifiedUserName = App.Common.GetUserName();
            }
            return Save();
        }
        public int Delete(T obj)
        {
            objeSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return _databaseContext.SaveChanges();
        }
        public T find(Expression<Func<T, bool>> where)
        {
            return objeSet.FirstOrDefault(where);
        }
    }
}
