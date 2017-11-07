﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class AccountVM
    {
        public int EmpId;
        public string EmpAccount;
        public string EmpPassword;
        public string EmpName;
        public string Email;
        public string Remarks;
        public int DeptId;
        public int RoleId;
        public bool IsAccessDenied = true;
        public DateTime StartDate;
        public DateTime EndDate;
        public string OwnerAccount;
        public bool PasswordHashed = true;
        public string DefaultRandomPassword;
        public string PostAccount;
    }
}
