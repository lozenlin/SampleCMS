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

    public bool ShowControlsBeforeFooterArea
    {
        get { return showControlsBeforeFooterArea; }
        set { showControlsBeforeFooterArea = value; }
    }
    protected bool showControlsBeforeFooterArea = true;

    public bool ShowReturnToListButton
    {
        get { return showReturnToListButton; }
        set { showReturnToListButton = value; }
    }
    protected bool showReturnToListButton = false;

    public string CustomBannerSubjectHtml
    {
        get { return customBannerSubjectHtml; }
        set { customBannerSubjectHtml = value; }
    }
    protected string customBannerSubjectHtml = "";

    public bool ShowCurrentNodeOfBreadcrumb
    {
        get { return ucBreadcrumb.ShowCurrentNode; }
        set { ucBreadcrumb.ShowCurrentNode = value; }
    }

    public string CustomCurrentNodeTextOfBreadcrumb
    {
        get { return ucBreadcrumb.CustomCurrentNodeText; }
        set { ucBreadcrumb.CustomCurrentNodeText = value; }
    }

    public string CustomRouteHtmlOfBreadcrumb
    {
        get { return ucBreadcrumb.CustomRouteHtml; }
        set { ucBreadcrumb.CustomRouteHtml = value; }
    }


    public void SetReturnToListUrl(string returnToListUrl)
    {
        btnReturnToList.HRef = returnToListUrl;
    }

    public string GetBreadcrumbTextItemHtmlOfBreadcrumb(string subject)
    {
        return ucBreadcrumb.GetBreadcrumbTextItemHtml(subject);
    }

    public string GetBreadcrumbLinkItemHtmlOfBreadcrumb(string subject, string title, string href)
    {
        return ucBreadcrumb.GetBreadcrumbLinkItemHtml(subject, title, href);
    }

    #endregion

    protected FrontendPageCommon c;
    protected ArticlePublisherLogic artPub;
    protected FrontendBasePage basePage;
    protected ArticleData articleData;
    protected string bodyTagAttributes = "class=\"inner-page\"";
    protected string bannerImageUrl = "images/hero2.jpg";

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(null);
        basePage = (FrontendBasePage)this.Page;
        articleData = basePage.GetArticleData();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
            DisplayArticle();
            DisplayAttachments();
            DisplayPictures();
            DisplayVideos();
            IncreaseReadCount();
        }

        if (articleData.ArticleSubject == "")
        {
            Page.Title = Resources.Lang.WebsiteName;
        }
        else
        {
            Page.Title = string.Format("{0} - {1}", articleData.ArticleSubject, Resources.Lang.WebsiteName);
        }
    }

    private void LoadUIData()
    {
        if (isHomePage)
        {
            bodyTagAttributes = "";
            InnerPageSection.Visible = false;
        }

        LoadBannerSubjectUIData();
        HandleLayoutMode();
        ControlsBeforeFooterArea.Visible = showControlsBeforeFooterArea;
        btnReturnToList.Visible = showReturnToListButton;
    }

    private void LoadBannerSubjectUIData()
    {
        if (articleData.BannerPicFileName != "")
        {
            bannerImageUrl = string.Format("images/{0}/{1}", c.qsLangNo, articleData.BannerPicFileName);
        }

        if (customBannerSubjectHtml != "")
        {
            BannerSubjectArea.Visible = false;
            ltrCustomBannerSubject.Text = customBannerSubjectHtml;
        }
    }

    private void HandleLayoutMode()
    {
        switch (articleData.LayoutModeId)
        {
            case 1:
                // Wide content
                break;
            case 2:
                // 2-col content
                Layout2ColContainerTagHead.Visible = true;
                Layout2ColContainerTagTail.Visible = true;
                Layout2ColMainTagHead.Visible = true;
                Layout2ColMainTagTail.Visible = true;
                Layout2ColSideSection.Visible = true;
                break;
            default:
                c.LoggerOfUI.ErrorFormat("invalid layoutModeId:{0}", articleData.LayoutModeId);
                Response.Redirect(c.ERROR_PAGE);
                break;
        }
    }

    private void DisplayArticle()
    {
        if (isHomePage)
            return;

        if (articleData.SubjectAtBannerArea)
        {
            ltrArticleSubjectInBanner.Text = articleData.ArticleSubject;
        }
        else
        {
            ltrArticleSubject.Text = articleData.ArticleSubject;
        }

        if (articleData.Subtitle != "")
        {
            SubtitleArea.Visible = true;
            ltrSubtitle.Text = articleData.Subtitle;
        }

        SpacerAfterSubtitle.Visible = !articleData.IsListAreaShowInFrontStage;
    }

    private void DisplayAttachments()
    {
        AttachmentsArea.Visible = articleData.IsAttAreaShowInFrontStage;

        if (!AttachmentsArea.Visible)
            return;
    }

    private void DisplayPictures()
    {
        PicturesArea.Visible = articleData.IsPicAreaShowInFrontStage;

        if (!PicturesArea.Visible)
            return;
    }

    private void DisplayVideos()
    {
        VideosArea.Visible = articleData.IsVideoAreaShowInFrontStage;

        if (!VideosArea.Visible)
            return;
    }

    private void IncreaseReadCount()
    {
        if (!articleData.ArticleId.HasValue)
            return;

        bool result = artPub.IncreaseArticleMultiLangReadCount(articleData.ArticleId.Value, c.qsCultureNameOfLangNo);
    }
}
