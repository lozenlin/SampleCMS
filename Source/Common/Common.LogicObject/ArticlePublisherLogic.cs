// ===============================================================================
// ArticlePublisherLogic of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// ArticlePublisherLogic.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using Common.DataAccess;
using Common.DataAccess.ArticlePublisher;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// 網頁內容發佈(上稿)
    /// </summary>
    public class ArticlePublisherLogic : IAuthenticationConditionProvider, ICustomEmployeeAuthorizationResult
    {
        protected ILog logger = null;
        protected string dbErrMsg = "";
        protected IAuthenticationConditionProvider authCondition;
        /// <summary>
        /// 後台網頁所屬的作業代碼
        /// </summary>
        protected int opIdOfPage;

        /// <summary>
        /// 網頁內容發佈(上稿)
        /// </summary>
        public ArticlePublisherLogic(IAuthenticationConditionProvider authCondition)
        {
            logger = LogManager.GetLogger(this.GetType());
            this.authCondition = authCondition;
        }

        // DataAccess functions

        /// <summary>
        /// DB command 執行後的錯誤訊息
        /// </summary>
        public string GetDbErrMsg()
        {
            return dbErrMsg;
        }

        #region Article DataAccess functions

        /// <summary>
        /// 取得後台用網頁內容資料
        /// </summary>
        public DataSet GetArticleDataForBackend(Guid articleId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_GetDataForBackend cmdInfo = new spArticle_GetDataForBackend() { ArticleId = articleId };

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得後台用網頁內容的多國語系資料
        /// </summary>
        public DataSet GetArticleMultiLangDataForBackend(Guid articleId, string cultureName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticleMultiLang_GetDataForbackend cmdInfo = new spArticleMultiLang_GetDataForbackend()
            {
                ArticleId = articleId,
                CultureName = cultureName
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得網頁內容最大排序編號
        /// </summary>
        public int GetArticleMaxSortNo(Guid parentId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_GetMaxSortNo cmdInfo = new spArticle_GetMaxSortNo() { ParentId = parentId };

            int errCode = -1;
            int result = cmd.ExecuteScalar<int>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增網頁內容
        /// </summary>
        public bool InsertArticleData(ArticleParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_InsertData cmdInfo = new spArticle_InsertData()
            {
                ArticleId = param.ArticleId,
                ParentId = param.ParentId,
                ArticleAlias = param.ArticleAlias,
                BannerPicFileName = param.BannerPicFileName,
                LayoutModeId = param.LayoutModeId,
                ShowTypeId = param.ShowTypeId,
                LinkUrl = param.LinkUrl,
                LinkTarget = param.LinkTarget,
                ControlName = param.ControlName,
                SubItemControlName = param.SubItemControlName,
                IsHideSelf = param.IsHideSelf,
                IsHideChild = param.IsHideChild,
                StartDate = param.StartDate,
                EndDate = param.EndDate,
                SortNo = param.SortNo,
                DontDelete = param.DontDelete,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (!result)
            {
                if (cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 2)
                {
                    param.HasIdBeenUsed = true;
                }
                else if (cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 3)
                {
                    param.HasAliasBeenUsed = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 新增網頁內容的多國語系資料
        /// </summary>
        public bool InsertArticleMultiLangData(ArticleMultiLangParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticleMultiLang_InsertData cmdInfo = new spArticleMultiLang_InsertData()
            {
                ArticleId = param.ArticleId,
                CultureName = param.CultureName,
                ArticleSubject = param.ArticleSubject,
                ArticleContext = param.ArticleContext,
                IsShowInLang = param.IsShowInLang,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 更新網頁內容
        /// </summary>
        public bool UpdateArticleData(ArticleParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_UpdateData cmdInfo = new spArticle_UpdateData()
            {
                ArticleId = param.ArticleId,
                ArticleAlias = param.ArticleAlias,
                BannerPicFileName = param.BannerPicFileName,
                LayoutModeId = param.LayoutModeId,
                ShowTypeId = param.ShowTypeId,
                LinkUrl = param.LinkUrl,
                LinkTarget = param.LinkTarget,
                ControlName = param.ControlName,
                SubItemControlName = param.SubItemControlName,
                IsHideSelf = param.IsHideSelf,
                IsHideChild = param.IsHideChild,
                StartDate = param.StartDate,
                EndDate = param.EndDate,
                SortNo = param.SortNo,
                DontDelete = param.DontDelete,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (!result)
            {
                if (cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 3)
                {
                    param.HasAliasBeenUsed = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 更新網頁內容的多國語系資料
        /// </summary>
        public bool UpdateArticleMultiLangData(ArticleMultiLangParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticleMultiLang_UpdateData cmdInfo = new spArticleMultiLang_UpdateData()
            {
                ArticleId = param.ArticleId,
                CultureName = param.CultureName,
                ArticleSubject = param.ArticleSubject,
                ArticleContext = param.ArticleContext,
                IsShowInLang = param.IsShowInLang,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region IAuthenticationConditionProvider

        /// <summary>
        /// 取得後台網頁所屬的作業代碼
        /// </summary>
        public int GetOpIdOfPage()
        {
            if (opIdOfPage < 1)
            {
                bool gotOpId = false;
                Guid curArticleId = authCondition.GetArticleId();
                Guid curParentId;
                int curArticleLevelNo;
                bool isParentIdNull = false;

                // get article info
                DataSet dsArticle = GetArticleDataForBackend(curArticleId);

                if (dsArticle != null && dsArticle.Tables[0].Rows.Count > 0)
                {
                    DataRow drArticle = dsArticle.Tables[0].Rows[0];

                    if (Convert.IsDBNull(drArticle["ParentId"]))
                    {
                        isParentIdNull = true;
                    }
                    else
                    {
                        curParentId = new Guid(drArticle.ToSafeStr("ParentId"));
                    }

                    curArticleLevelNo = Convert.ToInt32(drArticle["ArticleLevelNo"]);
                }

                if (!isParentIdNull)
                {
                    //todo by lozen, get opId by LinkUrl


                    //if (dsArticle == null || dsArticle.Tables[0].Rows.Count == 0)
                    //{
                    //    logger.Error(string.Format("can not get article data of {0}", curArticleId));
                    //    // break;
                    //}

                    //DataRow drArticle = dsArticle.Tables[0].Rows[0];
                    //curParentId = new Guid(drArticle.ToSafeStr("ParentId"));
                    //curArticleLevelNo = Convert.ToInt32(drArticle["ArticleLevelNo"]);
                }

                if (!gotOpId)
                {
                    opIdOfPage = authCondition.GetOpIdOfPage();
                }
            }

            return opIdOfPage;
        }

        public string GetEmpAccount()
        {
            return authCondition.GetEmpAccount();
        }

        public string GetRoleName()
        {
            return authCondition.GetRoleName();
        }

        public bool IsInRole(string roleName)
        {
            return authCondition.IsInRole(roleName);
        }

        public int GetDeptId()
        {
            return authCondition.GetDeptId();
        }

        public Guid GetArticleId()
        {
            return authCondition.GetArticleId();
        }

        #endregion

        #region ICustomEmployeeAuthorizationResult

        public EmployeeAuthorizationsWithOwnerInfoOfDataExamined InitialAuthorizationResult(bool isTopPageOfOperation, EmployeeAuthorizations authorizations)
        {
            EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = new EmployeeAuthorizationsWithOwnerInfoOfDataExamined(authorizations);

            //todo by lozen

            return authAndOwner;
        }

        #endregion
    }
}
