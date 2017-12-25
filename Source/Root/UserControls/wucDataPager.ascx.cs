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
    /// 頁碼的參數名稱 (default: p)
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
        btnPreviousPage.Title = Resources.Lang.Pager_btnPrevious_Hint;
        btnNextPage.Title = Resources.Lang.Pager_btnNext_Hint;
    }

    #region Public methods

    /// <summary>
    /// assign itemTotalCount first, then currentPageCode (to recalculate page code range)
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

        // pager buttons
        if (dataPager.CanShowPreviousButton)
        {
            PreviousPageArea.Attributes["class"] = "";
            btnPreviousPage.HRef = GetLinkUrlToReload(dataPager.CurrentPageCode - 1);
        }
        else
        {
            PreviousPageArea.Attributes["class"] = "disabled";
            btnPreviousPage.HRef = "";
        }

        if (dataPager.CanShowNextButton)
        {
            NextPageArea.Attributes["class"] = "";
            btnNextPage.HRef = GetLinkUrlToReload(dataPager.CurrentPageCode + 1);
        }
        else
        {
            NextPageArea.Attributes["class"] = "disabled";
            btnNextPage.HRef = "";
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
            PageCodeArea.Attributes["class"] = "active";
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
}