using Common.LogicObject;
using Common.Utility;
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
    protected ArticlePublisherLogic artPub;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfSubPages();

        artPub = new ArticlePublisherLogic();
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
            DataSet dsArticle = artPub.GetArticleDataForBackend(c.qsArtId);

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
                DataSet dsZhTw = artPub.GetArticleMultiLangDataForBackend(c.qsArtId, LangManager.CultureNameZHTW);

                if (dsZhTw != null && dsZhTw.Tables[0].Rows.Count > 0)
                {
                    DataRow drZhTw = dsZhTw.Tables[0].Rows[0];

                    txtArticleSubjectZhTw.Text = drZhTw.ToSafeStr("ArticleSubject");
                    chkIsShowInLangZhTw.Checked = Convert.ToBoolean(drZhTw["IsShowInLang"]);
                    txtCkeContextZhTw.Text = drZhTw["ArticleContext"].ToString();

                    if (!Convert.IsDBNull(drZhTw["MdfDate"]) && Convert.ToDateTime(drZhTw["MdfDate"]) > mdfDate)
                    {
                        mdfAccount = drZhTw.ToSafeStr("MdfAccount");
                        mdfDate = Convert.ToDateTime(drZhTw["MdfDate"]);
                    }
                }

                //en
                DataSet dsEn = artPub.GetArticleMultiLangDataForBackend(c.qsArtId, LangManager.CultureNameEN);

                if (dsEn != null && dsEn.Tables[0].Rows.Count > 0)
                {
                    DataRow drEn = dsEn.Tables[0].Rows[0];

                    txtArticleSubjectEn.Text = drEn.ToSafeStr("ArticleSubject");
                    chkIsShowInLangEn.Checked = Convert.ToBoolean(drEn["IsShowInLang"]);
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
            int newSortNo = artPub.GetArticleMaxSortNo(c.qsArtId) + 10;
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
            txtArticleAlias.Text = txtArticleAlias.Text.Trim();
            txtBannerPicFileName.Text = txtBannerPicFileName.Text.Trim();
            txtLinkUrl.Text = txtLinkUrl.Text.Trim();
            txtControlName.Text = txtControlName.Text.Trim();
            txtSubItemControlName.Text = txtSubItemControlName.Text.Trim();

            ArticleParams param = new ArticleParams()
            {
                ArticleAlias = txtArticleAlias.Text,
                BannerPicFileName = txtBannerPicFileName.Text,
                LayoutModeId = Convert.ToInt32(rdolLayoutMode.SelectedValue),
                ShowTypeId = Convert.ToInt32(rdolShowType.SelectedValue),
                LinkUrl = txtLinkUrl.Text,
                LinkTarget = chkIsNewWindow.Checked ? "_blank" : "",
                ControlName = txtControlName.Text,
                SubItemControlName = txtSubItemControlName.Text,
                IsHideSelf = chkIsHideSelf.Checked,
                IsHideChild = chkIsHideChild.Checked,
                StartDate = Convert.ToDateTime(txtStartDate.Text),
                EndDate = Convert.ToDateTime(txtEndDate.Text),
                SortNo = Convert.ToInt32(txtSortNo.Text),
                DontDelete = chkDontDelete.Checked,
                PostAccount = c.GetEmpAccount()
            };

            txtArticleSubjectZhTw.Text = txtArticleSubjectZhTw.Text.Trim();
            txtCkeContextZhTw.Text = StringUtility.GetSievedHtmlEditorValue(txtCkeContextZhTw.Text);

            ArticleMultiLangParams paramZhTw = new ArticleMultiLangParams()
            {
                CultureName = LangManager.CultureNameZHTW,
                ArticleSubject = txtArticleSubjectZhTw.Text,
                ArticleContext = txtCkeContextZhTw.Text,
                IsShowInLang = chkIsShowInLangZhTw.Checked,
                PostAccount = c.GetEmpAccount()
            };

            txtArticleSubjectEn.Text = txtArticleSubjectEn.Text.Trim();
            txtCkeContextEn.Text = StringUtility.GetSievedHtmlEditorValue(txtCkeContextEn.Text);

            ArticleMultiLangParams paramEn = new ArticleMultiLangParams()
            {
                CultureName = LangManager.CultureNameEN,
                ArticleSubject = txtArticleSubjectEn.Text,
                ArticleContext = txtCkeContextEn.Text,
                IsShowInLang = chkIsShowInLangEn.Checked,
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {
                Guid newArticleId = Guid.NewGuid();
                param.ArticleId = newArticleId;
                param.ParentId = c.qsArtId;

                if (param.ArticleAlias == "")
                {
                    param.ArticleAlias = newArticleId.ToString();
                }

                result = artPub.InsertArticleData(param);

                if (result)
                {
                    //ZhTw
                    if (result)
                    {
                        paramZhTw.ArticleId = param.ArticleId;
                        result = artPub.InsertArticleMultiLangData(paramZhTw);
                    }

                    //En
                    if (result)
                    {
                        paramEn.ArticleId = param.ArticleId;
                        result = artPub.InsertArticleMultiLangData(paramEn);
                    }

                    if (!result)
                    {
                        Master.ShowErrorMsg("新增多國語系資料失敗");
                    }
                }
                else
                {
                    if (param.HasIdBeenUsed)
                    {
                        Master.ShowErrorMsg("網頁代碼已被使用");
                    }
                    else if (param.HasAliasBeenUsed)
                    {
                        Master.ShowErrorMsg("網址別名已被使用");
                    }
                    else
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                    }
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.ArticleId = c.qsArtId;

                if (param.ArticleAlias == "")
                {
                    param.ArticleAlias = c.qsArtId.ToString();
                }

                result = artPub.UpdateArticleData(param);

                if (result)
                {
                    //ZhTw
                    if (result)
                    {
                        paramZhTw.ArticleId = param.ArticleId;
                        result = artPub.UpdateArticleMultiLangData(paramZhTw);
                    }

                    //En
                    if (result)
                    {
                        paramEn.ArticleId = param.ArticleId;
                        result = artPub.UpdateArticleMultiLangData(paramEn);
                    }

                    if (!result)
                    {
                        Master.ShowErrorMsg("更新多國語系資料失敗");
                    }
                }
                else
                {
                    if (param.HasAliasBeenUsed)
                    {
                        Master.ShowErrorMsg("網址別名已被使用");
                    }
                    else
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                    }
                }
            }

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Config"), true);
            }

            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = c.GetEmpAccount(),
                Description = string.Format("．{0}　．儲存網頁/Save article[{1}][{2}]　結果/result[{3}]", Title, txtArticleSubjectZhTw.Text, txtArticleSubjectEn.Text, result),
                IP = c.GetClientIP()
            });
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            Master.ShowErrorMsg(ex.Message);
        }
    }
}