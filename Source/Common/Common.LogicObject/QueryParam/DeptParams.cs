using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class DeptParams
    {
        public int DeptId;
        public string DeptName;
        public int SortNo;
        public string PostAccount;
        /// <summary>
        /// return:部門名稱是否重覆
        /// </summary>
        public bool HasDeptNameBeenUsed = false;
        /// <summary>
        /// return:部門已有帳號使用
        /// </summary>
        public bool IsThereAccountsOfDept = false;
    }
}
