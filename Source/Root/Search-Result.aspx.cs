using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Search_Result : FrontendBasePage
{
    protected SearchPageCommon c;
    protected ArticlePublisherLogic artPub;
    protected IMasterArticleSettings masterSettings;

    private string keywords;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new SearchPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        if (!c.RetrieveArticleIdAndData())
        {
            Response.Redirect(c.ERROR_PAGE);
        }

        articleData = c.GetArticleData();
        artPub = new ArticlePublisherLogic(null);
        masterSettings = (IMasterArticleSettings)this.Master;
        masterSettings.ShowBreadcrumbAndSearchArea = false;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ucDataPager.MaxItemCountOfPage = 10;
        ucDataPager.MaxDisplayCountInPageCodeArea = 5;
        ucDataPager.LinkUrlToReload = string.Concat(Request.AppRelativeCurrentExecutionFilePath, "?", Request.QueryString);
        ucDataPager.Initialize(0, 0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
            DisplaySearchResult();

            txtKeyword.Focus();
        }
    }

    private void LoadUIData()
    {
        txtKeyword.Attributes["placeholder"] = Resources.Lang.SearchResult_txtKeyword_Hint;
        txtKeyword.ToolTip = Resources.Lang.SearchResult_txtKeyword_Hint;
        btnToSearchResult.ToolTip = Resources.Lang.SearchResult_btnSearch_Hint;
    }

    private void DisplaySearchResult()
    {
        keywords = c.qsQueryKeyword.Trim();
        int total = 0;

        if (keywords != "")
        {
            char[] separators = new char[] { ',', ';', ' ', '　' };
            bool isMultiKeyword = false;

            //檢查是否為多項關鍵字
            foreach (char symbol in separators)
            {
                if (keywords.Contains(symbol.ToString()))
                {
                    isMultiKeyword = true;
                }
            }

            //重新用逗號串接搜尋關鍵字
            if (isMultiKeyword)
            {
                string[] tokens = keywords.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                keywords = string.Join(",", tokens);
            }

            SearchResultListQueryParams param = new SearchResultListQueryParams()
            {
                Keywords = keywords,
                CultureName = c.qsCultureNameOfLangNo
            };

            param.PagedParams = new PagedListQueryParams()
            {
                BeginNum = 0,
                EndNum = 0,
                SortField = "",
                IsSortDesc = false
            };

            // get total of items
            artPub.GetSearchDataSourceList(param);

            // update pager and get begin end of item numbers
            int itemTotalCount = param.PagedParams.RowCount;
            ucDataPager.Initialize(itemTotalCount, c.qsPageCode);

            param.PagedParams = new PagedListQueryParams()
            {
                BeginNum = ucDataPager.BeginItemNumberOfPage,
                EndNum = ucDataPager.EndItemNumberOfPage,
                SortField = "",
                IsSortDesc = false
            };

            DataSet dsResult = artPub.GetSearchDataSourceList(param);

            if (dsResult != null)
            {
                total = param.PagedParams.RowCount;
                rptResultItems.DataSource = dsResult.Tables[0];
                rptResultItems.DataBind();
            }
        }

        //關鍵字
        ltrKeywords.Text = c.qsQueryKeyword.Trim();

        //總筆數
        ltrTotal.Text = total.ToString();
    }

    protected void rptResultItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid articleId = (Guid)drvTemp["ArticleId"];
        string articleSubject = drvTemp.ToSafeStr("ArticleSubject");
        string articleContext = drvTemp.ToSafeStr("ArticleContext");
        string linkUrl = drvTemp.ToSafeStr("LinkUrl");
        string breadcrumbData = drvTemp.ToSafeStr("BreadcrumbData");

        HtmlAnchor btnSubject = (HtmlAnchor)e.Item.FindControl("btnSubject");
        btnSubject.InnerHtml = articleSubject;
        btnSubject.Title = articleSubject;
        btnSubject.HRef = string.Format("{0}?artid={1}&l={2}", linkUrl, articleId, c.qsLangNo);

        Literal ltrContext = (Literal)e.Item.FindControl("ltrContext");

        int maxLength = 350;

        if (new LangManager().GetCultureName(c.qsLangNo.ToString()) == "en")
        {
            maxLength = 700;
        }

        string shortArticleContext = articleContext;

        if (shortArticleContext.Length > maxLength)
        {
            shortArticleContext = shortArticleContext.Substring(0, maxLength) + "...";
        }

        if (!string.IsNullOrEmpty(keywords))
        {
            //加亮關鍵字
            string[] items = keywords.Split(',');

            foreach (string kw in items)
            {
                //處理特殊字元
                string tempKw = kw;

                foreach (char cSpecial in @"\.$^{[(|)*+?")
                {
                    string strSpecial = cSpecial.ToString();

                    if (tempKw.Contains(strSpecial))
                    {
                        tempKw = tempKw.Replace(strSpecial, @"\" + strSpecial);
                    }
                }

                string pattern = string.Format(@"(?is)(?<key>{0})", tempKw);
                shortArticleContext = Regex.Replace(shortArticleContext, pattern, "<span class='label label-warning'>${key}</span>");
            }
        }

        ltrContext.Text = shortArticleContext;

        Literal ltrBreadcrumb = (Literal)e.Item.FindControl("ltrBreadcrumb");
        string[] routeArray = breadcrumbData.Split(',');
        int itemLength = routeArray.Length;

        if (itemLength % 2 == 1)
        {
            itemLength--;
        }

        StringBuilder sbBreadcrumb = new StringBuilder(200);

        for (int i = 0; i < itemLength - 1; i += 2)
        {
            //加超連結,最後一個不加
            if (i == itemLength - 2)
            {
                // <li class="active">Data</li>
                sbBreadcrumb.AppendFormat("<li class='active'>{0}</li>", routeArray[i]);
            }
            else
            {
                // <li><a href="#">Library</a></li>
                sbBreadcrumb.AppendFormat("<li><a href='Article.aspx?artid={0}&l={2}' target='_blank' title='{1}'>{1}</a></li>", routeArray[i + 1], routeArray[i], c.qsLangNo);
            }
        }

        ltrBreadcrumb.Text = sbBreadcrumb.ToString();
    }

    protected void btnToSearchResult_Click(object sender, EventArgs e)
    {
        string keyword = txtKeyword.Text.Trim();

        if (keyword == "")
            return;

        c.GoToSearchResult(keyword);
    }
}