using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_wucSearchConditionPost : System.Web.UI.UserControl
{
    protected SearchPageCommon c;

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new SearchPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnToSearchResult_Click(object sender, EventArgs e)
    {
        string keyword = txtKeyword.Text.Trim();

        if (keyword == "")
            return;

        c.GoToSearchResult(keyword);
    }
}