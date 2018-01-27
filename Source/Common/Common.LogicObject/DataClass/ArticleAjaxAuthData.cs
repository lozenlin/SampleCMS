using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LogicObject
{
    public class ArticleAjaxAuthData
    {
        public string EmpAccount = "";
        /// <summary>
        /// 可閱讀任何人的子項目
        /// </summary>
        public bool CanReadSubItemOfOthers = false;
        /// <summary>
        /// 可閱讀同部門的子項目
        /// </summary>
        public bool CanReadSubItemOfCrew = false;
        /// <summary>
        /// 可閱讀自己的子項目
        /// </summary>
        public bool CanReadSubItemOfSelf = false;
        public DateTime PostDate;
    }
}
