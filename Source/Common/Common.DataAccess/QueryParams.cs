using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataAccess
{
    public class RoleOpDescParamsDA
    {
        public string RoleName;
        public int OpId;
        public bool CanRead = false;
        public bool CanEdit = false;
        public bool CanReadSubItemOfSelf = false;
        public bool CanEditSubItemOfSelf = false;
        public bool CanAddSubItemOfSelf = false;
        public bool CanDelSubItemOfSelf = false;
        public bool CanReadSubItemOfCrew = false;
        public bool CanEditSubItemOfCrew = false;
        public bool CanDelSubItemOfCrew = false;
        public bool CanReadSubItemOfOthers = false;
        public bool CanEditSubItemOfOthers = false;
        public bool CanDelSubItemOfOthers = false;
        public string PostAccount;
    }
}
