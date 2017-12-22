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

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// Article id
        /// </summary>
        public Guid? qsArtId
        {
            get
            {
                string str = QueryStringToSafeStr("artid");
                Guid result;

                if (str != null && Guid.TryParse(str, out result))
                {
                }
                else
                {
                    return null;
                }

                return result;
            }
        }

        /// <summary>
        /// Article alias
        /// </summary>
        public string qsAlias
        {
            get
            {
                return QueryStringToSafeStr("alias");
            }
        }

        /// <summary>
        /// Token value to preview article page
        /// </summary>
        public string qsPreview
        {
            get
            {
                return QueryStringToSafeStr("preview");
            }
        }

        #endregion

        protected ArticlePublisherLogic artPub;
        protected ArticleData articleData;

        /// <summary>
        /// 前台網頁的共用元件
        /// </summary>
        public FrontendPageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
            artPub = new ArticlePublisherLogic(null);
            articleData = new ArticleData();
        }

        /// <summary>
        /// Extract ArticleId and initial article data by querystring: preview, alias, artid
        /// </summary>
        public bool ExtractArticleIdAndInitialData()
        {
            bool result = ExtractArticleId();

            if (result)
            {
                result = InitialArticleData();
            }

            return result;
        }

        /// <summary>
        /// Extract ArticleId by querystring: preview, alias, artid
        /// </summary>
        public virtual bool ExtractArticleId()
        {
            bool result = false;

            if (qsPreview != null)
            {
                HandlePreviewToken();
            }
            else if (qsAlias != null)
            {
            }
            else if (qsArtId != null)
            {
                articleData.ArticleId = qsArtId.Value;
                result = true;
            }

            return result;
        }

        protected void HandlePreviewToken()
        {
            if (qsPreview == null)
                return;

            if (qsPreview == "1")
            {
                // redirect to back-stage to get authorization
            }
            else
            {
                // decrypt token
                // articleData.ArticleId = 
            }
        }

        protected bool InitialArticleData()
        {
            bool result = false;

            return result;
        }

        public ArticleData GetArticleData()
        {
            return articleData;
        }
    }

    /// <summary>
    /// 其他 Article 網頁的共用元件
    /// </summary>
    public class OtherArticlePageCommon : FrontendPageCommon
    {
        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie
        #endregion

        public OtherArticlePageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }

        /// <summary>
        /// Extract ArticleId and initial article data by querystring: preview, alias, artid
        /// </summary>
        public bool ExtractArticleIdAndInitialData(Guid articleId)
        {
            bool result = ExtractArticleId(articleId);

            if (result)
            {
                result = InitialArticleData();
            }

            return result;
        }

        /// <summary>
        /// Extract ArticleId by querystring: preview, alias, artid
        /// </summary>
        public override bool ExtractArticleId()
        {
            bool result = base.ExtractArticleId();

            // get ArticleId by LinkUrl

            return result;
        }

        /// <summary>
        /// Extract ArticleId by querystring: preview, alias, artid
        /// </summary>
        public virtual bool ExtractArticleId(Guid articleId)
        {
            bool result = false;

            if (qsPreview != null)
            {
                HandlePreviewToken();
            }
            else
            {
                articleData.ArticleId = articleId;
                result = true;
            }

            return result;
        }

    }
}
