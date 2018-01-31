using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article_Picture : System.Web.UI.Page
{
    protected ArticlePictureCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;
    protected ArticlePictureManagerLogic artPicMgr;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticlePictureCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(c);

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.SetCustomEmployeeAuthorizationResult(artPub);
        empAuth.InitialAuthorizationResultOfSubPages();

        artPicMgr = new ArticlePictureManagerLogic(this.Context);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!artPicMgr.Initialize(c.qsPicId, c.qsArtId))
        {
            string errMsg = ResUtility.GetErrMsgOfAttFileErrState(artPicMgr.GetErrState());
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
        rfvSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_IntegerOnly;
        rfvPicSubjectZhTw.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvPicSubjectEn.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvPickedFile.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;

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
        if (artPicMgr.FileExtLimitations != null && artPicMgr.FileExtLimitations.Count > 0)
        {
            string extCombined = string.Join(", ", artPicMgr.FileExtLimitations.ToArray());
            ltrExtLimitations.Text = extCombined;
            ExtLimitationsArea.Visible = true;

            if (artPicMgr.FileMimeLimitations != null && artPicMgr.FileMimeLimitations.Count > 0)
            {
                string acceptList = string.Join(",", artPicMgr.FileMimeLimitations.ToArray());
                fuPickedFile.Attributes.Add("accept", acceptList);
            }
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format(Resources.Lang.ArticlePicture_Title_AddNew_Format, c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.ArticlePicture_Title_Edit_Format, c.qsPicId);
    }

    private void DisplayArticlePictureData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            txtSortNo.Text = artPicMgr.SortNo.ToString();
            ltrPostAccount.Text = artPicMgr.PostAccount;
            ltrPostDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", artPicMgr.PostDate);
            CurFileArea.Visible = true;
            ltrFileSavedName.Text = artPicMgr.FileSavedName;
            ltrDownloadAtt.Text = Resources.Lang.Article_btnDownloadArtPic;
            btnDownloadAtt.Title = Resources.Lang.Article_btnDownloadArtPic_Hint;
            btnDownloadAtt.HRef = string.Format("FileArtPic.ashx?attid={0}&saveas=1", c.qsPicId);

            // cancel required field rule
            rfvPickedFile.ValidationGroup = "none";

            if (artPicMgr.MdfDate.HasValue)
            {
                ltrMdfAccount.Text = artPicMgr.MdfAccount;
                ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", artPicMgr.MdfDate);
            }

            //zh-TW
            if (LangManager.IsEnableEditLangZHTW())
            {
                txtPicSubjectZhTw.Text = artPicMgr.AttSubjectZhTw;
                chkIsShowInLangZhTw.Checked = artPicMgr.IsShowInLangZhTw;
            }

            //en
            if (LangManager.IsEnableEditLangEN())
            {
                txtPicSubjectEn.Text = artPicMgr.AttSubjectEn;
                chkIsShowInLangEn.Checked = artPicMgr.IsShowInLangEn;
            }

            btnSave.Visible = true;
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            txtSortNo.Text = artPicMgr.SortNo.ToString();
            btnSave.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Master.ShowErrorMsg("");

        if (!IsValid)
            return;

        if (c.qsAct == ConfigFormAction.add && !fuPickedFile.HasFile)
            return;

        try
        {
            bool result = false;
            txtPicSubjectZhTw.Text = txtPicSubjectZhTw.Text.Trim();
            txtPicSubjectEn.Text = txtPicSubjectEn.Text.Trim();

            artPicMgr.SortNo = Convert.ToInt32(txtSortNo.Text);
            artPicMgr.AttSubjectZhTw = txtPicSubjectZhTw.Text;
            artPicMgr.AttSubjectEn = txtPicSubjectEn.Text;
            artPicMgr.IsShowInLangZhTw = chkIsShowInLangZhTw.Checked;
            artPicMgr.IsShowInLangEn = chkIsShowInLangEn.Checked;

            result = artPicMgr.SaveData(fuPickedFile, c.GetEmpAccount());

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Picture"), true);
            }
            else
            {
                string errMsg = ResUtility.GetErrMsgOfAttFileErrState(artPicMgr.GetErrState());

                if (errMsg == "")
                {
                    errMsg = Resources.Lang.ErrMsg_SaveFailed;
                }

                Master.ShowErrorMsg(errMsg);
            }

            //新增後端操作記錄
            string description = string.Format("．{0}　．儲存網頁照片/Save article picture[{1}][{2}]" +
                "　有檔案/has file[{3}]　PicId[{4}]　結果/result[{5}]",
                Title, txtPicSubjectZhTw.Text, txtPicSubjectEn.Text,
                fuPickedFile.HasFile, artPicMgr.AttId, result);

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