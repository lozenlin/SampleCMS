using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : FrontendBasePage
{
    protected OtherArticlePageCommon c;
    protected ArticlePublisherLogic artPub;
    protected IMasterArticleSettings masterSettings;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new OtherArticlePageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        if (!c.RetrieveArticleIdAndData(Guid.Empty))
        {
            Response.Redirect(c.ERROR_PAGE);
        }

        articleData = c.GetArticleData();
        artPub = new ArticlePublisherLogic(null);
        masterSettings = (IMasterArticleSettings)this.Master;
        masterSettings.IsHomePage = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
}