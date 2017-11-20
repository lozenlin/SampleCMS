// ===============================================================================
// wucDataPager(UserControl) of EmployeeAuthority of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// wucDataPager.ascx
// wucDataPager.ascx.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

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

public partial class UserControls_wucDataPager : System.Web.UI.UserControl
{

    #region Public properties

    /// <summary>
    /// 重新載入用的連結內容
    /// </summary>
    public string LinkUrlToReload
    {
        get { return linkUrlToReload; }
        set { linkUrlToReload = value; }
    }
    private string linkUrlToReload = "";

    /// <summary>
    /// 頁碼的參數名稱
    /// </summary>
    public string PageCodeParaName
    {
        get { return pageCodeParaName; }
        set { pageCodeParaName = value; }
    }
    private string pageCodeParaName = "p";

    /// <summary>
    /// 每頁最大筆數
    /// </summary>
    public int MaxItemCountOfPage
    {
        get { return dataPager.MaxItemCountOfPage; }
        set { dataPager.MaxItemCountOfPage = value; }
    }

    /// <summary>
    /// 分頁區最多顯示頁碼數
    /// </summary>
    public int MaxDisplayCountInPageCodeArea
    {
        get { return dataPager.MaxDisplayCountInPageCodeArea; }
        set { dataPager.MaxDisplayCountInPageCodeArea = value; }
    }

    /// <summary>
    /// 目前頁碼
    /// </summary>
    public int CurrentPageCode
    {
        get { return dataPager.CurrentPageCode; }
    }

    /// <summary>
    /// 總筆數
    /// </summary>
    public int ItemTotalCount
    {
        get { return dataPager.ItemTotalCount; }
        set
        {
            dataPager.ItemTotalCount = value;
            PaginationArea.Visible = value > 0;
            PaginationInfoArea.Visible = PaginationArea.Visible;
        }
    }

    /// <summary>
    /// 總頁數
    /// </summary>
    public int PageTotalCount
    {
        get { return dataPager.PageTotalCount; }
    }

    /// <summary>
    /// 顯示的第一個頁碼
    /// </summary>
    public int FirstDisplayPageCode
    {
        get { return dataPager.FirstDisplayPageCode; }
    }

    /// <summary>
    /// 顯示的最後一個頁碼
    /// </summary>
    public int LastDisplayPageCode
    {
        get { return dataPager.LastDisplayPageCode; }
    }

    /// <summary>
    /// 頁面中的起始編號
    /// </summary>
    public int BeginItemNumberOfPage
    {
        get { return dataPager.BeginItemNumberOfPage; }
    }

    /// <summary>
    /// 頁面中的結束編號
    /// </summary>
    public int EndItemNumberOfPage
    {
        get { return dataPager.EndItemNumberOfPage; }
    }

    /// <summary>
    /// 在第一頁
    /// </summary>
    public bool IsAtFirstPage
    {
        get { return dataPager.CurrentPageCode == 1; }
    }

    /// <summary>
    /// 在最後一頁
    /// </summary>
    public bool IsAtLastPage
    {
        get { return dataPager.CurrentPageCode == dataPager.PageTotalCount; }
    }

    #endregion

    private DataPagerLogic dataPager;

