using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public interface IMasterArticleSettings
{
    /// <summary>
    /// 為首頁
    /// </summary>
    bool IsHomePage { get; set; }
    /// <summary>
    /// 顯示頁尾上方的控制按鈕區塊
    /// </summary>
    bool ShowControlsBeforeFooterArea { get; set; }
    /// <summary>
    /// 顯示回上層鈕
    /// </summary>
    bool ShowReturnToListButton { get; set; }
    /// <summary>
    /// 自訂橫幅標題區 html
    /// </summary>
    string CustomBannerSubjectHtml { get; set; }
    /// <summary>
    /// 是否顯示麵包屑的目前節點
    /// </summary>
    bool ShowCurrentNodeOfBreadcrumb { get; set; }
    /// <summary>
    /// 自訂麵包屑的目前節點文字
    /// </summary>
    string CustomCurrentNodeTextOfBreadcrumb { get; set; }
    /// <summary>
    /// 自訂麵包屑的 html 內容(系統自動增加 home)
    /// </summary>
    string CustomRouteHtmlOfBreadcrumb { get; set; }
    /// <summary>
    /// 是否顯示麵包屑與搜尋條件區塊
    /// </summary>
    bool ShowBreadcrumbAndSearchArea { get; set; }
    /// <summary>
    /// 是否顯示搜尋條件
    /// </summary>
    bool ShowSearchCondition { get; set; }

    /// <summary>
    /// 設定回上層鈕的連結內容
    /// </summary>
    void SetReturnToListUrl(string returnToListUrl);
    /// <summary>
    /// 取得麵包屑的文字項目 html
    /// </summary>
    string GetBreadcrumbTextItemHtmlOfBreadcrumb(string subject);
    /// <summary>
    /// 取得麵包屑的連結項目 html
    /// </summary>
    string GetBreadcrumbLinkItemHtmlOfBreadcrumb(string subject, string title, string href);
}