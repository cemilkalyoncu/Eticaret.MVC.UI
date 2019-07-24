using Eticaret.Core.DataAccess;
using Eticaret.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Business.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class
    {
        private Repository<T> repository = new Repository<T>();
        public virtual int Delete(T obj)
        {
            return repository.Delete(obj);
        }

        public virtual T find(Expression<Func<T, bool>> where)
        {
            return repository.find(where);
        }

        public virtual int Insert(T obj)
        {
            return repository.Insert(obj);
        }

        public virtual List<T> List()
        {
            return repository.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            return repository.List(where);
        }

        public virtual IQueryable<T> ListQueryable()
        {
            return repository.ListQueryable();
        }

        public virtual int Save()
        {
            return repository.Save();
        }

        public virtual int Update(T obj)
        {
            return repository.Update(obj);
        }
    }
}
