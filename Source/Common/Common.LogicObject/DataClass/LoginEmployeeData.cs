using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// 登入者資料
    /// </summary>
    [Serializable()]
    public class LoginEmployeeData
    {
        public int EmpId;
        public string EmpName;
        public string Email;
        public int DeptId;
        public string DeptName;
        public int RoleId;
        public string RoleName;
        public string RoleDisplayName;
        public DateTime StartDate;
        public DateTime EndDate;
        public string EmpAccount;
        public DateTime ThisLoginTime;
        public string ThisLoginIP;
        public DateTime LastLoginTime;
        public string LastLoginIP;
    }
}
