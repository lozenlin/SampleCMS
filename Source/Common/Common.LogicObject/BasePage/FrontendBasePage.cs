using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class FrontendBasePage : BasePage
    {
        protected FrontendPageCommon frontendPageCommon;

        public FrontendBasePage()
            : base()
        {
        }

        public void SetFrontendPageCommon(FrontendPageCommon frontendPageCommon)
        {
            this.frontendPageCommon = frontendPageCommon;
        }

        public FrontendPageCommon GetFrontendPageCommon()
        {
            return frontendPageCommon;
        }
    }
}
