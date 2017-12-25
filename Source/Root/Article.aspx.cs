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
    protected ArticlePublisherLogic artPub;
    protected IMasterArticleSettings masterSettings;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        if (!c.RetrieveArticleIdAndData())
        {
            Response.Redirect(c.ERROR_PAGE);
        }

        articleData = c.GetArticleData();
        artPub = new ArticlePublisherLogic(null);
        masterSettings = (IMasterArticleSettings)this.Master;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HandleShowType();
        }
    }

    private void HandleShowType()
    {
        switch (articleData.ShowTypeId)
        {
            case 1:
                // page
                DisplayArticle();
                break;
            case 2:
                // to sub-page
                RedirectToSubPage();
                break;
            case 3:
                // URL
                if (articleData.LinkUrl == "")
                {
                    string url = c.AppendCurrentQueryString(articleData.LinkUrl);
                    Response.Redirect(url);
                }
                else
                {
                    DisplayArticle();
                }
                break;
            case 4:
                // use control
                if (articleData.ControlName == "")
                {
                    Control ctl = LoadControl("~/UserControls" + articleData.ControlName);
                    ControlArea.Controls.Add(ctl);
                }
                else
                {
                    DisplayArticle();
                }
                break;
            default:
                c.LoggerOfUI.ErrorFormat("invalid showTypeId:{0}", articleData.ShowTypeId);
                Response.Redirect(c.ERROR_PAGE);
                break;
        }
    }

    private void DisplayArticle()
    {
        DisplaySubitems();

        ltrArticleContext.Text = articleData.ArticleContext;
    }

    private void DisplaySubitems()
    {
        SubitemsArea.Visible = articleData.IsListAreaShowInFrontStage;

        if (!SubitemsArea.Visible)
            return;


    }

    private void RedirectToSubPage()
    {

    }
}