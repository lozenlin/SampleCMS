using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class UserControls_wucHeadUpDisplay : System.Web.UI.UserControl, IHeadUpDisplay
{
    protected BackendPageCommon c;
    protected EmployeeAuthorityLogic empAuth;

    private bool useEnglishSubject = false;

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new BackendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic(c);

        if (c.seCultureNameOfBackend == "en")
        {
            useEnglishSubject = true;
        }

        if (!IsPostBack)
        {
            LoadUIData();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }

        if (c.seLoginEmpData.EmpAccount != null
            && Request.AppRelativeCurrentExecutionFilePath != "~/Back-End-Log.aspx")
        {
            //新增後端操作記錄
            string description = string.Format("．{0}　．頁碼/Page[{1}]　．路徑/Route[{2}]　．IsPostBack[{3}]",
                ltrHead.Text, c.qsPageCode, Common.Utility.StringUtility.RemoveHtmlTag(ltrBreadcrumb.Text.Replace("</", "/</")), IsPostBack);

            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = c.GetEmpAccount(),
                Description = description,
                IP = c.GetClientIP()
            });
        }
    }

    private void LoadUIData()
    {
        btnBackToParent.HRef = "~/Dashboard.aspx";
        btnBackToParent.Title = Resources.Lang.Main_btnBackToParent_Hint;
        ltrEdit.Text = Resources.Lang.Main_btnEdit;
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;
        ltrAddNew.Text = Resources.Lang.Main_btnAddNew;
        btnAddNew.Title = Resources.Lang.Main_btnAddNew_Hint;
        btnView.Title = Resources.Lang.Main_btnView_Hint;
        btnViewZhTw.Title = Resources.Lang.Main_btnViewZhTw_Hint;
        btnViewEn.Title = Resources.Lang.Main_btnViewEn_Hint;
        btnPreview.Title = Resources.Lang.Main_btnPreview_Hint;
        btnPreviewZhTw.Title = Resources.Lang.Main_btnPreviewZhTw_Hint;
        btnPreviewEn.Title = Resources.Lang.Main_btnPreviewEn_Hint;
    }

    private HtmlAnchor GetButton(HudButtonNameEnum buttonName)
    {
        HtmlAnchor btn = null;
        
        switch (buttonName)
        {
            case HudButtonNameEnum.BackToParent:
                btn = btnBackToParent;
                break;
            case HudButtonNameEnum.Edit:
                btn = btnEdit;
                break;
            case HudButtonNameEnum.AddNew:
                btn = btnAddNew;
                break;
            case HudButtonNameEnum.CustomPrimary1:
                btn = btnCustomPrimary1;
                break;
            case HudButtonNameEnum.CustomPrimary2:
                btn = btnCustomPrimary2;
                break;
            case HudButtonNameEnum.View:
                btn = btnView;
                break;
            case HudButtonNameEnum.ViewZhTw:
                btn = btnViewZhTw;
                break;
            case HudButtonNameEnum.ViewEn:
                btn = btnViewEn;
                break;
            case HudButtonNameEnum.Preview:
                btn = btnPreview;
                break;
            case HudButtonNameEnum.PreviewZhTw:
                btn = btnPreviewZhTw;
                break;
            case HudButtonNameEnum.PreviewEn:
                btn = btnPreviewEn;
                break;
            default:
                throw new Exception("HudButton does not exist");
        }

        return btn;
    }

    #region IHeadUpDisplay

    public string GetHeadText()
    {
        return ltrHead.Text;
    }

    public void SetHeadText(string value)
    {
        ltrHead.Text = value;
    }

    public string GetHeadIconImageUrl()
    {
        return imgHead.Src;
    }

    public void SetHeadIconImageUrl(string url)
    {
        imgHead.Src = url;
    }

    public string GetButtonAttribute(HudButtonNameEnum buttonName, HudButtonAttributeEnum buttonAttr)
    {
        string result = "";
        HtmlAnchor btn = GetButton(buttonName);

        switch (buttonAttr)
        {
            case HudButtonAttributeEnum.NavigateUrl:
                result = btn.HRef;
                break;
            case HudButtonAttributeEnum.JsInNavigateUrl:
                result = btn.HRef;
                if (result.StartsWith("javascript:"))
                    result = result.Substring("javascript:".Length);
                break;
            case HudButtonAttributeEnum.Text:
                if (buttonName == HudButtonNameEnum.Edit)
                {
                    result = ltrEdit.Text;
                }
                else if (buttonName == HudButtonNameEnum.AddNew)
                {
                    result = ltrAddNew.Text;
                }
                else
                {
                    result = btn.InnerHtml;
                }
                break;
            case HudButtonAttributeEnum.ToolTip:
                result = btn.Title;
                break;
            case HudButtonAttributeEnum.InnerHtml:
                result = btn.InnerHtml;
                break;
            default:
                throw new Exception("HudButton attribute does not exist");
        }

        return result;
    }

    public void SetButtonAttribute(HudButtonNameEnum buttonName, HudButtonAttributeEnum buttonAttr, string value)
    {
        HtmlAnchor btn = GetButton(buttonName);

        switch (buttonAttr)
        {
            case HudButtonAttributeEnum.NavigateUrl:
                btn.HRef = value;
                break;
            case HudButtonAttributeEnum.JsInNavigateUrl:
                btn.HRef = "javascript:" + value;
                break;
            case HudButtonAttributeEnum.Text:
                if (buttonName == HudButtonNameEnum.Edit)
                {
                    ltrEdit.Text = value;
                }
                else if (buttonName == HudButtonNameEnum.AddNew)
                {
                    ltrAddNew.Text = value;
                }
                else
                {
                    btn.InnerHtml = value;
                }
                break;
            case HudButtonAttributeEnum.ToolTip:
                btn.Title = value;
                break;
            case HudButtonAttributeEnum.InnerHtml:
                btn.InnerHtml = value;
                break;
            default:
                throw new Exception("HudButton attribute does not exist");
        }
    }

    public bool GetButtonVisible(HudButtonNameEnum buttonName)
    {
        HtmlAnchor btn = GetButton(buttonName);

        return btn.Visible;
    }

    public void SetButtonVisible(HudButtonNameEnum buttonName, bool visible)
    {
        HtmlAnchor btn = GetButton(buttonName);
        btn.Visible = visible;
    }

    public string GetBreadcrumbTextItemHtml(string subject)
    {
        return string.Format("<span class='breadcrumb-item active'>{0}</span>", subject);
    }

    public string GetBreadcrumbLinkItemHtml(string subject, string title, string href)
    {
        return string.Format("<a href='{0}' class='breadcrumb-item' title='{1}'>{2}</a>", href, title, subject);
    }

    /// <summary>
    /// e.g., Home / textAfterHomeNode
    /// </summary>
    public void RebuildBreadcrumb(string textAfterHomeNode, bool textIsHtml)
    {
        ltrBreadcrumb.Text = "";

        //add home node
        ltrBreadcrumb.Text += GetBreadcrumbLinkItemHtml(Resources.Lang.Main_Home, Resources.Lang.Main_Home, "/Management/Dashboard.aspx");

        //add text
        if (textIsHtml)
        {
            ltrBreadcrumb.Text += textAfterHomeNode;
        }
        else
        {
            ltrBreadcrumb.Text += GetBreadcrumbTextItemHtml(textAfterHomeNode);
        }
    }

    /// <summary>
    /// e.g., Home / OpSubject
    /// </summary>
    public bool RebuildBreadcrumbAndUpdateHead(int opId)
    {
        OperationHtmlAnchorData anchorData = empAuth.GetOperationHtmlAnchorData(opId, useEnglishSubject);

        if (anchorData == null)
            return false;

        if (anchorData.IconImageFileUrl != "")
        {
            imgHead.Src = "~/BPimages/icon/" + anchorData.IconImageFileUrl;
        }

        if (ltrHead.Text == "default_head")
        {
            ltrHead.Text = anchorData.Subject;
        }

        RebuildBreadcrumb(ltrHead.Text, false);

        return true;
    }

    #endregion
}