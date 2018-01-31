using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Article_Attach : System.Web.UI.Page
{
    protected ArticleAttachCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;
    protected AttachFileManagerLogic attFileMgr;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleAttachCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        artPub = new ArticlePublisherLogic(c);

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.SetCustomEmployeeAuthorizationResult(artPub);
        empAuth.InitialAuthorizationResultOfSubPages();

        attFileMgr = new AttachFileManagerLogic(this.Context);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!attFileMgr.Initialize(c.qsAttId, c.qsArtId))
        {
            string errMsg = ResUtility.GetErrMsgOfAttFileErrState(attFileMgr.GetErrState());
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
            DisplayAttachFileData();
            txtSortNo.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        rfvSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_IntegerOnly;
        rfvAttSubjectZhTw.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvAttSubjectEn.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvPickedFile.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        chkDontDelete.Text = Resources.Lang.Article_chkDontDelete;

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
            AttSubjectZhTwArea.Visible = false;
            chkIsShowInLangZhTw.Checked = false;
            chkIsShowInLangZhTw.Visible = false;
        }

        if (!LangManager.IsEnableEditLangEN())
        {
            AttSubjectEnArea.Visible = false;
            chkIsShowInLangEn.Checked = false;
            chkIsShowInLangEn.Visible = false;
        }
    }

    private void LoadExtAndMimeLimitations()
    {
        if (attFileMgr.FileExtLimitations != null && attFileMgr.FileExtLimitations.Count > 0)
        {
            string extCombined = string.Join(", ", attFileMgr.FileExtLimitations.ToArray());
            ltrExtLimitations.Text = extCombined;
            ExtLimitationsArea.Visible = true;

            if (attFileMgr.FileMimeLimitations != null && attFileMgr.FileMimeLimitations.Count > 0)
            {
                string acceptList = string.Join(",", attFileMgr.FileMimeLimitations.ToArray());
                fuPickedFile.Attributes.Add("accept", acceptList);
            }
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format(Resources.Lang.Attachment_Title_AddNew_Format, c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.Attachment_Title_Edit_Format, c.qsAttId);
    }

    private void DisplayAttachFileData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            txtSortNo.Text = attFileMgr.SortNo.ToString();
            chkDontDelete.Checked = attFileMgr.DontDelete;
            ltrPostAccount.Text = attFileMgr.PostAccount;
            ltrPostDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", attFileMgr.PostDate);
            CurFileArea.Visible = true;
            ltrFileSavedName.Text = attFileMgr.FileSavedName;
            ltrDownloadAtt.Text = Resources.Lang.Article_btnDownloadAtt;
            btnDownloadAtt.Title = Resources.Lang.Article_btnDownloadAtt_Hint;
            btnDownloadAtt.HRef = string.Format("FileAtt.ashx?attid={0}", c.qsAttId);

            // cancel required field rule
            rfvPickedFile.ValidationGroup = "none";

            if (attFileMgr.MdfDate.HasValue)
            {
                ltrMdfAccount.Text = attFileMgr.MdfAccount;
                ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", attFileMgr.MdfDate);
            }

            //zh-TW
            if (LangManager.IsEnableEditLangZHTW())
            {
                txtAttSubjectZhTw.Text = attFileMgr.AttSubjectZhTw;
                chkIsShowInLangZhTw.Checked = attFileMgr.IsShowInLangZhTw;
            }

            //en
            if (LangManager.IsEnableEditLangEN())
            {
                txtAttSubjectEn.Text = attFileMgr.AttSubjectEn;
                chkIsShowInLangEn.Checked = attFileMgr.IsShowInLangEn;
            }

            btnSave.Visible = true;
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            txtSortNo.Text = attFileMgr.SortNo.ToString();
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
            txtAttSubjectZhTw.Text = txtAttSubjectZhTw.Text.Trim();
            txtAttSubjectEn.Text = txtAttSubjectEn.Text.Trim();

            attFileMgr.SortNo = Convert.ToInt32(txtSortNo.Text);
            attFileMgr.AttSubjectZhTw = txtAttSubjectZhTw.Text;
            attFileMgr.AttSubjectEn = txtAttSubjectEn.Text;
            attFileMgr.IsShowInLangZhTw = chkIsShowInLangZhTw.Checked;
            attFileMgr.IsShowInLangEn = chkIsShowInLangEn.Checked;
            attFileMgr.DontDelete = chkDontDelete.Checked;

            result = attFileMgr.SaveData(fuPickedFile, c.GetEmpAccount());

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Attach"), true);
            }
            else
            {
                string errMsg = ResUtility.GetErrMsgOfAttFileErrState(attFileMgr.GetErrState());

                if (errMsg == "")
                {
                    errMsg = Resources.Lang.ErrMsg_SaveFailed;
                }

                Master.ShowErrorMsg(errMsg);
            }

            //新增後端操作記錄
            string description = string.Format("．{0}　．儲存附件/Save attach file[{1}][{2}]" +
                "　有檔案/has file[{3}]　AttId[{4}]　結果/result[{5}]",
                Title, txtAttSubjectZhTw.Text, txtAttSubjectEn.Text,
                fuPickedFile.HasFile, attFileMgr.AttId, result);

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