using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_List : System.Web.UI.Page
{
    protected BackendPageCommon c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new BackendPageCommon(this.Context, this.ViewState);
        empAuth = new EmployeeAuthorityLogic(c);
        hud = Master.GetHeadUpDisplay();
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
        //HUD
        if (empAuth.CanAddSubItemInThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
        }
    }
}