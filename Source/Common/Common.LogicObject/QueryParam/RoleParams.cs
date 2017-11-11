using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class RoleParams
    {
        public int RoleId;
        public string RoleName;
        public string RoleDisplayName;
        public int SortNo;
        public string CopyPrivilegeFromRoleName;
        public string PostAccount;
        /// <summary>
        /// return:身分是否重覆
        /// </summary>
        public bool HasRoleBeenUsed = false;
        /// <summary>
        /// return:身分已有帳號使用
        /// </summary>
        public bool IsThereAccountsOfRole = false;
    }
}
