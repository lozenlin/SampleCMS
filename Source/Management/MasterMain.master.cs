using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterMain : System.Web.UI.MasterPage
{
    protected BackendPageCommon c;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new LoginCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic(c);

        Page.Title = Resources.Lang.BackStageName;
        Page.MaintainScrollPositionOnPostBack = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (c.seLoginEmpData.EmpAccount == null)
        {
            ShowErrorMsg("Session 遺失 (lost session state)");
            c.LogOutWhenSessionMissed(this.Page, "Session 遺失 (lost session state)");
        }
        
        if (!IsPostBack)
        {
        }
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
