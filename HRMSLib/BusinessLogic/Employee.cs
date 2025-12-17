using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.BusinessLogic
{
    public class Employee
    {
        public long EmployeeID { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public string FatherOrSpouseName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string ShiftTiming { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string Status { get; set; }
    }
}
