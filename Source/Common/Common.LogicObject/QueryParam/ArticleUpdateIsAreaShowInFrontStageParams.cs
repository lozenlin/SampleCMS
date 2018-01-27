using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class ArticleUpdateIsAreaShowInFrontStageParams
    {
        public Guid ArticleId;
        public string AreaName;
        public bool IsShowInFrontStage;
        public string MdfAccount;
        public AuthenticationUpdateParams AuthUpdateParams;

        public ArticleUpdateIsAreaShowInFrontStageParams()
        {
            AuthUpdateParams = new AuthenticationUpdateParams();
        }
    }
}
