// ===============================================================================
// DataAccessCommandInfo of ArticlePublisher of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// ArticlePublisher.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Common.DataAccess.ArticlePublisher
{
    #region 網頁內容

    /// <summary>
    /// 取得後台用網頁內容資料
    /// </summary>
    public class spArticle_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得後台用網頁內容的多國語系資料
    /// </summary>
    public class spArticleMultiLang_GetDataForbackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string CultureName;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleMultiLang_GetDataForbackend";
        }
    }

    /// <summary>
    /// 取得網頁內容最大排序編號
    /// </summary>
    public class spArticle_GetMaxSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ParentId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_GetMaxSortNo";
        }
    }

    /// <summary>
    /// 新增網頁內容
    /// </summary>
    public class spArticle_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public Guid ParentId;
        public string ArticleAlias;
        public string BannerPicFileName;
        public int LayoutModeId;
        public int ShowTypeId;
        public string LinkUrl;
        public string LinkTarget;
        public string ControlName;
        public string SubItemControlName;
        public bool IsHideSelf;
        public bool IsHideChild;
        public DateTime StartDate;
        public DateTime EndDate;
        public int SortNo;
        public bool DontDelete;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_InsertData";
        }
    }

    /// <summary>
    /// 新增網頁內容的多國語系資料
    /// </summary>
    public class spArticleMultiLang_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string CultureName;
        public string ArticleSubject;
        public string ArticleContext;
        public bool IsShowInLang;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleMultiLang_InsertData";
        }
    }

    /// <summary>
    /// 更新網頁內容
    /// </summary>
    public class spArticle_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string ArticleAlias;
        public string BannerPicFileName;
        public int LayoutModeId;
        public int ShowTypeId;
        public string LinkUrl;
        public string LinkTarget;
        public string ControlName;
        public string SubItemControlName;
        public bool IsHideSelf;
        public bool IsHideChild;
        public DateTime StartDate;
        public DateTime EndDate;
        public int SortNo;
        public bool DontDelete;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_UpdateData";
        }
    }

    /// <summary>
    /// 更新網頁內容的多國語系資料
    /// </summary>
    public class spArticleMultiLang_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string CultureName;
        public string ArticleSubject;
        public string ArticleContext;
        public bool IsShowInLang;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleMultiLang_UpdateData";
        }
    }

    #endregion
}
