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

    public bool ShowBreadcrumbAndSearchArea
    {
        get { return showBreadcrumbAndSearchArea; }
        set { showBreadcrumbAndSearchArea = value; }
    }
    protected bool showBreadcrumbAndSearchArea = true;

    public bool ShowSearchCondition
    {
        get { return showSearchCondition; }
        set { showSearchCondition = value; }
    }
    protected bool showSearchCondition = true;


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
            DisplayUnitItems();
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
        btnBrand.HRef = string.Format("Index.aspx?l={0}", c.qsLangNo);
        btnBrand.Title = Resources.Lang.WebsiteName;

        if (isHomePage)
        {
            bodyTagAttributes = "";
            InnerPageSection.Visible = false;
        }

        ShowPreviewModeAlert();
        LoadLanguageButtonsUIData();
        LoadBannerSubjectUIData();
        HandleLayoutMode();

        BreadcrumbAndSearchArea.Visible = showBreadcrumbAndSearchArea;
        ucSearchCondition.Visible = showSearchCondition;
        ControlsBeforeFooterArea.Visible = showControlsBeforeFooterArea;
        btnReturnToList.Visible = showReturnToListButton;
    }

    private void ShowPreviewModeAlert()
    {
        if (articleData.IsPreviewMode)
        {
            IsPreviewModeAlert.Visible = true;
        }
    }

    private void LoadLanguageButtonsUIData()
    {
        btnZhTw.HRef = StringUtility.SetParaValueInUrl(Request.RawUrl, "l", LangManager.LangNoZHTW);
        btnEn.HRef = StringUtility.SetParaValueInUrl(Request.RawUrl, "l", LangManager.LangNoEN);
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
                DisplaySideLinks();
                break;
            default:
                c.LoggerOfUI.ErrorFormat("invalid layoutModeId:{0}", articleData.LayoutModeId);
                Response.Redirect(c.ERROR_PAGE);
                break;
        }
    }

    private void DisplayUnitItems()
    {
        // Home
        btnHomeInUnit.HRef = "Index.aspx?l=" + c.qsLangNo.ToString();
        btnHomeInUnit.InnerHtml = Resources.Lang.btnHome;
        btnHomeInUnit.Title = Resources.Lang.btnHome_Hint;

        if (articleData.ArticleId.Value == Guid.Empty)
        {
            HomeInUnitArea.Attributes["class"] = "active";
        }

        Guid rootId = Guid.Empty;
        DataSet dsUnitItems = artPub.GetArticleValidListForUnitArea(rootId, c.qsCultureNameOfLangNo, true);

        if (dsUnitItems != null)
        {
            rptUnitItems.DataSource = dsUnitItems.Tables[0];
            rptUnitItems.DataBind();
        }
    }

    protected void rptUnitItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        HtmlGenericControl ItemArea = (HtmlGenericControl)e.Item.FindControl("ItemArea");

        if (articleData.Lv1Id == articleId
            || articleData.Lv2Id == articleId
            || articleData.Lv3Id == articleId)
        {
            ItemArea.Attributes["class"] = "active";
        }

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        btnItem.InnerHtml = articleSubject;
        btnItem.Title = articleSubject;
        string destUrl = StringUtility.GetLinkUrlOfShowType(articleId, c.qsLangNo, showTypeId, linkUrl);
        btnItem.HRef = destUrl;
        Repeater rptSubitems = e.Item.FindControl("rptSubitems") as Repeater;

        if (!isHideChild && rptSubitems != null)
        {
            DataSet dsSubitems = artPub.GetArticleValidListForUnitArea(articleId, c.qsCultureNameOfLangNo, false);

            if (dsSubitems != null && dsSubitems.Tables[0].Rows.Count > 0)
            {
                btnItem.Attributes["class"] = "fh5co-sub-ddown";    // add down arrow

                rptSubitems.DataSource = dsSubitems.Tables[0];
                rptSubitems.DataBind();
            }
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
            //SubtitleArea.Visible = true;
            ltrSubtitle.Text = articleData.Subtitle;
        }

        SpacerAfterSubtitle.Visible = !articleData.IsListAreaShowInFrontStage;
    }

    private void DisplayAttachments()
    {
        if (!articleData.IsAttAreaShowInFrontStage)
            return;

        DataSet dsAttachments = artPub.GetAttachFileListForFrontend(articleData.ArticleId.Value, c.qsCultureNameOfLangNo);

        if (dsAttachments != null && dsAttachments.Tables[0].Rows.Count > 0)
        {
            try
            {
                Att.AttCombination attComb = new Att.AttCombination(dsAttachments.Tables[0]);
                rptAttachments.DataSource = attComb.GetList();
                rptAttachments.DataBind();

                AttachmentsArea.Visible = true;
            }
            catch (Exception ex)
            {
                c.LoggerOfUI.Error(string.Format("generate attachments failed in article (id:[{0}])", articleData.ArticleId), ex);
            }
        }
    }

    protected void rptAttachments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Att.AttInfo attInfo = (Att.AttInfo)e.Item.DataItem;

        Literal ltrAttSubject = (Literal)e.Item.FindControl("ltrAttSubject");
        ltrAttSubject.Text = attInfo.AttSubject;

        Repeater rptAttSubitems = (Repeater)e.Item.FindControl("rptAttSubitems");
        rptAttSubitems.DataSource = attInfo.Files;
        rptAttSubitems.DataBind();
    }

    protected void rptAttSubitems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Att.FileData fileData = (Att.FileData)e.Item.DataItem;

        HtmlAnchor btnDownload = (HtmlAnchor)e.Item.FindControl("btnDownload");
        btnDownload.HRef = string.Format("~/FileAtt.ashx?attid={0}&l={1}", fileData.AttId, c.qsLangNo);
        btnDownload.Title = fileData.FileName;

        HtmlImage imgExt = (HtmlImage)e.Item.FindControl("imgExt");
        imgExt.Src = "images/FileExtIcon/" + ResUtility.GetExtIconFileName(fileData.FileName);
        imgExt.Alt = ResUtility.GetExtIconText(fileData.FileName);

        Literal ltrDownload = (Literal)e.Item.FindControl("ltrDownload");
        ltrDownload.Text = string.Format("{0} ({1})", Resources.Lang.Att_lblDownload, fileData.FileExt.ToLower().Replace(".", ""));

        Literal ltrFileSize = (Literal)e.Item.FindControl("ltrFileSize");
        ltrFileSize.Text = fileData.FileSizeDesc;

        Literal ltrReadCount = (Literal)e.Item.FindControl("ltrReadCount");
        ltrReadCount.Text = fileData.ReadCount.ToString();

        Literal ltrMdfDate = (Literal)e.Item.FindControl("ltrMdfDate");
        ltrMdfDate.Text = fileData.MdfDate.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private void DisplayPictures()
    {
        if (!articleData.IsPicAreaShowInFrontStage)
            return;

        DataSet dsPictures = artPub.GetArticlePictureListForFrontend(articleData.ArticleId.Value, c.qsCultureNameOfLangNo);

        if (dsPictures != null && dsPictures.Tables[0].Rows.Count > 0)
        {
            rptPictures.DataSource = dsPictures.Tables[0];
            rptPictures.DataBind();

            PicturesArea.Visible = true;
        }
    }

    protected void rptPictures_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid picId = (Guid)drvTemp["PicId"];
        string picSubject = drvTemp.ToSafeStr("PicSubject");

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        btnItem.Title = picSubject;
        btnItem.HRef = string.Format("FileArtPic.ashx?attid={0}&l={1}", picId, c.qsLangNo);

        HtmlImage imgPic = (HtmlImage)e.Item.FindControl("imgPic");
        imgPic.Src = string.Format("FileArtPic.ashx?attid={0}&w=1024&h=768&l={1}", picId, c.qsLangNo);
        imgPic.Alt = picSubject;
    }

    private void DisplayVideos()
    {
        if (!articleData.IsVideoAreaShowInFrontStage)
            return;

        DataSet dsVideos = artPub.GetArticleVideoListForFrontend(articleData.ArticleId.Value, c.qsCultureNameOfLangNo);

        if (dsVideos != null && dsVideos.Tables[0].Rows.Count > 0)
        {
            rptVideos.DataSource = dsVideos.Tables[0];
            rptVideos.DataBind();

            VideosArea.Visible = true;
        }
    }

    protected void rptVideos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid vidId = (Guid)drvTemp["VidId"];
        string vidSubject = drvTemp.ToSafeStr("VidSubject");
        string vidDesc = drvTemp.ToSafeStr("VidDesc");
        string sourceVideoId = drvTemp.ToSafeStr("SourceVideoId");

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        btnItem.HRef = string.Format("https://www.youtube.com/embed/{0}?autoplay=1", sourceVideoId);
        btnItem.Title = vidDesc;
        btnItem.Attributes["data-fancybox-title"] = vidSubject;

        HtmlImage imgPic = (HtmlImage)e.Item.FindControl("imgPic");
        imgPic.Src = string.Format("http://i.ytimg.com/vi/{0}/hqdefault.jpg", sourceVideoId);
        imgPic.Alt = vidSubject;

        Literal ltrVidSubject = (Literal)e.Item.FindControl("ltrVidSubject");
        ltrVidSubject.Text = vidSubject;
    }

    private void IncreaseReadCount()
    {
        if (!articleData.ArticleId.HasValue)
            return;

        bool result = artPub.IncreaseArticleMultiLangReadCount(articleData.ArticleId.Value, c.qsCultureNameOfLangNo);
    }

    private void DisplaySideLinks()
    {
        if (!articleData.ParentId.HasValue)
            return;

        Guid parentId = articleData.ParentId.Value;

        if (articleData.ArticleLevelNo > 1)
        {
            // change to parentId of parent
            DataSet dsParent = artPub.GetArticleDataForFrontend(parentId, c.qsCultureNameOfLangNo);

            if (dsParent != null && dsParent.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsParent.Tables[0].Rows[0];

                parentId = (Guid)drFirst["ParentId"];
            }
        }

        DataSet dsSideLinks = artPub.GetArticleValidListForSideSection(parentId, c.qsCultureNameOfLangNo);

        if (dsSideLinks != null)
        {
            rptSideLinks.DataSource = dsSideLinks.Tables[0];
            rptSideLinks.DataBind();
        }
    }

    protected void rptSideLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        HtmlGenericControl ItemArea = (HtmlGenericControl)e.Item.FindControl("ItemArea");

        if (articleData.ArticleId.Value == articleId)
        {
            ItemArea.Attributes["class"] = "active";
        }

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");
        btnItem.InnerHtml = articleSubject;
        btnItem.Title = articleSubject;
        string destUrl = StringUtility.GetLinkUrlOfShowType(articleId, c.qsLangNo, showTypeId, linkUrl);
        btnItem.HRef = destUrl;
        Repeater rptSubitems = e.Item.FindControl("rptSubitems") as Repeater;

        if (rptSubitems != null)    /*  && !isHideChild <-- 視專案需要加回 */
        {
            DataSet dsSubitems = artPub.GetArticleValidListForSideSection(articleId, c.qsCultureNameOfLangNo);

            if (dsSubitems != null && dsSubitems.Tables[0].Rows.Count > 0)
            {
                rptSubitems.DataSource = dsSubitems.Tables[0];
                rptSubitems.DataBind();
            }
        }
    }
}
