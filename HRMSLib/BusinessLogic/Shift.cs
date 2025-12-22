using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.BusinessLogic
{
    public class Shift
    {
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int ShiftTypeID { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int GraceMinutes { get; set; }
        public int MinWorkMinutes { get; set; }
        public bool IsCrossMidnight { get; set; }
    }
}
