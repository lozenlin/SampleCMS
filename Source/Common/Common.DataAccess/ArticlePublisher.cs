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
    /// 取得前台用網頁內容資料
    /// </summary>
    public class spArticle_GetDataForFrontend : IDataAccessCommandInfo
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
            return "dbo.spArticle_GetDataForFrontend";
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
        public bool SubjectAtBannerArea;
        public DateTime PublishDate;
        public bool IsShowInUnitArea;
        public bool IsShowInSitemap;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public bool IsListAreaShowInFrontStage;
        public bool IsAttAreaShowInFrontStage;
        public bool IsPicAreaShowInFrontStage;
        public bool IsVideoAreaShowInFrontStage;
        public string SubItemLinkUrl;


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
        public string Subtitle;
        public string PublisherName;

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
        public bool SubjectAtBannerArea;
        public DateTime PublishDate;
        public bool IsShowInUnitArea;
        public bool IsShowInSitemap;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public bool IsListAreaShowInFrontStage;
        public bool IsAttAreaShowInFrontStage;
        public bool IsPicAreaShowInFrontStage;
        public bool IsVideoAreaShowInFrontStage;
        public string SubItemLinkUrl;

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
        public string Subtitle;
        public string PublisherName;

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

    /// <summary>
    /// 更新網頁內容的指定區域是否在前台顯示
    /// </summary>
    public class spArticle_UpdateIsAreaShowInFrontStage : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string AreaName;
        public bool IsShowInFrontStage;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_UpdateIsAreaShowInFrontStage";
        }
    }

    /// <summary>
    /// 更新網頁內容的前台子項目排序欄位
    /// </summary>
    public class spArticle_UpdateSortFieldOfFrontStage : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_UpdateSortFieldOfFrontStage";
        }
    }

    /// <summary>
    /// 依網址別名取得網頁代碼
    /// </summary>
    public class spArticle_GetArticleIdByAlias : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string ArticleAlias;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_GetArticleIdByAlias";
        }
    }

    /// <summary>
    /// 依超連結網址取得網頁代碼
    /// </summary>
    public class spArticle_GetArticleIdByLinkUrl : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string LinkUrl;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticle_GetArticleIdByLinkUrl";
        }
    }

    /// <summary>
    /// 取得指定網頁內容的前幾層網頁代碼
    /// </summary>
    public class spArticle_GetTopLevelIds : IDataAccessCommandInfo
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
            return "dbo.spArticle_GetTopLevelIds";
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

    /// <summary>
    /// 新增附件檔案資料
    /// </summary>
    public class spAttachFile_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public Guid ArticleId;
        public string FilePath;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public bool DontDelete;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFile_InsertData";
        }
    }

    /// <summary>
    /// 新增附件檔案的多國語系資料
    /// </summary>
    public class spAttachFileMultiLang_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public string CultureName;
        public string AttSubject;
        public bool IsShowInLang;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFileMultiLang_InsertData";
        }
    }

    /// <summary>
    /// 更新附件檔案資料
    /// </summary>
    public class spAttachFile_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public string FilePath;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public bool DontDelete;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFile_UpdateData";
        }
    }

    /// <summary>
    /// 更新附件檔案的多國語系資料
    /// </summary>
    public class spAttachFileMultiLang_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public string CultureName;
        public string AttSubject;
        public bool IsShowInLang;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFileMultiLang_UpdateData";
        }
    }

    /// <summary>
    /// 刪除附件檔案資料
    /// </summary>
    public class spAttachFile_DeleteData : IDataAccessCommandInfo
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
            return "dbo.spAttachFile_DeleteData";
        }
    }

    /// <summary>
    /// 取得後台用指定語系的附件檔案清單
    /// </summary>
    public class spAttachFileMultiLang_GetListForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
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
            return "dbo.spAttachFileMultiLang_GetListForBackend";
        }
    }

    /// <summary>
    /// 加大附件檔案的排序編號
    /// </summary>
    public class spAttachFile_IncreaseSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFile_IncreaseSortNo";
        }
    }

    /// <summary>
    /// 減小附件檔案的排序編號
    /// </summary>
    public class spAttachFile_DecreaseSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid AttId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spAttachFile_DecreaseSortNo";
        }
    }

    /// <summary>
    /// 增加附件檔案的多國語系資料被點閱次數
    /// </summary>
    public class spAttachFileMultiLang_IncreaseReadCount : IDataAccessCommandInfo
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
            return "dbo.spAttachFileMultiLang_IncreaseReadCount";
        }
    }

    #endregion

    #region 網頁照片

    /// <summary>
    /// 取得後台用網頁照片資料
    /// </summary>
    public class spArticlePicture_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePicture_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得後台用網頁照片的多國語系資料
    /// </summary>
    public class spArticlePictureMultiLang_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;
        public string CultureName;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePictureMultiLang_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得網頁照片的最大排序編號
    /// </summary>
    public class spArticlePicture_GetMaxSortNo : IDataAccessCommandInfo
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
            return "dbo.spArticlePicture_GetMaxSortNo";
        }
    }

    /// <summary>
    /// 刪除網頁照片資料
    /// </summary>
    public class spArticlePicture_DeleteData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePicture_DeleteData";
        }
    }

    /// <summary>
    /// 新增網頁照片資料
    /// </summary>
    public class spArticlePicture_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;
        public Guid ArticleId;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePicture_InsertData";
        }
    }

    /// <summary>
    /// 新增網頁照片的多國語系資料
    /// </summary>
    public class spArticlePictureMultiLang_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;
        public string CultureName;
        public string PicSubject;
        public bool IsShowInLang;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePictureMultiLang_InsertData";
        }
    }

    /// <summary>
    /// 更新網頁照片資料
    /// </summary>
    public class spArticlePicture_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePicture_UpdateData";
        }
    }

    /// <summary>
    /// 更新網頁照片的多國語系資料
    /// </summary>
    public class spArticlePictureMultiLang_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid PicId;
        public string CultureName;
        public string PicSubject;
        public bool IsShowInLang;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticlePictureMultiLang_UpdateData";
        }
    }

    /// <summary>
    /// 取得後台用指定語系的網頁照片清單
    /// </summary>
    public class spArticlePictureMultiLang_GetListForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
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
            return "dbo.spArticlePictureMultiLang_GetListForBackend";
        }
    }

    #endregion

    #region 網頁影片

    /// <summary>
    /// 取得後台用網頁影片資料
    /// </summary>
    public class spArticleVideo_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideo_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得後台用網頁影片的多國語系資料
    /// </summary>
    public class spArticleVideoMultiLang_GetDataForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;
        public string CultureName;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideoMultiLang_GetDataForBackend";
        }
    }

    /// <summary>
    /// 取得網頁影片的最大排序編號
    /// </summary>
    public class spArticleVideo_GetMaxSortNo : IDataAccessCommandInfo
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
            return "dbo.spArticleVideo_GetMaxSortNo";
        }
    }

    /// <summary>
    /// 新增網頁影片資料
    /// </summary>
    public class spArticleVideo_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;
        public Guid ArticleId;
        public int SortNo;
        public string VidLinkUrl;
        public string SourceVideoId;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideo_InsertData";
        }
    }

    /// <summary>
    /// 新增網頁影片的多國語系資料
    /// </summary>
    public class spArticleVideoMultiLang_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;
        public string CultureName;
        public string VidSubject;
        public string VidDesc;
        public bool IsShowInLang;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideoMultiLang_InsertData";
        }
    }

    /// <summary>
    /// 更新網頁影片資料
    /// </summary>
    public class spArticleVideo_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;
        public int SortNo;
        public string VidLinkUrl;
        public string SourceVideoId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideo_UpdateData";
        }
    }

    /// <summary>
    /// 更新網頁影片的多國語系資料
    /// </summary>
    public class spArticleVideoMultiLang_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;
        public string CultureName;
        public string VidSubject;
        public string VidDesc;
        public bool IsShowInLang;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideoMultiLang_UpdateData";
        }
    }

    /// <summary>
    /// 取得後台用指定語系的網頁影片清單
    /// </summary>
    public class spArticleVideoMultiLang_GetListForBackend : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid ArticleId;
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
            return "dbo.spArticleVideoMultiLang_GetListForBackend";
        }
    }

    /// <summary>
    /// 刪除網頁影片資料
    /// </summary>
    public class spArticleVideo_DeleteData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public Guid VidId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spArticleVideo_DeleteData";
        }
    }

    #endregion
}
