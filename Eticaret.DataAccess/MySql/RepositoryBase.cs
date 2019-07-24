using Eticaret.DataAccess;
using Eticaret.DataAccess.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.DataAccess
{
    public class RepositoryBase
    {
        protected static MySqlDatabaseContext _mySqlDatabaseContext;

        protected RepositoryBase()
        {
            databaseContextCreate();
        }
        public static object _locksync = new object();
        public static void databaseContextCreate()
        {
            if (_mySqlDatabaseContext == null)
            {
                lock (_locksync)
                {
                    if (_mySqlDatabaseContext == null)
                    {
                        _mySqlDatabaseContext = new MySqlDatabaseContext();
                    }
                }
            }
        }
    }
}
