using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article_Node : System.Web.UI.Page
{
    protected ArticleCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        c.SelectMenuItemToThisPage();

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfTopPage();

        hud = Master.GetHeadUpDisplay();
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ucDataPager.MaxItemCountOfPage = 20;
        ucDataPager.MaxDisplayCountInPageCodeArea = 5;
        ucDataPager.LinkUrlToReload = string.Concat(Request.AppRelativeCurrentExecutionFilePath, "?", Request.QueryString);
        ucDataPager.Initialize(0, 0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RebuildBreadcrumbAndHeadOfHUD();
        Title = hud.GetHeadText() + " - " + Title;

        if (!IsPostBack)
        {
            if (!empAuth.CanOpenThisPage())
            {
                Response.Redirect(c.BACK_END_HOME);
            }

            LoadUIData();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {

                }

                Master.FlagValue = "";
            }
        }
    }

    private void RebuildBreadcrumbAndHeadOfHUD()
    {
        hud.RebuildBreadcrumbAndUpdateHead(c.GetOpIdOfPage());
    }

    private void LoadUIData()
    {
        btnSearch.ToolTip = Resources.Lang.SearchPanel_btnSearch_Hint;
        btnClear.ToolTip = Resources.Lang.SearchPanel_btnClear_Hint;

        //HUD
        if (empAuth.CanEditThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.Edit, true);
            hud.SetButtonAttribute(HudButtonNameEnum.Edit, HudButtonAttributeEnum.JsInNavigateUrl,
                string.Format("popWin('Article-Config.aspx?act={0}&artid={1}', 700, 600);", ConfigFormAction.edit, c.qsArtId));
        }

        if (empAuth.CanAddSubItemInThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
            hud.SetButtonAttribute(HudButtonNameEnum.AddNew, HudButtonAttributeEnum.JsInNavigateUrl,
                string.Format("popWin('Article-Config.aspx?act={0}&artid={1}', 700, 600);", ConfigFormAction.add, c.qsArtId));
        }

        //conditions UI

        //condition vlaues
        txtKw.Text = c.qsKw;

        //columns of list
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        //Response.Redirect(c.BuildUrlOfListPage(c.qsId, sortField, isSortDesc));
    }
}