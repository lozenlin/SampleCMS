// ===============================================================================
// FrontendPageCommon of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// FrontendPageCommon.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

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
        public string WEBSITE_HOME = "Index.aspx";
        public string ERROR_PAGE = "ErrorPage.aspx";

        protected ArticlePublisherLogic artPub;
        protected ArticleData articleData;

        /// <summary>
        /// 前台網頁的共用元件
        /// </summary>
        public FrontendPageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
            artPub = new ArticlePublisherLogic(null);
        }

        public bool InitialArticleData()
        {
            bool result = false;

            return result;
        }

        public ArticleData GetArticleData()
        {
            return articleData;
        }
    }
}
