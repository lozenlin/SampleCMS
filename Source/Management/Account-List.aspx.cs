using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_List : System.Web.UI.Page
{
    protected AccountCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new AccountCommonOfBackend(this.Context, this.ViewState);
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
        hud.RebuildBreadcrumbAndUpdateHead(c.GetOpIdOfPage());
        Title = hud.GetHeadText() + " - " + Title;

        if (!IsPostBack)
        {
            if (!empAuth.CanOpenThisPage())
            {
                Response.Redirect(c.BACK_END_HOME);
            }

            LoadUIData();
            DisplayAccounts();
        }
    }

    private void LoadUIData()
    {
        //HUD
        if (empAuth.CanAddSubItemInThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
        }

        //conditions UI
        LoadListModeUIData();
        LoadDeptUIData();

        //condition vlaues
        ddlListMode.SelectedValue = c.qsListMode.ToString();
        ddlDept.SelectedValue = c.qsDeptId.ToString();
        txtKw.Text = c.qsKw;

        c.LoadSortCols(new string[] { 
            "DeptName", "RoleSortNo", "EmpName", 
            "EmpAccount", "StartDate"
        });
    }

    private void LoadListModeUIData()
    {
        ddlListMode.Items.Clear();
        ddlListMode.Items.Add(new ListItem("(全部)", "0"));
        ddlListMode.Items.Add(new ListItem("正常", "1"));
        ddlListMode.Items.Add(new ListItem("已停權", "2"));
    }

    private void LoadDeptUIData()
    {
        ddlDept.Items.Clear();
        DataSet dsDept = empAuth.GetDepartmentListToSelect();

        if (dsDept != null)
        {
            ddlDept.DataTextField = "DeptName";
            ddlDept.DataValueField = "DeptId";
            ddlDept.DataSource = dsDept.Tables[0];
            ddlDept.DataBind();
        }

        ddlDept.Items.Insert(0, new ListItem("(全部)", "0"));
    }

    private void DisplayAccounts()
    {
        
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        Response.Redirect(c.BuildUrlOfListPage(c.qsListMode, c.qsDeptId, c.qsKw,
            sortField, isSortDesc, c.qsPageCode));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtKw.Text = txtKw.Text.Trim();

        Response.Redirect(c.BuildUrlOfListPage(Convert.ToInt32(ddlListMode.SelectedValue), Convert.ToInt32(ddlDept.SelectedValue), txtKw.Text,
            "", false, 1));
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(StringUtility.SetParaValueInUrl(Request.AppRelativeCurrentExecutionFilePath, "l", c.qsLangNo));
    }
}