using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Common.LogicObject
{
    /// <summary>
    /// 前台網頁的共用元件
    /// </summary>
    public class FrontendPageCommon : PageCommon
    {
        /// <summary>
        /// 前台網頁的共用元件
        /// </summary>
        public FrontendPageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }
    }
}
