using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.BusinessLogic
{
    public class LoggedInUser
    {
        public int UserID { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool Active { get; set; }
        public int PrimaryDepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Cnic { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; } 
        public string filePath { get; set; } = "";
        public string ImageType { get; set; } = "";
    }

}
