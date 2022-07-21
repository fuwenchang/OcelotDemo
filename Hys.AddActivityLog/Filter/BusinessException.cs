using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hys.AddActivityLog.Filter
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) 
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
