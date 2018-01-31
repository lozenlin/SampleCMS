using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article_Video : System.Web.UI.Page
{
    protected ArticleVideoCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleVideoCommonOfBackend(this.Context, this.ViewState);
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
            if (!empAuth.CanEditThisPage())
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            LoadUIData();
            DisplayArticleVideoData();
            txtSortNo.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        rfvSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_IntegerOnly;
        rfvVidSubjectZhTw.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvVidSubjectEn.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvSourceVideoId.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        btnGetYoutubeId.OnClientClick = "return confirm('" + Resources.Lang.ArticleVideo_btnGetYoutubeId_Confirm + "');";

        SetupLangRelatedFields();
    }

    /// <summary>
    /// 設定語系相關欄位
    /// </summary>
    private void SetupLangRelatedFields()
    {
        if (!LangManager.IsEnableEditLangZHTW())
        {
            VidSubjectZhTwArea.Visible = false;
            chkIsShowInLangZhTw.Checked = false;
            chkIsShowInLangZhTw.Visible = false;
            VidDescZhTwArea.Visible = false;
        }

        if (!LangManager.IsEnableEditLangEN())
        {
            VidSubjectEnArea.Visible = false;
            chkIsShowInLangEn.Checked = false;
            chkIsShowInLangEn.Visible = false;
            VidDescEnArea.Visible = false;
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format(Resources.Lang.ArticleVideo_Title_AddNew_Format, c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.ArticleVideo_Title_Edit_Format, c.qsPicId);
    }

    private void DisplayArticleVideoData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            DataSet dsVideo = artPub.GetArticleVideoDataForBackend(c.qsVidId);

            if (dsVideo != null && dsVideo.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsVideo.Tables[0].Rows[0];

                txtSortNo.Text = drFirst.ToSafeStr("SortNo");
                txtVidLinkUrl.Text = drFirst.ToSafeStr("VidLinkUrl");
                txtSourceVideoId.Text = drFirst.ToSafeStr("SourceVideoId");
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
                    DataSet dsZhTw = artPub.GetArticleVideoMultiLangDataForBackend(c.qsVidId, LangManager.CultureNameZHTW);

                    if (dsZhTw != null && dsZhTw.Tables[0].Rows.Count > 0)
                    {
                        DataRow drZhTw = dsZhTw.Tables[0].Rows[0];

                        txtVidSubjectZhTw.Text = drZhTw.ToSafeStr("VidSubject");
                        chkIsShowInLangZhTw.Checked = Convert.ToBoolean(drZhTw["IsShowInLang"]);
                        txtVidDescZhTw.Text = drZhTw.ToSafeStr("VidDesc");

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
                    DataSet dsEn = artPub.GetArticleVideoMultiLangDataForBackend(c.qsVidId, LangManager.CultureNameEN);

                    if (dsEn != null && dsEn.Tables[0].Rows.Count > 0)
                    {
                        DataRow drEn = dsEn.Tables[0].Rows[0];

                        txtVidSubjectEn.Text = drEn.ToSafeStr("VidSubject");
                        chkIsShowInLangEn.Checked = Convert.ToBoolean(drEn["IsShowInLang"]);
                        txtVidDescEn.Text = drEn.ToSafeStr("VidDesc");

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
            int newSortNo = artPub.GetArticleVideoMaxSortNo(c.qsArtId) + 10;
            txtSortNo.Text = newSortNo.ToString();

            btnSave.Visible = true;
        }
    }

    protected void btnGetYoutubeId_Click(object sender, EventArgs e)
    {
        txtVidLinkUrl.Text = txtVidLinkUrl.Text.Trim();
        txtSourceVideoId.Text = StringUtility.GetYoutubeIdFromUrl(txtVidLinkUrl.Text);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Master.ShowErrorMsg("");

        if (!IsValid)
            return;

        try
        {
            txtVidLinkUrl.Text = txtVidLinkUrl.Text.Trim();
            txtSourceVideoId.Text = txtSourceVideoId.Text.Trim();

            ArticleVideoParams param = new ArticleVideoParams()
            {
                SortNo = Convert.ToInt32(txtSortNo.Text),
                VidLinkUrl = txtVidLinkUrl.Text,
                SourceVideoId = txtSourceVideoId.Text,
                PostAccount = c.GetEmpAccount()
            };

            txtVidSubjectZhTw.Text = txtVidSubjectZhTw.Text.Trim();
            txtVidDescZhTw.Text = txtVidDescZhTw.Text.Trim();

            ArticleVideoMultiLangParams paramZhTw = new ArticleVideoMultiLangParams()
            {
                CultureName = LangManager.CultureNameZHTW,
                VidSubject = txtVidSubjectZhTw.Text,
                IsShowInLang = chkIsShowInLangZhTw.Checked,
                VidDesc = txtVidDescZhTw.Text,
                PostAccount = c.GetEmpAccount()
            };

            txtVidSubjectEn.Text = txtVidSubjectEn.Text.Trim();
            txtVidDescEn.Text = txtVidDescEn.Text.Trim();

            ArticleVideoMultiLangParams paramEn = new ArticleVideoMultiLangParams()
            {
                CultureName = LangManager.CultureNameEN,
                VidSubject = txtVidSubjectEn.Text,
                IsShowInLang = chkIsShowInLangEn.Checked,
                VidDesc = txtVidDescEn.Text,
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {
                Guid newVidId = Guid.NewGuid();
                param.VidId = newVidId;
                param.ArticleId = c.qsArtId;

                result = artPub.InsertArticleVideoData(param);

                if (result)
                {
                    //zh-TW
                    if (result && LangManager.IsEnableEditLangZHTW())
                    {
                        paramZhTw.VidId = param.VidId;
                        result = artPub.InsertArticleVideoMultiLangData(paramZhTw);
                    }

                    //en
                    if (result && LangManager.IsEnableEditLangEN())
                    {
                        paramEn.VidId = param.VidId;
                        result = artPub.InsertArticleVideoMultiLangData(paramEn);
                    }

                    if (!result)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddMultiLangFailed);
                    }
                }
                else
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.VidId = c.qsVidId;

                result = artPub.UpdateArticleVideoData(param);

                if (result)
                {
                    //zh-TW
                    if (result && LangManager.IsEnableEditLangZHTW())
                    {
                        paramZhTw.VidId = param.VidId;
                        result = artPub.UpdateArticleVideoMultiLangData(paramZhTw);
                    }

                    //en
                    if (result && LangManager.IsEnableEditLangEN())
                    {
                        paramEn.VidId = param.VidId;
                        result = artPub.UpdateArticleVideoMultiLangData(paramEn);
                    }

                    if (!result)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateMultiLangFailed);
                    }
                }
                else
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateFailed);
                }
            }

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Video"), true);
            }

            //新增後端操作記錄
            string description = string.Format("．{0}　．儲存網頁影片/Save article video[{1}][{2}]　VidId[{3}]　結果/result[{4}]",
                Title, txtVidSubjectZhTw.Text, txtVidSubjectEn.Text, param.VidId, result);

            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = c.GetEmpAccount(),
                Description = description,
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