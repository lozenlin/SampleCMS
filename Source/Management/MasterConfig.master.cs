using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MasterConfig : System.Web.UI.MasterPage
{
    protected BackendPageCommon c;

    #region Public properties

    public string FlagValue
    {
        get { return txtFlag.Value; }
        set { txtFlag.Value = value; }
    }

    /// <summary>
    /// 啟用小日曆的中文模式
    /// </summary>
    public bool EnableDatepickerTW
    {
        get { return ltrDatepickerJsTW.Visible; }
        set { ltrDatepickerJsTW.Visible = value; }
    }

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new BackendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        Page.MaintainScrollPositionOnPostBack = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Public Methods

    /// <summary>
    /// 顯示錯誤訊息
    /// </summary>
    public void ShowErrorMsg(string value)
    {
        ltrErrMsg.Text = value;
        ErrorMsgArea.Visible = (value != "");

        if (ErrorMsgArea.Visible)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorMsg", "smoothUp();", true);
        }
    }

    #endregion
}
