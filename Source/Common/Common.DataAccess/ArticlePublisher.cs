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
    public class spArticleMultiLang_GetDataForBackend : IDataAccessCommandInfo
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
            return "dbo.spArticleMultiLang_GetDataForBackend";
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

    /// <summary>
    /// 取得後台用指定語系的網頁內容清單
    /// </summary>
    public class spArticleMultiLang_GetListForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ParentId;
        public string CultureName;
        public string Kw;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public bool CanReadSubItemOfOthers;
        public bool CanReadSubItemOfCrew;
        public bool CanReadSubItemOfSelf;
        public string MyAccount;
        public int MyDeptId;
        [OutputPara]
        public int RowCount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleMultiLang_GetListForBackend";
        }
    }

    /// <summary>
    /// 刪除網頁內容
    /// </summary>
    public class spArticle_DeleteData : IDataAccessCommandInfo
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
            return "dbo.spArticle_DeleteData";
        }
    }

    /// <summary>
    /// 加大網頁內容的排序編號
    /// </summary>
    public class spArticle_IncreaseSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_IncreaseSortNo";
        }
    }

    /// <summary>
    /// 減小網頁內容的排序編號
    /// </summary>
    public class spArticle_DecreaseSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_DecreaseSortNo";
        }
    }

    /// <summary>
    /// 取得指定語系的網頁內容階層資料
    /// </summary>
    public class spArticleMultiLang_GetLevelInfo : IDataAccessCommandInfo
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
            return "dbo.spArticleMultiLang_GetLevelInfo";
        }
    }

    #endregion

    #region 附件檔案

    /// <summary>
    /// 取得後台用附件檔案資料
    /// </summary>
    public class spAttachFile_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFile_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得後台用附件檔案的多國語系資料
    /// </summary>
    public class spAttachFileMultiLang_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public string CultureName;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFileMultiLang_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得附件檔案的最大排序編號
    /// </summary>
    public class spAttachFile_GetMaxSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid? ArticleId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFile_GetMaxSortNo";
        }
    }

    #endregion
}
