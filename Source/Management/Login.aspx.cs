using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected LoginCommonOfBackend c;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new LoginCommonOfBackend(this.Context, this.ViewState);
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {

    }
}