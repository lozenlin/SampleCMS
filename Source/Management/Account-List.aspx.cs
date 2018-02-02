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

public partial class Account_List : BasePage
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
            DisplayAccounts();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {
                    DisplayAccounts();
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
                string.Format("popWin('Account-Config.aspx?act={0}', 700, 600);", ConfigFormAction.add));
        }

        //conditions UI
        LoadEmpRangeUIData();
        LoadDeptUIData();

        //condition vlaues
        ddlEmpRange.SelectedValue = c.qsEmpRange.ToString();
        ddlDept.SelectedValue = c.qsDeptId.ToString();
        txtKw.Text = c.qsKw;

        //columns of list
        btnSortDeptName.Text = Resources.Lang.Col_DeptName;
        hidSortDeptName.Text = btnSortDeptName.Text;
        btnSortRoleSortNo.Text = Resources.Lang.Col_Role;
        hidSortRoleSortNo.Text = btnSortRoleSortNo.Text;
        btnSortEmpName.Text = Resources.Lang.Col_EmpName;
        hidSortEmpName.Text = btnSortEmpName.Text;
        btnSortEmpAccount.Text = Resources.Lang.Col_EmpAccount;
        hidSortEmpAccount.Text = btnSortEmpAccount.Text;
        btnSortStartDate.Text = Resources.Lang.Col_ValidationDate;
        hidSortStartDate.Text = btnSortStartDate.Text;
        btnSortOwnerName.Text = Resources.Lang.Col_OwnerName;
        hidSortOwnerName.Text = btnSortOwnerName.Text;

        c.DisplySortableCols(new string[] { 
            "DeptName", "RoleSortNo", "EmpName", 
            "EmpAccount", "StartDate", "OwnerName"
        });
    }

    private void LoadEmpRangeUIData()
    {
        ddlEmpRange.Items.Clear();
        ddlEmpRange.Items.Add(new ListItem(Resources.Lang.SearchOption_All, "0"));
        ddlEmpRange.Items.Add(new ListItem(Resources.Lang.SearchOption_Normal, "1"));
        ddlEmpRange.Items.Add(new ListItem(Resources.Lang.SearchOption_AccessDenied, "2"));
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

        ddlDept.Items.Insert(0, new ListItem(Resources.Lang.SearchOption_All, "0"));
    }

    private void DisplayAccounts()
    {
        AccountListQueryParams param = new AccountListQueryParams()
        {
            ListMode = c.qsEmpRange,
            DeptId = c.qsDeptId,
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
        empAuth.GetAccountList(param);

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

        DataSet dsAccounts = empAuth.GetAccountList(param);

        if (dsAccounts != null)
        {
            rptAccounts.DataSource = dsAccounts.Tables[0];
            rptAccounts.DataBind();
        }

        if (c.qsPageCode > 1 || c.qsSortField != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "isSearchPanelCollapsingAtBeginning", "isSearchPanelCollapsingAtBeginning = true;", true);
        }
    }

    protected void rptAccounts_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int empId = Convert.ToInt32(drvTemp["EmpId"]);
        string empAccount = drvTemp.ToSafeStr("EmpAccount");
        string roleName = drvTemp.ToSafeStr("RoleName");
        bool isAccessDenied = Convert.ToBoolean(drvTemp["IsAccessDenied"]);
        DateTime startDate = Convert.ToDateTime(drvTemp["StartDate"]);
        DateTime endDate = Convert.ToDateTime(drvTemp["EndDate"]);
        string remarks = drvTemp.ToSafeStr("Remarks").Trim();

        HtmlGenericControl ctlRoleDisplayName = (HtmlGenericControl)e.Item.FindControl("ctlRoleDisplayName");
        ctlRoleDisplayName.InnerHtml = drvTemp.ToSafeStr("RoleDisplayName");
        ctlRoleDisplayName.Attributes["class"] = "RoleDisplay-" + roleName;

        HtmlTableRow EmpArea = (HtmlTableRow)e.Item.FindControl("EmpArea");

        if (isAccessDenied)
        {
            HtmlGenericControl ctlIsAccessDenied = (HtmlGenericControl)e.Item.FindControl("ctlIsAccessDenied");
            ctlIsAccessDenied.Attributes["title"] = Resources.Lang.Status_AccessDenied;
            ctlIsAccessDenied.Visible = true;

            EmpArea.Attributes["class"] = "table-danger";
        }

        HtmlGenericControl ctlAccountState = (HtmlGenericControl)e.Item.FindControl("ctlAccountState");

        if (DateTime.Today < startDate && empAccount != "admin")
        {
            // on schedule
            ctlAccountState.Attributes["class"] = "fa fa-hourglass-start fa-lg text-info";
            ctlAccountState.Attributes["title"] = Resources.Lang.Status_OnSchedule;
        }
        else if (endDate < DateTime.Today && empAccount != "admin" || isAccessDenied)
        {
            // offline
            ctlAccountState.Attributes["class"] = "fa fa-ban fa-lg text-danger";
            ctlAccountState.Attributes["title"] = Resources.Lang.Status_AccessDeniedOrExpired;
            EmpArea.Attributes["class"] = "table-danger";
        }
        else
        {
            // online
            ctlAccountState.Attributes["title"] = Resources.Lang.Status_Normal;
        }

        Literal ltrValidDateRange = (Literal)e.Item.FindControl("ltrValidDateRange");
        ltrValidDateRange.Text = string.Format("{0:yyyy-MM-dd} ~ {1:yyyy-MM-dd}", startDate, endDate);

        if (remarks != "")
        {
            HtmlGenericControl ctlRemarks = (HtmlGenericControl)e.Item.FindControl("ctlRemarks");
            ctlRemarks.Attributes["title"] = remarks;
            ctlRemarks.Visible = true;
        }

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Account-Config.aspx?act={0}&empid={1}', 700, 600); return false;", ConfigFormAction.edit, empId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", empId.ToString(), empAccount);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Account_ConfirmDelete_Format + "');",
            drvTemp.ToSafeStr("EmpName"), drvTemp.ToSafeStr("EmpAccount"));

        string ownerAccount = drvTemp.ToSafeStr("OwnerAccount");
        int ownerDeptId = Convert.ToInt32(drvTemp["OwnerDeptId"]);

        btnEdit.Visible = (empAuth.CanEditThisPage(false, ownerAccount, ownerDeptId) || c.IsMyAccount(empAccount));

        if (!empAuth.CanDelThisPage(ownerAccount, ownerDeptId)
            || empAccount == "admin"
            || empAccount == c.GetEmpAccount())
        {
            btnDelete.Visible = false;
        }
    }

    protected void rptAccounts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                int empId = Convert.ToInt32(args[0]);
                string empAccount = args[1];

                bool result = empAuth.DeleteEmployeeData(empId);

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除帳號/Delete account　．代碼/id[{0}]　帳號/account[{1}]　結果/result[{2}]", empId, empAccount, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "Emp-" + empId.ToString() + "-" + empAccount, result);

                if (result)
                {
                    DisplayAccounts();
                }
                else
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeleteAccountFailed);
                }

                break;
        }
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        Response.Redirect(c.BuildUrlOfListPage(c.qsEmpRange, c.qsDeptId, c.qsKw,
            sortField, isSortDesc, c.qsPageCode));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtKw.Text = txtKw.Text.Trim();

        Response.Redirect(c.BuildUrlOfListPage(Convert.ToInt32(ddlEmpRange.SelectedValue), Convert.ToInt32(ddlDept.SelectedValue), txtKw.Text,
            "", false, 1));
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.AppRelativeCurrentExecutionFilePath);
    }
}