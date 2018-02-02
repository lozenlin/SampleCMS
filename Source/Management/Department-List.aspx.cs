using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Department_List : BasePage
{
    protected DepartmentCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new DepartmentCommonOfBackend(this.Context, this.ViewState);
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
            DisplayDepartments();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {
                    DisplayDepartments();
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
                string.Format("popWin('Department-Config.aspx?act={0}', 700, 600);", ConfigFormAction.add));
        }

        //conditions UI

        //condition vlaues
        txtKw.Text = c.qsKw;

        //columns of list
        btnSortDeptName.Text = Resources.Lang.Col_DeptName;
        hidSortDeptName.Text = btnSortDeptName.Text;
        btnSortSortNo.Text = Resources.Lang.Col_SortNo;
        hidSortSortNo.Text = btnSortSortNo.Text;
        btnSortEmpTotal.Text = Resources.Lang.Col_EmpTotal;
        hidSortEmpTotal.Text = btnSortEmpTotal.Text;

        c.DisplySortableCols(new string[] { 
            "DeptName", "SortNo", "EmpTotal"
        });
    }

    private void DisplayDepartments()
    {
        DeptListQueryParams param = new DeptListQueryParams()
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
        empAuth.GetDepartmentList(param);

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

        DataSet dsRoles = empAuth.GetDepartmentList(param);

        if (dsRoles != null)
        {
            rptDepartments.DataSource = dsRoles.Tables[0];
            rptDepartments.DataBind();
        }

        if (c.qsPageCode > 1 || c.qsSortField != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "isSearchPanelCollapsingAtBeginning", "isSearchPanelCollapsingAtBeginning = true;", true);
        }
    }

    protected void rptDepartments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int deptId = Convert.ToInt32(drvTemp["DeptId"]);
        string deptName = drvTemp.ToSafeStr("DeptName");
        int empTotal = Convert.ToInt32(drvTemp["EmpTotal"]);

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Department-Config.aspx?act={0}&id={1}', 700, 600); return false;", ConfigFormAction.edit, deptId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", deptId.ToString(), deptName);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Dept_ConfirmDelete_Format + "');",
            deptId, deptName);

        string ownerAccount = drvTemp.ToSafeStr("PostAccount");
        int ownerDeptId = Convert.ToInt32(drvTemp["PostDeptId"]);

        btnEdit.Visible = empAuth.CanEditThisPage(false, ownerAccount, ownerDeptId);

        if (!empAuth.CanDelThisPage(ownerAccount, ownerDeptId)
            || empTotal > 0)
        {
            btnDelete.Visible = false;
        }
    }

    protected void rptDepartments_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                int deptId = Convert.ToInt32(args[0]);
                string deptName = args[1];
                DeptParams param = new DeptParams() { DeptId = deptId };

                bool result = empAuth.DeleteDepartmentData(param);

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除部門/Delete department　．代碼/id[{0}]　名稱/name[{1}]　結果/result[{2}]", deptId, deptName, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "dept-" + deptId.ToString() + "-" + deptName, result);

                if (result)
                {
                    DisplayDepartments();
                }
                else
                {
                    if (param.IsThereAccountsOfDept)
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ThereIsAccountOfDept);
                    else
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeleteDeptFailed);
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