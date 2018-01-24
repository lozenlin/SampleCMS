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

using Common.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public string ERROR_PAGE;

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

        #endregion

        protected ArticlePublisherLogic artPub;
        protected ArticleData articleData;
        protected bool isPreviewMode = false;
        protected string aesKeyOfFP = "";   // 16 letters
        protected string aesKeyOfBP = "";   // 16 letters
        protected string basicIV = "";  // 16 letters

        /// <summary>
        /// 前台網頁的共用元件
        /// </summary>
        public FrontendPageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
            artPub = new ArticlePublisherLogic(null);
            articleData = new ArticleData();

            ERROR_PAGE = string.Format("ErrorPage.aspx?l={0}", qsLangNo);
            aesKeyOfFP = ConfigurationManager.AppSettings["AesKeyOfFP"];
            aesKeyOfBP = ConfigurationManager.AppSettings["AesKeyOfBP"];
            basicIV = ConfigurationManager.AppSettings["AesIV"];
        }

        #region Public methods

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

        public ArticleData GetArticleData()
        {
            return articleData;
        }

        public bool GetIsPreviewMode()
        {
            return isPreviewMode;
        }

        #endregion

        protected bool HandlePreviewToken()
        {
            bool result = false;

            if (qsPreview == null)
                return false;

            if (qsPreview == "1")
            {
                // redirect to back-stage to get authorization
                string websiteUrl = ConfigurationManager.AppSettings["WebsiteUrl"];
                string backendSsoAuthenticatorUrl = ConfigurationManager.AppSettings["BackendSsoAuthenticatorUrl"];

                if (string.IsNullOrEmpty(backendSsoAuthenticatorUrl))
                {
                    logger.Error("Invalid AppSettings/BackendSsoAuthenticatorUrl");
                    return false;
                }

                string valueInToken = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string token = AesUtility.Encrypt(valueInToken, aesKeyOfFP, basicIV);
                string location = websiteUrl + "/" + Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "");
                string url = StringUtility.SetParaValueInUrl(backendSsoAuthenticatorUrl, "token", Server.UrlEncode(token));
                url = StringUtility.SetParaValueInUrl(url, "location", Server.UrlEncode(location));
                url = AppendCurrentQueryString(url);
                Response.Redirect(url);
            }
            else
            {
                try
                {
                    // decrypt token
                    string valueInToken = AesUtility.Decrypt(qsPreview, aesKeyOfBP, basicIV);
                    PreviewArticle previewArticle = JsonConvert.DeserializeObject<PreviewArticle>(valueInToken);

                    if (!string.IsNullOrEmpty(previewArticle.EmpAccount))
                    {
                        if (DateTime.Now <= previewArticle.ValidTime)
                        {
                            articleData.ArticleId = new Guid(previewArticle.ArticleId);
                            result = true;
                            isPreviewMode = true;

                            logger.DebugFormat("{0} previews {1} (id:[{2}])(lang:{3}).",
                                previewArticle.EmpAccount,
                                Request.AppRelativeCurrentExecutionFilePath,
                                previewArticle.ArticleId,
                                qsLangNo);
                        }
                        else
                        {
                            logger.InfoFormat("{0} previews {1} but exceed valid time.", previewArticle.EmpAccount, Request.AppRelativeCurrentExecutionFilePath);
                        }
                    }
                    else
                    {
                        logger.InfoFormat("user previews {0} but not logged in.", Request.AppRelativeCurrentExecutionFilePath);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("", ex);
                }
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
                        articleData.IsPreviewMode = isPreviewMode;

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

    /// <summary>
    /// 搜尋網頁的共用元件
    /// </summary>
    public class SearchPageCommon : OtherArticlePageCommon
    {
        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// 搜尋用關鍵字
        /// </summary>
        public string qsQueryKeyword
        {
            get
            {
                return QueryStringToSafeStr("q") ?? "";
            }
        }

        #endregion

        public SearchPageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }

        /// <summary>
        /// 到搜尋結果頁
        /// </summary>
        public void GoToSearchResult(string keyword)
        {
            char[] separators = new char[] { ',', ';', ' ', '　' };
            bool isMultiKeyword = false;

            //檢查是否為多項關鍵字
            foreach (char symbol in separators)
            {
                if (keyword.Contains(symbol.ToString()))
                {
                    isMultiKeyword = true;
                }
            }

            //更新搜尋關鍵字
            if (isMultiKeyword)
            {
                string[] tokens = keyword.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (string token in tokens)
                {
                    artPub.SaveKeywordData(qsCultureNameOfLangNo, token);
                }
            }
            else
            {
                artPub.SaveKeywordData(qsCultureNameOfLangNo, keyword);
            }

            Response.Redirect(BuildSearchResultUrl(keyword));
        }

        /// <summary>
        /// 建立搜尋結果頁網址
        /// </summary>
        public virtual string BuildSearchResultUrl(string keyword)
        {
            return string.Format("~/Search-Result.aspx?q={0}&l={1}", Server.UrlEncode(keyword), qsLangNo);
        }
    }
}
