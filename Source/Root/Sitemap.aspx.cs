using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Sitemap : FrontendBasePage
{
    protected OtherArticlePageCommon c;
    protected ArticlePublisherLogic artPub;
    protected IMasterArticleSettings masterSettings;

    private int[] itemNum = new int[3];

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new OtherArticlePageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        if (!c.RetrieveArticleIdAndData())
        {
            Response.Redirect(c.ERROR_PAGE);
        }

        articleData = c.GetArticleData();
        artPub = new ArticlePublisherLogic(null);
        masterSettings = (IMasterArticleSettings)this.Master;
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadSystemVersion();
            DisplaySitemapLinks();
        }
    }

    private void DisplaySitemapLinks()
    {
        Guid rootId = Guid.Empty;
        DataSet dsLinks = artPub.GetArticleValidListForSitemap(rootId, c.qsCultureNameOfLangNo);

        if (dsLinks != null)
        {
            rptSitemapLinks.DataSource = dsLinks.Tables[0];
            rptSitemapLinks.DataBind();
        }
    }

    protected void rptSitemapLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid articleId = (Guid)drvTemp["ArticleId"];
        string articleSubject = drvTemp.ToSafeStr("ArticleSubject");
        int showTypeId = Convert.ToInt32(drvTemp["ShowTypeId"]);
        string linkUrl = drvTemp.ToSafeStr("LinkUrl");
        string linkTarget = drvTemp.ToSafeStr("LinkTarget");
        bool isHideChild = Convert.ToBoolean(drvTemp["IsHideChild"]);
        int articleLevelNo = Convert.ToInt32(drvTemp["ArticleLevelNo"]);

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        string destUrl = StringUtility.GetLinkUrlOfShowType(articleId, c.qsLangNo, showTypeId, linkUrl);
        btnItem.HRef = destUrl;
        string subject = "";

        switch (articleLevelNo)
        {
            case 1:
                itemNum[0] = e.Item.ItemIndex + 1;
                subject = string.Format("{0}. {1}", e.Item.ItemIndex + 1, articleSubject);
                break;
            case 2:
                itemNum[1] = e.Item.ItemIndex + 1;
                subject = string.Format("{0}-{1}. {2}", itemNum[0], e.Item.ItemIndex + 1, articleSubject);
                break;
            case 3:
                itemNum[2] = e.Item.ItemIndex + 1;
                subject = string.Format("{0}-{1}-{2}. {3}", itemNum[0], itemNum[1], e.Item.ItemIndex + 1, articleSubject);
                break;
        }

        btnItem.InnerHtml = subject;
        btnItem.Title = subject;

        Repeater rptSubitems = e.Item.FindControl("rptSubitems") as Repeater;

        if (!isHideChild && rptSubitems != null)
        {
            DataSet dsSubitems = artPub.GetArticleValidListForSitemap(articleId, c.qsCultureNameOfLangNo);

            if (dsSubitems != null && dsSubitems.Tables[0].Rows.Count > 0)
            {
                rptSubitems.DataSource = dsSubitems.Tables[0];
                rptSubitems.DataBind();
            }
        }

    }

    private void LoadSystemVersion()
    {
        try
        {
            System.Reflection.Assembly asmAppCode = System.Reflection.Assembly.Load("App_Code");
            ltrSystemVersion.Text = asmAppCode.GetName().Version.ToString();

            System.Reflection.Assembly asmLogicObject = System.Reflection.Assembly.Load("Common.LogicObject");
            ltrLogicObjectVersion.Text = asmLogicObject.GetName().Version.ToString();

            System.Reflection.Assembly asmDataAccess = System.Reflection.Assembly.Load("Common.DataAccess");
            ltrDataAccessVersion.Text = asmDataAccess.GetName().Version.ToString();

            System.Reflection.Assembly asmUtility = System.Reflection.Assembly.Load("Common.Utility");
            ltrUtilityVersion.Text = asmUtility.GetName().Version.ToString();
        }
        catch { }
    }
}