    protected void Page_Init(object sender, EventArgs e)
    {
        dataPager = new DataPagerLogic();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
            DisplayPagination();
        }
    }

    private void LoadUIData()
    {
        btnFirstPage.InnerHtml = Resources.Lang.Pager_btnFirstPage;
        btnFirstPage.Title = Resources.Lang.Pager_btnFirstPage_Hint;
        btnPreviousPage.Title = Resources.Lang.Pager_btnPrevious_Hint;
        btnNextPage.Title = Resources.Lang.Pager_btnNext_Hint;
        btnLastPage.InnerHtml = Resources.Lang.Pager_btnLast;
        btnLastPage.Title = Resources.Lang.Pager_btnLast_Hint;
        txtPageCode.ToolTip = Resources.Lang.Pager_txtPageCode_Hint;
        rfvPageCode.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rvPageCode.ErrorMessage = "*" + Resources.Lang.ErrMsg_InvalidPageCode;
        btnJumpToPage.Text = Resources.Lang.Pager_btnJumpToPage;
        btnJumpToPage.ToolTip = Resources.Lang.Pager_btnJumpToPage_Hint;
    }

    #region Public methods

    /// <summary>
    /// first is itemTotalCount, then currentPageCode (to recalculate page code range)
    /// </summary>
    public void Initialize(int itemTotalCount, int currentPageCode)
    {
        ItemTotalCount = itemTotalCount;
        dataPager.SetCurrentPageCodeAndRecalc(currentPageCode);
    }

    public void SetCurrentPageCodeAndRecalc(int pageCode)
    {
        dataPager.SetCurrentPageCodeAndRecalc(pageCode);
    }

    public void RefreshPagerAfterPostBack()
    {
        dataPager.CalculatePageCodeRange();
        DisplayPagination();
    }

    #endregion

    private void DisplayPagination()
    {
        if (dataPager.CurrentPageCode > 1
            && dataPager.PageTotalCount < dataPager.CurrentPageCode)
        {
            int newPageCode = 1;
            if (newPageCode < dataPager.PageTotalCount)
                newPageCode = dataPager.PageTotalCount;

            Response.Redirect(GetLinkUrlToReload(newPageCode));
        }

        // pagination data source
        DataTable dtPagination = new DataTable();
        dtPagination.Columns.Add("PageCode");

        for (int num = dataPager.FirstDisplayPageCode; num <= dataPager.LastDisplayPageCode; num++)
        {
            DataRow dr = dtPagination.NewRow();
            dr["PageCode"] = num;
            dtPagination.Rows.Add(dr);
        }

        rptPagination.DataSource = dtPagination;
        rptPagination.DataBind();

        // drop down list
        DataTable dtPageSelect = new DataTable();
        dtPageSelect.Columns.Add("Display");
        dtPageSelect.Columns.Add("Value");

        if (dataPager.PageTotalCount <= 50)
        {
            DataRow drNew = dtPageSelect.NewRow();
            int pageSelectCount = dataPager.PageTotalCount;

            if (pageSelectCount < 1)
                pageSelectCount = 1;

            for (int num = 1; num <= pageSelectCount; num++)
            {
                drNew = dtPageSelect.NewRow();
                drNew["Display"] = string.Format("- {0} -", num);
                drNew["Value"] = num;
                dtPageSelect.Rows.Add(drNew);
            }

            ddlPageSelect.DataTextField = "Display";
            ddlPageSelect.DataValueField = "Value";
            ddlPageSelect.DataSource = dtPageSelect;
            ddlPageSelect.DataBind();
        }
        else
        {
            //太多頁數時,改用輸入數字跳頁
            ddlPageSelect.Visible = false;
            TextCtrlArea.Visible = true;
        }

        ltrTotalCount.Text = string.Format(Resources.Lang.Pager_TotalCount_Format, dataPager.ItemTotalCount);
        ltrLastPageCode.Text = dataPager.PageTotalCount.ToString();
        ltrCurrentPageCode.Text = dataPager.CurrentPageCode.ToString();

        if (ddlPageSelect.Items.FindByValue(ltrCurrentPageCode.Text) != null)
            ddlPageSelect.SelectedValue = ltrCurrentPageCode.Text;

        // pager buttons
        if (dataPager.CanShowPreviousButton)
        {
            PreviousPageArea.Attributes["class"] = "page-item";
            btnPreviousPage.HRef = GetLinkUrlToReload(dataPager.CurrentPageCode - 1);
        }
        else
        {
            PreviousPageArea.Attributes["class"] = "page-item disabled";
            btnPreviousPage.HRef = "";
        }

        if (dataPager.CanShowNextButton)
        {
            NextPageArea.Attributes["class"] = "page-item";
            btnNextPage.HRef = GetLinkUrlToReload(dataPager.CurrentPageCode + 1);
        }
        else
        {
            NextPageArea.Attributes["class"] = "page-item disabled";
            btnNextPage.HRef = "";
        }

        if (dataPager.CanShowFirstButton)
        {
            btnFirstPage.Attributes["class"] = "";
            btnFirstPage.HRef = GetLinkUrlToReload(1);
        }
        else
        {
            btnFirstPage.Attributes["class"] = "text-muted";
            btnFirstPage.HRef = "";
        }

        if (dataPager.CanShowLastButton)
        {
            btnLastPage.Attributes["class"] = "";
            btnLastPage.HRef = GetLinkUrlToReload(dataPager.PageTotalCount);
        }
        else
        {
            btnLastPage.Attributes["class"] = "text-muted";
            btnLastPage.HRef = "";
        }
    }

    protected void rptPagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;
        int pageCode = Convert.ToInt32(drvTemp["PageCode"]);

        HtmlAnchor btnPageCode = (HtmlAnchor)e.Item.FindControl("btnPageCode");
        btnPageCode.InnerHtml = pageCode.ToString();
        btnPageCode.Title = btnPageCode.InnerHtml;
        btnPageCode.HRef = GetLinkUrlToReload(pageCode);

        if (dataPager.CurrentPageCode == pageCode)
        {
            HtmlGenericControl PageCodeArea = (HtmlGenericControl)e.Item.FindControl("PageCodeArea");
            PageCodeArea.Attributes["class"] = "page-item active";
            btnPageCode.HRef = "javascript:void(0);";            
        }
    }

    /// <summary>
    /// 取得重新載入用的連結內容
    /// </summary>
    private string GetLinkUrlToReload(int pageCode)
    {
        //設定網址中的參數值
        return StringUtility.SetParaValueInUrl(linkUrlToReload, pageCodeParaName, pageCode.ToString());
    }

    protected void ddlPageSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPageSelect.SelectedIndex < 0)
            return;

        Response.Redirect(GetLinkUrlToReload(Convert.ToInt32(ddlPageSelect.SelectedValue)));
    }

    protected void btnJumpToPage_Click(object sender, EventArgs e)
    {
        if (TextCtrlArea.Visible)
        {
            if (!Page.IsValid || txtPageCode.Text.Trim() == "")
                return;

            int pageCode = Convert.ToInt32(txtPageCode.Text.Trim());
            int pageTotalCount = Convert.ToInt32(ltrLastPageCode.Text);

            if (pageCode > pageTotalCount)
                pageCode = pageTotalCount;

            Response.Redirect(GetLinkUrlToReload(pageCode));
        }
    }
}