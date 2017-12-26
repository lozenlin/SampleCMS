using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_wucBreadcrumb : System.Web.UI.UserControl
{
    protected FrontendPageCommon c;
    protected ArticlePublisherLogic artPub;
    protected FrontendBasePage basePage;
    protected ArticleData articleData;

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new FrontendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(null);
        basePage = (FrontendBasePage)this.Page;
        articleData = basePage.GetArticleData();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DisplayBreadcrumb();
        }
    }

    public string GetBreadcrumbTextItemHtml(string subject)
    {
        return string.Format("<li class='active'>{0}</li>", subject);
    }

    public string GetBreadcrumbLinkItemHtml(string subject, string title, string href)
    {
        return string.Format("<li><a href='{0}' title='{1}'>{2}</a></li>", href, title, subject);
    }

    private void DisplayBreadcrumb()
    {
        if (!articleData.ArticleId.HasValue)
            return;

        // add home node
        ltrBreadcrumb.Text += GetBreadcrumbLinkItemHtml("首頁", "首頁", "Index.aspx?l=" + c.qsLangNo.ToString());

        Guid articleId = articleData.ArticleId.Value;
        DataSet dsLevelInfo = artPub.GetArticleMultiLangLevelInfo(articleId, c.qsCultureNameOfLangNo);

        if (dsLevelInfo != null && dsLevelInfo.Tables[0].Rows.Count > 0)
        {
            int total = dsLevelInfo.Tables[0].Rows.Count;
            for (int i = total - 1; i >= 0; i--)
            {
                DataRow drLevelInfo = dsLevelInfo.Tables[0].Rows[i];

                Guid itemId = (Guid)drLevelInfo["ArticleId"];

                if (itemId == Guid.Empty)
                {
                    continue;
                }

                string itemSubject = drLevelInfo.ToSafeStr("ArticleSubject");
                bool isHideSelf = Convert.ToBoolean(drLevelInfo["IsHideSelf"]);
                bool isShowInLang = Convert.ToBoolean(drLevelInfo["IsShowInLang"]);
                DateTime startDate = Convert.ToDateTime(drLevelInfo["StartDate"]);
                DateTime endDate = Convert.ToDateTime(drLevelInfo["EndDate"]);

                if (startDate <= DateTime.Today && DateTime.Today <= endDate
                    && !isHideSelf
                    && isShowInLang)
                {
                    if (i == 0)
                    {
                        ltrBreadcrumb.Text += GetBreadcrumbTextItemHtml(itemSubject);
                    }
                    else
                    {
                        string href = string.Format("Article.aspx?artid={0}&l={1}", itemId, c.qsLangNo);
                        ltrBreadcrumb.Text += GetBreadcrumbLinkItemHtml(itemSubject, itemSubject, href);
                    }
                }
            }
        }
    }
}