using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article_Picture : System.Web.UI.Page
{
    protected ArticleCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;
    protected ArticlePictureManagerLogic artPicFileMgr;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(c);

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.SetCustomEmployeeAuthorizationResult(artPub);
        empAuth.InitialAuthorizationResultOfSubPages();

        artPicFileMgr = new ArticlePictureManagerLogic(this.Context);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!artPicFileMgr.Initialize(c.qsAttId, c.qsArtId))
        {
            string errMsg = ResUtility.GetErrMsgOfAttFileErrState(artPicFileMgr.GetErrState());
            Master.ShowErrorMsg(errMsg);
            return;
        }

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
            DisplayArticlePictureData();
            txtSortNo.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {

        SetupLangRelatedFields();
        LoadExtAndMimeLimitations();
    }

    /// <summary>
    /// 設定語系相關欄位
    /// </summary>
    private void SetupLangRelatedFields()
    {
        if (!LangManager.IsEnableEditLangZHTW())
        {
            PicSubjectZhTwArea.Visible = false;
            chkIsShowInLangZhTw.Checked = false;
            chkIsShowInLangZhTw.Visible = false;
        }

        if (!LangManager.IsEnableEditLangEN())
        {
            PicSubjectEnArea.Visible = false;
            chkIsShowInLangEn.Checked = false;
            chkIsShowInLangEn.Visible = false;
        }
    }

    private void LoadExtAndMimeLimitations()
    {
        if (artPicFileMgr.FileExtLimitations != null && artPicFileMgr.FileExtLimitations.Count > 0)
        {
            string extCombined = string.Join(", ", artPicFileMgr.FileExtLimitations.ToArray());
            ltrExtLimitations.Text = extCombined;
            ExtLimitationsArea.Visible = true;

            if (artPicFileMgr.FileMimeLimitations != null && artPicFileMgr.FileMimeLimitations.Count > 0)
            {
                string acceptList = string.Join(",", artPicFileMgr.FileMimeLimitations.ToArray());
                fuPickedFile.Attributes.Add("accept", acceptList);
            }
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format("新增網頁照片 - 網頁id:{0}", c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改網頁照片 - 照片id:{0}", c.qsPicId);
    }

    private void DisplayArticlePictureData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            txtSortNo.Text = artPicFileMgr.SortNo.ToString();
            ltrPostAccount.Text = artPicFileMgr.PostAccount;
            ltrPostDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", artPicFileMgr.PostDate);
            CurFileArea.Visible = true;
            ltrFileSavedName.Text = artPicFileMgr.FileSavedName;
            ltrDownloadAtt.Text = Resources.Lang.Article_btnDownloadAtt;
            btnDownloadAtt.Title = Resources.Lang.Article_btnDownloadAtt_Hint;
            btnDownloadAtt.HRef = string.Format("FileAtt.ashx?attid={0}", c.qsAttId);

            // cancel required field rule
            rfvPickedFile.ValidationGroup = "none";

            if (artPicFileMgr.MdfDate.HasValue)
            {
                ltrMdfAccount.Text = artPicFileMgr.MdfAccount;
                ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", artPicFileMgr.MdfDate);
            }

            //zh-TW
            if (LangManager.IsEnableEditLangZHTW())
            {
                txtPicSubjectZhTw.Text = artPicFileMgr.AttSubjectZhTw;
                chkIsShowInLangZhTw.Checked = artPicFileMgr.IsShowInLangZhTw;
            }

            //en
            if (LangManager.IsEnableEditLangEN())
            {
                txtPicSubjectEn.Text = artPicFileMgr.AttSubjectEn;
                chkIsShowInLangEn.Checked = artPicFileMgr.IsShowInLangEn;
            }

            btnSave.Visible = true;
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            txtSortNo.Text = artPicFileMgr.SortNo.ToString();
            btnSave.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}