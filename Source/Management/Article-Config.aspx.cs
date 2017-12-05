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
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(c);

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.SetCustomEmployeeAuthorizationResult(artPub);
        empAuth.InitialAuthorizationResultOfSubPages();
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
        rfvSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_IntegerOnly;
        rfvStartDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covStartDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_InvalidDate;
        rfvEndDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covEndDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_InvalidDate;
        rfvArticleSubjectZhTw.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvArticleSubjectEn.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;

        LoadLayoutModeUIData();
        LoadShowTypeUIData();

        chkIsNewWindow.Text = Resources.Lang.Col_OpenInNewWindow_Hint;
        chkIsHideSelf.Text = Resources.Lang.Article_chkIsHideSelf;
        chkIsHideChild.Text = Resources.Lang.Article_chkIsHideChild;
        chkDontDelete.Text = Resources.Lang.Article_chkDontDelete;

        SetupLangRelatedFields();
    }

    /// <summary>
    /// 設定語系相關欄位
    /// </summary>
    private void SetupLangRelatedFields()
    {
        if (!LangManager.IsEnableEditLangZHTW())
        {
            ArticleSubjectZhTwArea.Visible = false;
            PreviewBannerZhTwArea.Visible = false;
            chkIsShowInLangZhTw.Checked = false;
            chkIsShowInLangZhTw.Visible = false;
            ContextTabZhTwArea.Visible = false;
            ContextPnlZhTwArea.Visible = false;
        }

        if (!LangManager.IsEnableEditLangEN())
        {
            ArticleSubjectEnArea.Visible = false;
            PreviewBannerEnArea.Visible = false;
            chkIsShowInLangEn.Checked = false;
            chkIsShowInLangEn.Visible = false;
            ContextTabEnArea.Visible = false;
            ContextPnlEnArea.Visible = false;
        }
    }

    private void LoadLayoutModeUIData()
    {
        rdolLayoutMode.Items.Clear();
        rdolLayoutMode.Items.Add(new ListItem(Resources.Lang.LayoutModeOption_WideContent + "　", "1"));
        rdolLayoutMode.Items.Add(new ListItem(Resources.Lang.LayoutModeOption_TwoColContent + "　", "2"));
        rdolLayoutMode.Items[0].Selected = true;
    }

    private void LoadShowTypeUIData()
    {
        rdolShowType.Items.Clear();
        rdolShowType.Items.Add(new ListItem(Resources.Lang.PageShowTypeOption_Page + "　", "1"));
        rdolShowType.Items.Add(new ListItem(Resources.Lang.PageShowTypeOption_ToSubPage + "　", "2"));
        rdolShowType.Items.Add(new ListItem(Resources.Lang.PageShowTypeOption_URL + "　", "3"));
        rdolShowType.Items.Add(new ListItem(Resources.Lang.PageShowTypeOption_UseControl + "　", "4"));
        rdolShowType.Items[0].Selected = true;
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format(Resources.Lang.Article_Title_AddNew_Format, c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.Article_Title_Edit_Format, c.qsArtId);
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
                if (LangManager.IsEnableEditLangZHTW())
                {
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
                }

                //en
                if (LangManager.IsEnableEditLangEN())
                {
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
                    if (result && LangManager.IsEnableEditLangZHTW())
                    {
                        paramZhTw.ArticleId = param.ArticleId;
                        result = artPub.InsertArticleMultiLangData(paramZhTw);
                    }

                    //En
                    if (result && LangManager.IsEnableEditLangEN())
                    {
                        paramEn.ArticleId = param.ArticleId;
                        result = artPub.InsertArticleMultiLangData(paramEn);
                    }

                    if (!result)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddMultiLangFailed);
                    }
                }
                else
                {
                    if (param.HasIdBeenUsed)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ArticleIdHasBeenUsed);
                    }
                    else if (param.HasAliasBeenUsed)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ArticleAliasHasBeenUsed);
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
                    if (result && LangManager.IsEnableEditLangZHTW())
                    {
                        paramZhTw.ArticleId = param.ArticleId;
                        result = artPub.UpdateArticleMultiLangData(paramZhTw);
                    }

                    //En
                    if (result && LangManager.IsEnableEditLangEN())
                    {
                        paramEn.ArticleId = param.ArticleId;
                        result = artPub.UpdateArticleMultiLangData(paramEn);
                    }

                    if (!result)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateMultiLangFailed);
                    }
                }
                else
                {
                    if (param.HasAliasBeenUsed)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ArticleAliasHasBeenUsed);
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