using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Back_End_Log : BasePage
{
    protected BackEndLogCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new BackEndLogCommonOfBackend(this.Context, this.ViewState);
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
            DisplayLogs();
        }
    }

    private void LoadUIData()
    {
        btnSearch.ToolTip = Resources.Lang.SearchPanel_btnSearch_Hint;
        btnClear.ToolTip = Resources.Lang.SearchPanel_btnClear_Hint;

        //HUD

        //conditions UI
        LoadTimeUIData();
        LoadRangeModeUIData();

        //condition vlaues
        txtStartDate.Text = string.Format("{0:yyyy-MM-dd}", c.qsStartDateOfQuery);
        ddlHourStart.SelectedValue = c.qsStartDateOfQuery.Hour.ToString();
        ddlMinStart.SelectedValue = c.qsStartDateOfQuery.Minute.ToString();
        ddlSecStart.SelectedValue = c.qsStartDateOfQuery.Second.ToString();
        txtEndDate.Text = string.Format("{0:yyyy-MM-dd}", c.qsEndDateOfQuery);
        ddlHourEnd.SelectedValue = c.qsEndDateOfQuery.Hour.ToString();
        ddlMinEnd.SelectedValue = c.qsEndDateOfQuery.Minute.ToString();
        ddlSecEnd.SelectedValue = c.qsEndDateOfQuery.Second.ToString();
        txtAccount.Text = c.qsAccount;
        chkIsAccKw.Checked = c.qsIsAccKw;
        txtIP.Text = c.qsIP;
        chkIsIpHeadKw.Checked = c.qsIsIpHeadKw;
        txtDescKw.Text = c.qsDescKw;
        ddlRangeMode.SelectedValue = c.qsRangeMode.ToString();

        //columns of list
        btnSortOpDate.Text = Resources.Lang.Col_OpDate;
        hidSortOpDate.Text = btnSortOpDate.Text;
        btnSortIP.Text = Resources.Lang.Col_IP;
        hidSortIP.Text = btnSortIP.Text;
        btnSortEmpName.Text = Resources.Lang.Col_EmpNameOfLog;
        hidSortEmpName.Text = btnSortEmpName.Text;
        btnSortDescription.Text = Resources.Lang.Col_LogRecord;
        hidSortDescription.Text = btnSortDescription.Text;

        c.DisplySortableCols(new string[] { 
            "OpDate", "IP", "EmpName", 
            "Description"
        });
    }

    private void LoadTimeUIData()
    {
        for (int hour = 0; hour <= 23; hour++)
        {
            string text = hour.ToString("00");
            string value = hour.ToString();

            ddlHourStart.Items.Add(new ListItem(text, value));
            ddlHourEnd.Items.Add(new ListItem(text, value));
        }

        ddlHourStart.SelectedValue = "0";
        ddlHourEnd.SelectedValue = "23";

        for (int min = 0; min <= 59; min++)
        {
            string text = min.ToString("00");
            string value = min.ToString();

            ddlMinStart.Items.Add(new ListItem(text, value));
            ddlMinEnd.Items.Add(new ListItem(text, value));

            ddlSecStart.Items.Add(new ListItem(text, value));
            ddlSecEnd.Items.Add(new ListItem(text, value));
        }

        ddlMinStart.SelectedValue = "0";
        ddlMinEnd.SelectedValue = "59";

        ddlSecStart.SelectedValue = "0";
        ddlSecEnd.SelectedValue = "59";
    }

    private void LoadRangeModeUIData()
    {
        ddlRangeMode.Items.Clear();
        ddlRangeMode.Items.Add(new ListItem(Resources.Lang.SearchOption_All, "0"));
        ddlRangeMode.Items.Add(new ListItem(Resources.Lang.SearchOption_LoginRelated, "1"));
    }

    private void DisplayLogs()
    {
        BackEndLogListQueryParams param = new BackEndLogListQueryParams()
        {
            StartDate = c.qsStartDateOfQuery,
            EndDate = c.qsEndDateOfQuery,
            Account = c.qsAccount,
            IsAccKw = c.qsIsAccKw,
            IP = c.qsIP,
            IsIpHeadKw = c.qsIsIpHeadKw,
            DescKw = c.qsDescKw,
            RangeMode = c.qsRangeMode
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
        empAuth.GetBackEndLogList(param);

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

        DataSet dsLogs = empAuth.GetBackEndLogList(param);

        if (dsLogs != null)
        {
            rptLogs.DataSource = dsLogs.Tables[0];
            rptLogs.DataBind();
        }

        if (c.qsPageCode > 1 || c.qsSortField != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "isSearchPanelCollapsingAtBeginning", "isSearchPanelCollapsingAtBeginning = true;", true);
        }
    }

    protected void rptLogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        string empAccount = drvTemp.ToSafeStr("EmpAccount");
        string empName = drvTemp.ToSafeStr("EmpName");

        Literal ltrEmpName = (Literal)e.Item.FindControl("ltrEmpName");
        ltrEmpName.Text = string.Format("{0}({1})", empName, empAccount);

        Literal ltrDescription = (Literal)e.Item.FindControl("ltrDescription");
        ltrDescription.Text = drvTemp.ToSafeStr("Description").Replace("　．", "<br>．");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtAccount.Text = txtAccount.Text.Trim();
        txtIP.Text = txtIP.Text.Trim();
        DateTime startDate;
        string strStartDate = txtStartDate.Text + " " + ddlHourStart.SelectedValue + ":" + ddlMinStart.SelectedValue + ":" + ddlSecStart.SelectedValue;
        if (!DateTime.TryParse(strStartDate, out startDate))
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_InvalidOpDateStart);
            return;
        }

        DateTime endDate;
        string strEndDate = txtEndDate.Text + " " + ddlHourEnd.SelectedValue + ":" + ddlMinEnd.SelectedValue + ":" + ddlSecEnd.SelectedValue;
        if (!DateTime.TryParse(strEndDate, out endDate))
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_InvalidOpDateEnd);
            return;
        }

        txtDescKw.Text = txtDescKw.Text.Trim();

        Response.Redirect(c.BuildUrlOfListPage(txtAccount.Text, txtIP.Text, startDate,
            endDate, chkIsAccKw.Checked, chkIsIpHeadKw.Checked,
            txtDescKw.Text, Convert.ToInt32(ddlRangeMode.SelectedValue), "",
            false, 1));
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
        Response.Redirect(c.BuildUrlOfListPage(c.qsAccount, c.qsIP, c.qsStartDateOfQuery,
            c.qsEndDateOfQuery, c.qsIsAccKw, c.qsIsIpHeadKw,
            c.qsDescKw, c.qsRangeMode, sortField,
            isSortDesc, c.qsPageCode));
    }
}