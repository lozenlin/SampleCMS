using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pick_CustomWebProgram : System.Web.UI.Page
{
    protected ArticleCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;

    private List<ControlInfo> infoList;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(c);

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.SetCustomEmployeeAuthorizationResult(artPub);
        empAuth.InitialAuthorizationResultOfSubPages();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Authenticate
            if (!empAuth.CanEditThisPage())
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            InitialInfoList();
            DisplayList();
        }

        LoadTitle();
    }

    private void LoadTitle()
    {
        Title = Resources.Lang.GroupLabel_CustomWebProgram;
    }

    private void InitialInfoList()
    {
        infoList = new List<ControlInfo>();

        infoList.Add(new ControlInfo()
        {
            Name = "",
            PicFileName = "default.png"
        });

        infoList.Add(new ControlInfo()
        {
            Name = "~/Sitemap.aspx",
            PicFileName = "Sitemap.png"
        });

        infoList.Add(new ControlInfo()
        {
            Name = "~/Search-Result.aspx",
            PicFileName = "Search-Result.png"
        });
    }

    private void DisplayList()
    {
        rptList.DataSource = infoList;
        rptList.DataBind();
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ControlInfo info = (ControlInfo)e.Item.DataItem;

        string displayText = info.Name;

        if (info.Name == "")
        {
            displayText = "(" + Resources.Lang.Pick_btnClear + ")";
        }


        HtmlAnchor btnPic = (HtmlAnchor)e.Item.FindControl("btnPic");
        btnPic.HRef = "BPimages/CustomWebProgram/" + info.PicFileName;
        btnPic.Title = info.Name;

        HtmlImage imgPic = (HtmlImage)e.Item.FindControl("imgPic");
        imgPic.Src = "BPimages/CustomWebProgram/" + info.PicFileName;

        HtmlGenericControl ctlNameArea = (HtmlGenericControl)e.Item.FindControl("ctlNameArea");
        ctlNameArea.InnerHtml = displayText;
        ctlNameArea.Attributes["title"] = displayText;

        LinkButton btnSelect = (LinkButton)e.Item.FindControl("btnSelect");
        btnSelect.CommandArgument = info.Name;
        btnSelect.OnClientClick = string.Format("return confirm('" + Resources.Lang.Pick_ConfirmSelect_Format + "');", displayText);
    }

    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "sel":
                string controlName = e.CommandArgument.ToString();
                string js = Common.Utility.StringUtility.GetWriteValueOfOpenerJs(c.qsCtlText, controlName);
                js += " closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "sel", js, true);
                break;
        }
    }

    private class ControlInfo
    {
        public string Name;
        public string PicFileName;
    }
}