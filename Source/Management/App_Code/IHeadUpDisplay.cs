using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 抬頭顯示區的開放功能
/// </summary>
public interface IHeadUpDisplay
{
    string GetHeadText();
    void SetHeadText(string value);
    string GetHeadIconImageUrl();
    void SetHeadIconImageUrl(string url);
    string GetButtonAttribute(HudButtonNameEnum buttonName, HudButtonAttributeEnum buttonAttr);
    void SetButtonAttribute(HudButtonNameEnum buttonName, HudButtonAttributeEnum buttonAttr, string value);
    bool GetButtonVisible(HudButtonNameEnum buttonName);
    void SetButtonVisible(HudButtonNameEnum buttonName, bool visible);
    string GetBreadcrumbTextItemHtml(string subject);
    string GetBreadcrumbLinkItemHtml(string subject, string title, string href);
    /// <summary>
    /// e.g., Home / textAfterHomeNode
    /// </summary>
    void RebuildBreadcrumb(string textAfterHomeNode, bool textIsHtml);
    /// <summary>
    /// e.g., Home / OpSubject
    /// </summary>
    bool RebuildBreadcrumbAndUpdateHead(int opId);
}

public enum HudButtonNameEnum
{
    BackToParent,
    Edit,
    AddNew,
    CustomPrimary1,
    CustomPrimary2,
    View,
    ViewZhTw,
    ViewEn,
    Preview,
    PreviewZhTw,
    PreviewEn
}

public enum HudButtonAttributeEnum
{
    NavigateUrl,
    JsInNavigateUrl,
    Text,
    ToolTip,
    InnerHtml
}
