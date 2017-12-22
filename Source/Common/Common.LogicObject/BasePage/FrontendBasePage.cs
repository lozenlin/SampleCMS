using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class FrontendBasePage : BasePage
    {
        protected FrontendPageCommon c;

        public FrontendBasePage()
            : base()
        {
        }

        public void SetFrontendPageCommon(FrontendPageCommon frontendPageCommon)
        {
            c = frontendPageCommon;
        }

        public FrontendPageCommon GetFrontendPageCommon()
        {
            return c;
        }
    }
}
