using Eticaret.Entities.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Business.Results
{
    public class BusinessErrorResult<T> where T : class
    {
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public BusinessErrorResult()
        {
            Errors = new List<ErrorMessageObj>();
        }
        public void AddError(EnumErrorMessages enumErrorMessages, string message)
        {
            Errors.Add(new ErrorMessageObj()
            {
                WrongCode = enumErrorMessages,
                Messages = message
            });
        }
    }
}
