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

public partial class LayoutControls_ListItemsThumb : System.Web.UI.UserControl
{
    protected FrontendPageCommon c;
    protected ArticlePublisherLogic artPub;
    protected FrontendBasePage basePage;
    protected ArticleData articleData;
    protected IMasterArticleSettings masterSettings;

    private bool isLazyLoadingMode = true;   //滾動加載模式

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(null);
        basePage = (FrontendBasePage)this.Page;
        articleData = basePage.GetArticleData();
        masterSettings = (IMasterArticleSettings)this.Page.Master;

        ucDataPager.MaxItemCountOfPage = 10;
        ucDataPager.MaxDisplayCountInPageCodeArea = 5;
        ucDataPager.LinkUrlToReload = string.Concat(Request.AppRelativeCurrentExecutionFilePath, "?", Request.QueryString);
        ucDataPager.Initialize(0, 0);

        if (articleData.IsPreviewMode)
        {
            isLazyLoadingMode = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (isLazyLoadingMode)
            {
                LazyLoadingArea.Visible = true;
                LazyLoadingCtrlArea.Visible = true;
                ucDataPager.Visible = false;
            }
            else
            {
                DisplaySubitems();
            }
        }
    }

    private void DisplaySubitems()
    {
        ArticleValidListQueryParams param = new ArticleValidListQueryParams()
        {
            ParentId = articleData.ArticleId.Value,
            CultureName = c.qsCultureNameOfLangNo,
            Kw = ""
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 0,
            EndNum = 0,
            SortField = articleData.SortFieldOfFrontStage,
            IsSortDesc = articleData.IsSortDescOfFrontStage
        };

        // get total of items
        artPub.GetArticleValidListForFrontend(param);

        // update pager and get begin end of item numbers
        int itemTotalCount = param.PagedParams.RowCount;
        ucDataPager.Initialize(itemTotalCount, c.qsPageCode);

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = ucDataPager.BeginItemNumberOfPage,
            EndNum = ucDataPager.EndItemNumberOfPage,
            SortField = articleData.SortFieldOfFrontStage,
            IsSortDesc = articleData.IsSortDescOfFrontStage
        };

        DataSet dsSubitems = artPub.GetArticleValidListForFrontend(param);

        if (dsSubitems != null)
        {
            rptSubitems.DataSource = dsSubitems.Tables[0];
            rptSubitems.DataBind();
        }
    }

    protected void rptSubitems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid articleId = (Guid)drvTemp["ArticleId"];
        string articleSubject = drvTemp.ToSafeStr("ArticleSubject");
        int showTypeId = Convert.ToInt32(drvTemp["ShowTypeId"]);
        string linkUrl = drvTemp.ToSafeStr("LinkUrl");
        string linkTarget = drvTemp.ToSafeStr("LinkTarget");
        string destUrl = "/" + StringUtility.GetLinkUrlOfShowType(articleId, c.qsLangNo, showTypeId, linkUrl);

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        btnItem.HRef = destUrl;
        btnItem.Title = articleSubject;

        HtmlAnchor btnPic = (HtmlAnchor)e.Item.FindControl("btnPic");
        btnPic.HRef = destUrl;
        btnPic.Title = articleSubject;

        if (linkTarget != "")
        {
            btnItem.Target = linkTarget;
            btnPic.Target = linkTarget;
        }

        // get thumb picture
        DataSet dsArtPic = artPub.GetArticlePictureListForFrontend(articleId, c.qsCultureNameOfLangNo);

        if (dsArtPic != null && dsArtPic.Tables[0].Rows.Count > 0)
        {
            DataRow drFirst = dsArtPic.Tables[0].Rows[0];

            Guid picId = (Guid)drFirst["PicId"];
            string picSubject = drFirst.ToSafeStr("PicSubject");

            HtmlImage imgPic = (HtmlImage)e.Item.FindControl("imgPic");
            imgPic.Src = string.Format("/FileArtPic.ashx?attid={0}&w=640&h=480&l={1}", picId, c.qsLangNo);
            imgPic.Alt = picSubject;
        }
    }
}