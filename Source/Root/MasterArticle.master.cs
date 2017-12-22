using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterArticle : System.Web.UI.MasterPage, IMasterArticleSettings
{
    #region IMasterArticleSettings

    public bool IsHomePage
    {
        get { return isHomePage; }
        set { isHomePage = value; }
    }
    protected bool isHomePage = false;

    #endregion

    protected FrontendBasePage basePage;
    protected FrontendPageCommon c;
    protected ArticleData articleData;
    protected string bodyTagAttributes = "class=\"inner-page\"";

    protected void Page_Init(object sender, EventArgs e)
    {
        basePage = (FrontendBasePage)this.Page;
        c = basePage.GetFrontendPageCommon();
        articleData = c.GetArticleData();
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
        if (isHomePage)
        {
            bodyTagAttributes = "";
        }
    }
}
