using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Role_List : BasePage
{
    protected RoleCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new RoleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        c.SelectMenuItemToThisPage();

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfTopPage();

        hud = Master.GetHeadUpDisplay();
        isBackendPage = true;
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
            DisplayRoles();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {
                    DisplayRoles();
                }

                Master.FlagValue = "";
            }
        }
    }

    private void LoadUIData()
    {
        btnSearch.ToolTip = Resources.Lang.SearchPanel_btnSearch_Hint;
        btnClear.ToolTip = Resources.Lang.SearchPanel_btnClear_Hint;

        //HUD
        if (empAuth.CanAddSubItemInThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
            hud.SetButtonAttribute(HudButtonNameEnum.AddNew, HudButtonAttributeEnum.JsInNavigateUrl,
                string.Format("popWin('Role-Config.aspx?act={0}', 700, 600);", ConfigFormAction.add));
        }

        //conditions UI

        //condition vlaues
        txtKw.Text = c.qsKw;

        //columns of list
        btnSortRoleName.Text = Resources.Lang.Col_RoleName;
        hidSortRoleName.Text = btnSortRoleName.Text;
        btnSortRoleDisplayName.Text = Resources.Lang.Col_RoleDisplayName;
        hidSortRoleDisplayName.Text = btnSortRoleDisplayName.Text;
        btnSortSortNo.Text = Resources.Lang.Col_SortNo;
        hidSortSortNo.Text = btnSortSortNo.Text;
        btnSortEmpTotal.Text = Resources.Lang.Col_EmpTotal;
        hidSortEmpTotal.Text = btnSortEmpTotal.Text;

        c.DisplySortableCols(new string[] { 
            "RoleName", "RoleDisplayName", "SortNo", 
            "EmpTotal"
        });

    }

    private void DisplayRoles()
    {
        RoleListQueryParams param = new RoleListQueryParams()
        {
             Kw = c.qsKw
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 0,
            EndNum = 0,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        param.AuthParams = new AuthenticationQueryParams()
        {
            CanReadSubItemOfOthers = empAuth.CanReadSubItemOfOthers(),
            CanReadSubItemOfCrew = empAuth.CanReadSubItemOfCrew(),
            CanReadSubItemOfSelf = empAuth.CanReadSubItemOfSelf(),
            MyAccount = c.GetEmpAccount(),
            MyDeptId = c.GetDeptId()
        };

        // get total of items
        empAuth.GetEmployeeRoleList(param);

        // update pager and get begin end of item numbers
        int itemTotalCount = param.PagedParams.RowCount;
        ucDataPager.Initialize(itemTotalCount, c.qsPageCode);
        if (IsPostBack)
            ucDataPager.RefreshPagerAfterPostBack();

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = ucDataPager.BeginItemNumberOfPage,
            EndNum = ucDataPager.EndItemNumberOfPage,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        DataSet dsRoles = empAuth.GetEmployeeRoleList(param);

        if (dsRoles != null)
        {
            rptRoles.DataSource = dsRoles.Tables[0];
            rptRoles.DataBind();
        }

        if (c.qsPageCode > 1 || c.qsSortField != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "isSearchPanelCollapsingAtBeginning", "isSearchPanelCollapsingAtBeginning = true;", true);
        }
    }

    protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int roleId = Convert.ToInt32(drvTemp["RoleId"]);
        string roleName = drvTemp.ToSafeStr("RoleName");
        string roleDisplayName = drvTemp.ToSafeStr("RoleDisplayName");
        int empTotal = Convert.ToInt32(drvTemp["EmpTotal"]);

        HtmlGenericControl ctlRoleName = (HtmlGenericControl)e.Item.FindControl("ctlRoleName");
        ctlRoleName.InnerHtml = roleName;
        ctlRoleName.Attributes["class"] = "RoleDisplay-" + roleName;

        HtmlGenericControl ctlRoleDisplayName = (HtmlGenericControl)e.Item.FindControl("ctlRoleDisplayName");
        ctlRoleDisplayName.InnerHtml = roleDisplayName;
        ctlRoleDisplayName.Attributes["class"] = "RoleDisplay-" + roleName;

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Role-Config.aspx?act={0}&roleid={1}', 700, 600); return false;", ConfigFormAction.edit, roleId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        HtmlAnchor btnGrant = (HtmlAnchor)e.Item.FindControl("btnGrant");
        btnGrant.Attributes["onclick"] = string.Format("popWin('Role-Privilege.aspx?roleid={0}', 700, 600); return false;", roleId);
        btnGrant.Title = Resources.Lang.Main_btnGrant_Hint;

        Literal ltrGrant = (Literal)e.Item.FindControl("ltrGrant");
        ltrGrant.Text = Resources.Lang.Main_btnGrant;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", roleId.ToString(), roleName);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Role_ConfirmDelete_Format + "');",
            roleName, roleDisplayName);

        string ownerAccount = drvTemp.ToSafeStr("PostAccount");
        int ownerDeptId = Convert.ToInt32(drvTemp["PostDeptId"]);

        btnEdit.Visible = empAuth.CanEditThisPage(false, ownerAccount, ownerDeptId);
        btnGrant.Visible = btnEdit.Visible && (roleName != "admin");

        if (!empAuth.CanDelThisPage(ownerAccount, ownerDeptId)
            || roleName == "admin"
            || empTotal > 0)
        {
            btnDelete.Visible = false;
        }

    }

    protected void rptRoles_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                int roleId = Convert.ToInt32(args[0]);
                string roleName = args[1];
                RoleParams param = new RoleParams() { RoleId = roleId };

                bool result = empAuth.DeleteEmployeeRoleData(param);

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除身分/Delete role　．代碼/id[{0}]　名稱/name[{1}]　結果/result[{2}]", roleId, roleName, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "role-" + roleId.ToString() + "-" + roleName, result);

                if (result)
                {
                    DisplayRoles();
                }
                else
                {
                    if (param.IsThereAccountsOfRole)
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ThereIsAccountOfRole);
                    else
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeleteRoleFailed);
                }

                break;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtKw.Text = txtKw.Text.Trim();

        Response.Redirect(c.BuildUrlOfListPage(txtKw.Text, "", false,
            1));
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.AppRelativeCurrentExecutionFilePath);
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        Response.Redirect(c.BuildUrlOfListPage(c.qsKw, sortField, isSortDesc,
            c.qsPageCode));
    }
}