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
using System.Data;
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
        /// 語言號碼(l或lang,l優先)
        /// </summary>
        public override int qsLangNo
        {
            get
            {
                string str = QueryStringToSafeStr("l");
                if (str == null)
                    str = QueryStringToSafeStr("lang");

                int nResult;

                if (str != null)
                    str = str.Trim();

                if (string.IsNullOrEmpty(str))
                {
                    //未指定,抓瀏覽器的
                    string resultCultureName = GetAllowedUserCultureName();

                    nResult = Convert.ToInt32(new LangManager().GetLangNo(resultCultureName));
                }
                else if (int.TryParse(str, out nResult))
                {
                    //有指定, 限制範圍
                    if (nResult < 1 || nResult > 2)
                        nResult = 1;
                }

                return nResult;
            }
        }

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
        protected bool isPreviewMode = false;

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
        /// Retrieve ArticleId and initial article data by querystring: preview, alias, artid
        /// </summary>
        public bool RetrieveArticleIdAndData()
        {
            bool result = RetrieveArticleId();

            if (result)
            {
                result = RetrieveArticleData();
            }

            return result;
        }

        /// <summary>
        /// Retrieve ArticleId by querystring: preview, alias, artid
        /// </summary>
        public virtual bool RetrieveArticleId()
        {
            bool result = false;

            if (qsPreview != null)
            {
                result = HandlePreviewToken();
            }
            else if (qsAlias != null)
            {
                // get id by alias
                articleData.ArticleId = artPub.GetArticleIdByAlias(qsAlias);

                if (articleData.ArticleId.HasValue)
                {
                    result = true;
                }
                else
                {
                    logger.DebugFormat("can not get ArticleId of alias[{0}]", qsAlias);
                }
            }
            else if (qsArtId != null)
            {
                articleData.ArticleId = qsArtId.Value;
                result = true;
            }

            return result;
        }

        protected bool HandlePreviewToken()
        {
            bool result = false;

            if (qsPreview == null)
                return false;

            if (qsPreview == "1")
            {
                // redirect to back-stage to get authorization
            }
            else
            {
                // decrypt token
                // articleData.ArticleId = 
            }

            return result;
        }

        /// <summary>
        /// Retrieve article data by ArticleId
        /// </summary>
        protected bool RetrieveArticleData()
        {
            bool result = false;

            if (!articleData.ArticleId.HasValue)
                return result;

            DataSet dsArticle = artPub.GetArticleDataForFrontend(articleData.ArticleId.Value, qsCultureNameOfLangNo);

            if (dsArticle != null && dsArticle.Tables[0].Rows.Count > 0)
            {
                DataRow drArticle = dsArticle.Tables[0].Rows[0];
                bool isValid = false;

                if (isPreviewMode)
                {
                    isValid = true;
                }
                else
                {
                    // check validation date, isHideSelf, isShowInLang
                    DateTime startDate = Convert.ToDateTime(drArticle["StartDate"]);
                    DateTime endDate = Convert.ToDateTime(drArticle["EndDate"]);
                    bool isHideSelf = Convert.ToBoolean(drArticle["IsHideSelf"]);
                    bool isShowInLang = Convert.ToBoolean(drArticle["IsShowInLang"]);

                    if (startDate <= DateTime.Today && DateTime.Today <= endDate
                        && !isHideSelf
                        && isShowInLang)
                    {
                        isValid = true;
                    }
                    else
                    {
                        logger.DebugFormat("The article (id:[{0}]) that client requires is disabled.", articleData.ArticleId);
                    }
                }

                if (isValid)
                {
                    try
                    {
                        articleData.ImportDataFrom(drArticle);

                        // get top level id's
                        DataSet dsTopLevelIds = artPub.GetArticleTopLevelIds(articleData.ArticleId.Value);

                        if (dsTopLevelIds != null && dsTopLevelIds.Tables[0].Rows.Count > 0)
                        {
                            DataRow drFirst = dsTopLevelIds.Tables[0].Rows[0];

                            if (!Convert.IsDBNull(drFirst["Lv1Id"]))
                            {
                                articleData.Lv1Id = (Guid)drFirst["Lv1Id"];
                            }

                            if (!Convert.IsDBNull(drFirst["Lv2Id"]))
                            {
                                articleData.Lv2Id = (Guid)drFirst["Lv2Id"];
                            }

                            if (!Convert.IsDBNull(drFirst["Lv3Id"]))
                            {
                                articleData.Lv3Id = (Guid)drFirst["Lv3Id"];
                            }
                        }
                        else
                        {
                            throw new Exception("dsTopLevelIds is empty");
                        }

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("Import data to ArticleData failed (id:[{0}]).", articleData.ArticleId), ex);
                    }
                }
            }
            else
            {
                logger.DebugFormat("Article data (id:[{0}]) is empty.", articleData.ArticleId);
            }

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
        /// Retrieve ArticleId and initial article data by querystring: preview, alias, artid
        /// </summary>
        public bool RetrieveArticleIdAndData(Guid articleId)
        {
            bool result = RetrieveArticleId(articleId);

            if (result)
            {
                result = RetrieveArticleData();
            }

            return result;
        }

        /// <summary>
        /// Retrieve ArticleId by querystring: preview, alias, artid
        /// </summary>
        public override bool RetrieveArticleId()
        {
            bool result = base.RetrieveArticleId();

            // get ArticleId by LinkUrl
            string linkUrl = Request.AppRelativeCurrentExecutionFilePath;
            articleData.ArticleId = artPub.GetArticleIdByLinkUrl(linkUrl);

            result = articleData.ArticleId.HasValue;

            if (!result)
            {
                logger.DebugFormat("can not get ArticleId of linkUrl[{0}]", linkUrl);
            }

            return result;
        }

        /// <summary>
        /// Retrieve ArticleId by querystring: preview, alias, artid
        /// </summary>
        public virtual bool RetrieveArticleId(Guid articleId)
        {
            bool result = false;

            if (qsPreview != null)
            {
                result = HandlePreviewToken();
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
