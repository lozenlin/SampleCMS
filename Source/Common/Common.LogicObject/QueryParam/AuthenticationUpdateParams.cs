using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LogicObject
{
    public class AuthenticationUpdateParams
    {
        /// <summary>
        /// 可修改任何人的子項目
        /// </summary>
        public bool CanEditSubItemOfOthers = false;
        /// <summary>
        /// 可修改同部門的子項目
        /// </summary>
        public bool CanEditSubItemOfCrew = false;
        /// <summary>
        /// 可修改自己的子項目
        /// </summary>
        public bool CanEditSubItemOfSelf = false;
        public string MyAccount = "";
        public int MyDeptId = 0;
    }
}
