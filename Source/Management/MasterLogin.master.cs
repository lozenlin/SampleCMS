using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterLogin : System.Web.UI.MasterPage
{
    protected BackendPageCommon c;

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new BackendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        ltrBackStageName.Text = Resources.Lang.BackStageName;
        Page.Title = ltrBackStageName.Text;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
        }
    }

    private void LoadUIData()
    {
        ltrClientIP.Text = c.GetClientIP();
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
