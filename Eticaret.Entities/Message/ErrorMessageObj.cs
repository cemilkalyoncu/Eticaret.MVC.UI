using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Entities.Message
{
    public class ErrorMessageObj
    {
        public EnumErrorMessages WrongCode { get; set; }
        public string Messages { get; set; }
    }
}
