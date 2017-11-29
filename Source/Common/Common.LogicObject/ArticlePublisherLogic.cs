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
    public class ArticlePublisherLogic
    {
        protected ILog logger = null;
        protected string dbErrMsg = "";

        /// <summary>
        /// 網頁內容發佈(上稿)
        /// </summary>
        public ArticlePublisherLogic()
        {
            logger = LogManager.GetLogger(this.GetType());
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

        #endregion
    }
}
