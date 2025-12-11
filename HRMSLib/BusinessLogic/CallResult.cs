using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.BusinessLogic
{
    [Serializable]
    public class CallResult
    {
        public bool Successful = true;
        public string Code = string.Empty;
        public string Message = string.Empty;
        public object Value = null;
    }
}
