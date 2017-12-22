using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article : FrontendBasePage
{
    protected FrontendPageCommon c;
    protected IMasterArticleSettings masterSettings;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        frontendPageCommon = c;
        c.InitialLoggerOfUI(this.GetType());
        bool result = c.InitialArticleData();

        masterSettings = (IMasterArticleSettings)this.Master;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
}