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
        btnGetYoutubeId.OnClientClick = "return confirm('確定要替換目前的 Youtube 影片代碼?');";

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
            Title = string.Format("新增 Youtube 影片 - 網頁id:{0}", c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改 Youtube 影片 - 影片id:{0}", c.qsPicId);
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
                    DataSet dsZhTw = artPub.GetArticleVideoMultiLangDataForBackend(c.qsVidId, c.seCultureNameOfBackend);

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
                    DataSet dsEn = artPub.GetArticleVideoMultiLangDataForBackend(c.qsVidId, c.seCultureNameOfBackend);

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

    }
}