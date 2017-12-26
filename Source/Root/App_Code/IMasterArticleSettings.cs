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
    /// 設定回上層鈕的連結內容
    /// </summary>
    void SetReturnToListUrl(string returnToListUrl);
    /// <summary>
    /// 自訂橫幅標題區 html
    /// </summary>
    string CustomBannerSubjectHtml { get; set; }
}