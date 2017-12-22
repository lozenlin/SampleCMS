using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class FrontendBasePage : BasePage
    {
        protected ArticleData articleData;

        public FrontendBasePage()
            : base()
        {
        }

        public ArticleData GetArticleData()
        {
            return articleData;
        }
    }
}
