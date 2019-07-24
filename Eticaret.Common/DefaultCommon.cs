using Eticaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Eticaret.Common
{
    public class DefaultCommon:ICommon
    {
        public string GetUserName()
        {
            return "system";
        }
        
    }
}
