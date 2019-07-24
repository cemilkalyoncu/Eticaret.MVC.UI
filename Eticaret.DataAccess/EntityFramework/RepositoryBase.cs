using Eticaret.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.DataAccess.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext _databaseContext;

        protected RepositoryBase()
        {
            databaseContextCreate();
        }
        public static object _locksync = new object();
        public static void databaseContextCreate()
        {
            if (_databaseContext == null)
            {
                lock (_locksync)
                {
                    if (_databaseContext == null)
                    {
                        _databaseContext = new DatabaseContext();
                    }
                }
            }
        }
    }
}
