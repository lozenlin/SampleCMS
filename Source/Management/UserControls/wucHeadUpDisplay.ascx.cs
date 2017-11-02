using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class UserControls_wucHeadUpDisplay : System.Web.UI.UserControl, IHeadUpDisplay
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

    #endregion

}