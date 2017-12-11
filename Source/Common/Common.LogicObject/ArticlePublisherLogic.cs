﻿// ===============================================================================
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
    public class ArticlePublisherLogic : ICustomEmployeeAuthorizationResult
    {
        protected ILog logger = null;
        protected string dbErrMsg = "";
        protected IAuthenticationConditionProvider authCondition;

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
            spArticleMultiLang_GetDataForBackend cmdInfo = new spArticleMultiLang_GetDataForBackend()
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

        /// <summary>
        /// 取得後台用指定語系的網頁內容清單
        /// </summary>
        public DataSet GetArticleMultiLangListForBackend(ArticleListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticleMultiLang_GetListForBackend cmdInfo = new spArticleMultiLang_GetListForBackend()
            {
                ParentId = param.ParentId,
                CultureName = param.CultureName,
                Kw = param.Kw,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc,
                CanReadSubItemOfOthers = param.AuthParams.CanReadSubItemOfOthers,
                CanReadSubItemOfCrew = param.AuthParams.CanReadSubItemOfCrew,
                CanReadSubItemOfSelf = param.AuthParams.CanReadSubItemOfSelf,
                MyAccount = param.AuthParams.MyAccount,
                MyDeptId = param.AuthParams.MyDeptId
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();
            param.PagedParams.RowCount = cmdInfo.RowCount;

            return ds;
        }

        /// <summary>
        /// 刪除網頁內容
        /// </summary>
        public bool DeleteArticleData(Guid articleId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_DeleteData cmdInfo = new spArticle_DeleteData() { ArticleId = articleId };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 加大網頁內容的排序編號
        /// </summary>
        public bool IncreaseArticleSortNo(Guid articleId, string mdfAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_IncreaseSortNo cmdInfo = new spArticle_IncreaseSortNo()
            {
                ArticleId = articleId,
                MdfAccount = mdfAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 減小網頁內容的排序編號
        /// </summary>
        public bool DecreaseArticleSortNo(Guid articleId, string mdfAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticle_DecreaseSortNo cmdInfo = new spArticle_DecreaseSortNo()
            {
                ArticleId = articleId,
                MdfAccount = mdfAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 取得指定語系的網頁內容階層資料
        /// </summary>
        public DataSet GetArticleMultiLangLevelInfo(Guid articleId, string cultureName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticleMultiLang_GetLevelInfo cmdInfo = new spArticleMultiLang_GetLevelInfo()
            {
                ArticleId = articleId,
                CultureName = cultureName
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        #endregion

        #region AttachFile DataAccess functions

        /// <summary>
        /// 取得後台用附件檔案資料
        /// </summary>
        public DataSet GetAttachFileDataForBackend(Guid attId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_GetDataForBackend cmdInfo = new spAttachFile_GetDataForBackend() { AttId = attId };

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得後台用附件檔案的多國語系資料
        /// </summary>
        public DataSet GetAttachFileMultiLangDataForBackend(Guid attId, string cultureName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFileMultiLang_GetDataForBackend cmdInfo = new spAttachFileMultiLang_GetDataForBackend()
            {
                AttId = attId,
                CultureName = cultureName
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得附件檔案的最大排序編號
        /// </summary>
        public int GetAttachFileMaxSortNo(Guid? articleId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_GetMaxSortNo cmdInfo = new spAttachFile_GetMaxSortNo() { ArticleId = articleId };

            int errCode = -1;
            int result = cmd.ExecuteScalar<int>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增附件檔案資料
        /// </summary>
        public bool InsertAttachFileData(AttachFileParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_InsertData cmdInfo = new spAttachFile_InsertData()
            {
                AttId = param.AttId,
                ArticleId = param.ArticleId,
                FilePath = param.FilePath,
                FileSavedName = param.FileSavedName,
                FileSize = param.FileSize,
                SortNo = param.SortNo,
                FileMIME = param.FileMIME,
                DontDelete = param.DontDelete,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增附件檔案的多國語系資料
        /// </summary>
        public bool InsertAttachFileMultiLangData(AttachFileMultiLangParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFileMultiLang_InsertData cmdInfo = new spAttachFileMultiLang_InsertData()
            {
                AttId = param.AttId,
                CultureName = param.CultureName,
                AttSubject = param.AttSubject,
                IsShowInLang = param.IsShowInLang,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 更新附件檔案資料
        /// </summary>
        public bool UpdateAttachFileData(AttachFileParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_UpdateData cmdInfo = new spAttachFile_UpdateData()
            {
                AttId = param.AttId,
                FilePath = param.FilePath,
                FileSavedName = param.FileSavedName,
                FileSize = param.FileSize,
                SortNo = param.SortNo,
                FileMIME = param.FileMIME,
                DontDelete = param.DontDelete,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 更新附件檔案的多國語系資料
        /// </summary>
        public bool UpdateAttachFileMultiLangData(AttachFileMultiLangParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFileMultiLang_UpdateData cmdInfo = new spAttachFileMultiLang_UpdateData()
            {
                AttId = param.AttId,
                CultureName = param.CultureName,
                AttSubject = param.AttSubject,
                IsShowInLang = param.IsShowInLang,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 刪除附件檔案資料
        /// </summary>
        public bool DeleteAttachFileData(Guid attId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_DeleteData cmdInfo = new spAttachFile_DeleteData() { AttId = attId };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 取得後台用指定語系的附件檔案清單
        /// </summary>
        public DataSet GetAttachFileMultiLangListForBackend(AttachFileListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFileMultiLang_GetListForBackend cmdInfo = new spAttachFileMultiLang_GetListForBackend()
            {
                ArticleId = param.ArticleId,
                CultureName = param.CultureName,
                Kw = param.Kw,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc,
                CanReadSubItemOfOthers = param.AuthParams.CanReadSubItemOfOthers,
                CanReadSubItemOfCrew = param.AuthParams.CanReadSubItemOfCrew,
                CanReadSubItemOfSelf = param.AuthParams.CanReadSubItemOfSelf,
                MyAccount = param.AuthParams.MyAccount,
                MyDeptId = param.AuthParams.MyDeptId
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();
            param.PagedParams.RowCount = cmdInfo.RowCount;

            return ds;
        }

        /// <summary>
        /// 加大附件檔案的排序編號
        /// </summary>
        public bool IncreaseAttachFileSortNo(Guid attId, string mdfAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_IncreaseSortNo cmdInfo = new spAttachFile_IncreaseSortNo()
            {
                AttId = attId,
                MdfAccount = mdfAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 減小附件檔案的排序編號
        /// </summary>
        public bool DecreaseAttachFileSortNo(Guid attId, string mdfAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFile_DecreaseSortNo cmdInfo = new spAttachFile_DecreaseSortNo()
            {
                AttId = attId,
                MdfAccount = mdfAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 增加附件檔案的多國語系資料被點閱次數
        /// </summary>
        public bool IncreaseAttachFileMultiLangReadCount(Guid attId, string cultureName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spAttachFileMultiLang_IncreaseReadCount cmdInfo = new spAttachFileMultiLang_IncreaseReadCount()
            {
                AttId = attId,
                CultureName = cultureName
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region ArticlePicture DataAccess functions

        /// <summary>
        /// 取得後台用網頁照片資料
        /// </summary>
        public DataSet GetArticlePictureDataForBackend(Guid picId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePicture_GetDataForBackend cmdInfo = new spArticlePicture_GetDataForBackend() { PicId = picId };

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得後台用網頁照片的多國語系資料
        /// </summary>
        public DataSet GetArticlePictureMultiLangDataForBackend(Guid picId, string cultureName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePictureMultiLang_GetDataForBackend cmdInfo = new spArticlePictureMultiLang_GetDataForBackend()
            {
                PicId = picId,
                CultureName = cultureName
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得網頁照片的最大排序編號
        /// </summary>
        public int GetArticlePictureMaxSortNo(Guid? articleId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePicture_GetMaxSortNo cmdInfo = new spArticlePicture_GetMaxSortNo() { ArticleId = articleId };

            int errCode = -1;
            int result = cmd.ExecuteScalar<int>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 刪除網頁照片資料
        /// </summary>
        public bool DeleteArticlePictureData(Guid picId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePicture_DeleteData cmdInfo = new spArticlePicture_DeleteData() { PicId = picId };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增網頁照片資料
        /// </summary>
        public bool InsertArticlePictureData(ArticlePictureParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePicture_InsertData cmdInfo = new spArticlePicture_InsertData()
            {
                PicId = param.PicId,
                ArticleId = param.ArticleId,
                FileSavedName = param.FileSavedName,
                FileSize = param.FileSize,
                SortNo = param.SortNo,
                FileMIME = param.FileMIME,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增網頁照片的多國語系資料
        /// </summary>
        public bool InsertArticlePictureMultiLangData(ArticlePictureMultiLangParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePictureMultiLang_InsertData cmdInfo = new spArticlePictureMultiLang_InsertData()
            {
                PicId = param.PicId,
                CultureName = param.CultureName,
                PicSubject = param.PicSubject,
                IsShowInLang = param.IsShowInLang,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 更新網頁照片資料
        /// </summary>
        public bool UpdateArticlePictureData(ArticlePictureParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePicture_UpdateData cmdInfo = new spArticlePicture_UpdateData()
            {
                PicId = param.PicId,
                FileSavedName = param.FileSavedName,
                FileSize = param.FileSize,
                SortNo = param.SortNo,
                FileMIME = param.FileMIME,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 更新網頁照片的多國語系資料
        /// </summary>
        public bool UpdateArticlePictureMultiLangData(ArticlePictureMultiLangParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePictureMultiLang_UpdateData cmdInfo = new spArticlePictureMultiLang_UpdateData()
            {
                PicId = param.PicId,
                CultureName = param.CultureName,
                PicSubject = param.PicSubject,
                IsShowInLang = param.IsShowInLang,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 取得後台用指定語系的網頁照片清單
        /// </summary>
        public DataSet GetArticlePicutreMultiLangListForBackend(ArticlePictureListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spArticlePictureMultiLang_GetListForBackend cmdInfo = new spArticlePictureMultiLang_GetListForBackend()
            {
                ArticleId = param.ArticleId,
                CultureName = param.CultureName,
                Kw = param.Kw,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc,
                CanReadSubItemOfOthers = param.AuthParams.CanReadSubItemOfOthers,
                CanReadSubItemOfCrew = param.AuthParams.CanReadSubItemOfCrew,
                CanReadSubItemOfSelf = param.AuthParams.CanReadSubItemOfSelf,
                MyAccount = param.AuthParams.MyAccount,
                MyDeptId = param.AuthParams.MyDeptId
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();
            param.PagedParams.RowCount = cmdInfo.RowCount;

            return ds;
        }

        #endregion

        /// <summary>
        /// 從資料集載入身分的授權設定
        /// </summary>
        protected EmployeeAuthorizations LoadRoleAuthorizationsFromDataSet(EmployeeAuthorizations authorizations, DataSet dsRoleOp, bool isRoleAdmin)
        {
            if (isRoleAdmin)
            {
                // admin, open all
                authorizations.CanRead = true;
                authorizations.CanEdit = true;

                authorizations.CanReadSubItemOfSelf = true;
                authorizations.CanEditSubItemOfSelf = true;
                authorizations.CanAddSubItemOfSelf = true;
                authorizations.CanDelSubItemOfSelf = true;

                authorizations.CanReadSubItemOfCrew = true;
                authorizations.CanEditSubItemOfCrew = true;
                authorizations.CanDelSubItemOfCrew = true;

                authorizations.CanReadSubItemOfOthers = true;
                authorizations.CanEditSubItemOfOthers = true;
                authorizations.CanDelSubItemOfOthers = true;
            }
            else
            {
                if (dsRoleOp == null || dsRoleOp.Tables[0].Rows.Count == 0)
                {
                    // no data, close all
                    authorizations.CanRead = false;
                    authorizations.CanEdit = false;

                    authorizations.CanReadSubItemOfSelf = false;
                    authorizations.CanEditSubItemOfSelf = false;
                    authorizations.CanAddSubItemOfSelf = false;
                    authorizations.CanDelSubItemOfSelf = false;

                    authorizations.CanReadSubItemOfCrew = false;
                    authorizations.CanEditSubItemOfCrew = false;
                    authorizations.CanDelSubItemOfCrew = false;

                    authorizations.CanReadSubItemOfOthers = false;
                    authorizations.CanEditSubItemOfOthers = false;
                    authorizations.CanDelSubItemOfOthers = false;
                }
                else
                {
                    DataRow drRoleOp = dsRoleOp.Tables[0].Rows[0];

                    // load settings
                    authorizations.CanRead = Convert.ToBoolean(drRoleOp["CanRead"]);
                    authorizations.CanEdit = Convert.ToBoolean(drRoleOp["CanEdit"]);

                    authorizations.CanReadSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanReadSubItemOfSelf"]);
                    authorizations.CanEditSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanEditSubItemOfSelf"]);
                    authorizations.CanAddSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanAddSubItemOfSelf"]);
                    authorizations.CanDelSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanDelSubItemOfSelf"]);

                    authorizations.CanReadSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanReadSubItemOfCrew"]);
                    authorizations.CanEditSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanEditSubItemOfCrew"]);
                    authorizations.CanDelSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanDelSubItemOfCrew"]);

                    authorizations.CanReadSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanReadSubItemOfOthers"]);
                    authorizations.CanEditSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanEditSubItemOfOthers"]);
                    authorizations.CanDelSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanDelSubItemOfOthers"]);
                }
            }

            return authorizations;
        }

        #region ICustomEmployeeAuthorizationResult

        public EmployeeAuthorizationsWithOwnerInfoOfDataExamined InitialAuthorizationResult(bool isTopPageOfOperation, EmployeeAuthorizations authorizations)
        {
            EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = new EmployeeAuthorizationsWithOwnerInfoOfDataExamined(authorizations);

            bool gotOpAuth = false;
            Guid initArticleId= authCondition.GetArticleId();
            Guid curArticleId = initArticleId;
            Guid curParentId = Guid.Empty;
            int curArticleLevelNo;
            string linkUrl = "";
            bool isRoot = false;
            bool isRoleAdmin = authCondition.IsInRole("admin");

            // get article info
            DataSet dsArticle = GetArticleDataForBackend(curArticleId);

            if (dsArticle != null && dsArticle.Tables[0].Rows.Count > 0)
            {
                DataRow drArticle = dsArticle.Tables[0].Rows[0];

                if (Convert.IsDBNull(drArticle["ParentId"]))
                {
                    isRoot = true;
                }
                else
                {
                    curParentId = new Guid(drArticle.ToSafeStr("ParentId"));
                }

                curArticleLevelNo = Convert.ToInt32(drArticle["ArticleLevelNo"]);

                authAndOwner.OwnerAccountOfDataExamined = drArticle.ToSafeStr("PostAccount");
                authAndOwner.OwnerDeptIdOfDataExamined = Convert.ToInt32(drArticle["PostDeptId"]);
            }

            if (isRoot || isRoleAdmin)
            {
                return authAndOwner;
            }

            do
            {
                // get opId by LinkUrl
                linkUrl = string.Format("Article-Node.aspx?artid={0}", curArticleId);

                IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
                Common.DataAccess.EmployeeAuthority.spOperations_GetOpInfoByLinkUrl opCmdInfo = new DataAccess.EmployeeAuthority.spOperations_GetOpInfoByLinkUrl()
                {
                    LinkUrl = linkUrl
                };
                DataSet dsOpInfo = cmd.ExecuteDataset(opCmdInfo);

                if (dsOpInfo != null && dsOpInfo.Tables[0].Rows.Count > 0)
                {
                    DataRow drOpInfo = dsOpInfo.Tables[0].Rows[0];
                    int opId = Convert.ToInt32(drOpInfo["OpId"]);

                    // get authorizations

                    Common.DataAccess.EmployeeAuthority.spEmployeeRoleOperationsDesc_GetDataOfOp cmdInfo = new DataAccess.EmployeeAuthority.spEmployeeRoleOperationsDesc_GetDataOfOp()
                    {
                        OpId = opId,
                        RoleName = authCondition.GetRoleName()
                    };
                    DataSet dsRoleOp = cmd.ExecuteDataset(cmdInfo);

                    if (dsRoleOp != null && dsRoleOp.Tables[0].Rows.Count > 0)
                    {
                        authAndOwner = (EmployeeAuthorizationsWithOwnerInfoOfDataExamined)LoadRoleAuthorizationsFromDataSet(authAndOwner, dsRoleOp, isRoleAdmin);
                    }
                    else
                    {
                        authAndOwner = (EmployeeAuthorizationsWithOwnerInfoOfDataExamined)LoadRoleAuthorizationsFromDataSet(authAndOwner, null, isRoleAdmin);
                    }

                    gotOpAuth = true;
                }
                else
                {
                    if (curParentId == Guid.Empty)
                    {
                        // parent is root
                        break;
                    }

                    // get parent info
                    DataSet dsParent = GetArticleDataForBackend(curParentId);

                    if (dsParent == null || dsParent.Tables[0].Rows.Count == 0)
                    {
                        logger.Error(string.Format("can not get article data of {0}", curParentId));
                        break;
                    }

                    // move to parent level
                    DataRow drParent = dsParent.Tables[0].Rows[0];
                    curArticleId = curParentId;
                    curParentId = new Guid(drParent.ToSafeStr("ParentId"));
                    curArticleLevelNo = Convert.ToInt32(drParent["ArticleLevelNo"]);
                }
            } while (!gotOpAuth);

            if (isTopPageOfOperation && curArticleId != initArticleId)
            {
                // notice that the authorizations belong to parent, so this page is not top page of operation.
                authAndOwner.IsTopPageOfOperation = false;
                authAndOwner.IsTopPageOfOperationChanged = true;
            }

            return authAndOwner;
        }

        #endregion
    }
}
