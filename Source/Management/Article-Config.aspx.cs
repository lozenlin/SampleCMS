using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article_Config : System.Web.UI.Page
{
    protected ArticleCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    protected ArticlePublisherLogic articlePub;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfSubPages();

        articlePub = new ArticlePublisherLogic();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Authenticate
            if (c.qsAct == ConfigFormAction.edit && !empAuth.CanEditThisPage()
                || c.qsAct == ConfigFormAction.add && !empAuth.CanAddSubItemInThisPage())
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            LoadUIData();
            DisplayArticleData();
            txtSortNo.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format("新增網頁 - id:{0}", c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改網頁 - id:{0}", c.qsArtId);
    }

    private void DisplayArticleData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            DataSet dsArticle = articlePub.GetArticleDataForBackend(c.qsArtId);

            if (dsArticle != null && dsArticle.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsArticle.Tables[0].Rows[0];

                txtSortNo.Text = drFirst.ToSafeStr("SortNo");
                txtStartDate.Text = string.Format("{0:yyyy-MM-dd}", drFirst["StartDate"]);
                txtEndDate.Text = string.Format("{0:yyyy-MM-dd}", drFirst["EndDate"]);
                txtBannerPicFileName.Text = drFirst.ToSafeStr("BannerPicFileName");
                txtArticleAlias.Text = drFirst.ToSafeStr("ArticleAlias");
                rdolLayoutMode.SelectedValue = drFirst.ToSafeStr("LayoutModeId");
                rdolShowType.SelectedValue = drFirst.ToSafeStr("ShowTypeId");
                txtLinkUrl.Text = drFirst.ToSafeStr("LinkUrl");
                string linkTarget = drFirst.ToSafeStr("LinkTarget");
                chkIsNewWindow.Checked = (linkTarget == "_blank");
                txtControlName.Text = drFirst.ToSafeStr("ControlName");
                txtSubItemControlName.Text = drFirst.ToSafeStr("SubItemControlName");
                chkIsHideSelf.Checked = Convert.ToBoolean(drFirst["IsHideSelf"]);
                chkIsHideChild.Checked = Convert.ToBoolean(drFirst["IsHideChild"]);
                chkDontDelete.Checked = Convert.ToBoolean(drFirst["DontDelete"]);
                ltrPostAccount.Text = drFirst.ToSafeStr("PostAccount");
                ltrPostDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", drFirst["PostDate"]);
                string mdfAccount = drFirst.ToSafeStr("MdfAccount");
                DateTime mdfDate = DateTime.MinValue;

                if (!Convert.IsDBNull(drFirst["MdfDate"]))
                {
                    mdfDate = Convert.ToDateTime(drFirst["MdfDate"]);
                }

                //zh-TW
                DataSet dsZhTw = articlePub.GetArticleMultiLangDataForBackend(c.qsArtId, LangManager.CultureNameZHTW);

                if (dsZhTw != null && dsZhTw.Tables[0].Rows.Count > 0)
                {
                    DataRow drZhTw = dsZhTw.Tables[0].Rows[0];

                    txtArticleSubjectZhTw.Text = drZhTw.ToSafeStr("ArticleSubject");
                    IsShowInLangZhTw.Checked = Convert.ToBoolean(drZhTw["IsShowInLang"]);
                    txtCkeContextZhTw.Text = drZhTw["ArticleContext"].ToString();

                    if (!Convert.IsDBNull(drZhTw["MdfDate"]) && Convert.ToDateTime(drZhTw["MdfDate"]) > mdfDate)
                    {
                        mdfAccount = drZhTw.ToSafeStr("MdfAccount");
                        mdfDate = Convert.ToDateTime(drZhTw["MdfDate"]);
                    }
                }

                //en
                DataSet dsEn = articlePub.GetArticleMultiLangDataForBackend(c.qsArtId, LangManager.CultureNameEN);

                if (dsEn != null && dsEn.Tables[0].Rows.Count > 0)
                {
                    DataRow drEn = dsEn.Tables[0].Rows[0];

                    txtArticleSubjectEn.Text = drEn.ToSafeStr("ArticleSubject");
                    IsShowInLangEn.Checked = Convert.ToBoolean(drEn["IsShowInLang"]);
                    txtCkeContextEn.Text = drEn["ArticleContext"].ToString();

                    if (!Convert.IsDBNull(drEn["MdfDate"]) && Convert.ToDateTime(drEn["MdfDate"]) > mdfDate)
                    {
                        mdfAccount = drEn.ToSafeStr("MdfAccount");
                        mdfDate = Convert.ToDateTime(drEn["MdfDate"]);
                    }
                }

                if (mdfDate != DateTime.MinValue)
                {
                    ltrMdfAccount.Text = mdfAccount;
                    ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", mdfDate);
                }

                btnSave.Visible = true;
            }
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            int newSortNo = articlePub.GetArticleMaxSortNo(c.qsArtId) + 10;
            txtSortNo.Text = newSortNo.ToString();
            DateTime startDate = DateTime.Today.AddDays(3);
            DateTime endDate = startDate.AddYears(10);
            txtStartDate.Text = string.Format("{0:yyyy-MM-dd}", startDate);
            txtEndDate.Text = string.Format("{0:yyyy-MM-dd}", endDate);

            btnSave.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Master.ShowErrorMsg("");

        if (!IsValid)
            return;

        try
        {
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            Master.ShowErrorMsg(ex.Message);
        }
    }
}