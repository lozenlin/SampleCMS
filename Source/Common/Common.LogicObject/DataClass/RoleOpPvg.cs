using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// role-operation privilege to store in session
    /// </summary>
    [Serializable()]
    public class RoleOpPvg
    {
        // Pvg value: 0=not-allowed, 1:read, 2:edit, 4:add, 8:delete

        public string RoleName;
        public int OpId;
        public int PvgOfItem;
        public int PvgOfSubitemSelf;
        public int PvgOfSubitemCrew;
        public int PvgOfSubitemOthers;
    }
}
