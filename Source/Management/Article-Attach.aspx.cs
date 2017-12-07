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
    protected ArticleCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;
    protected AttachFileManagerLogic attFileMgr;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
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
            Master.ShowErrorMsg("初始化附件資料失敗");
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
            Title = string.Format("新增網頁附件 - 網頁id:{0}", c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改網頁附件 - 附件id:{0}", c.qsAttId);
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

    private string GetErrMsgOfAttFileErrState(AttFileErrState errState)
    {
        string errMsg = "";

        switch (attFileMgr.GetErrState())
        {
            case AttFileErrState.LoadDataFailed:
                errMsg = "載入資料失敗";
                break;
            case AttFileErrState.LoadMultiLangDataFailed:
                errMsg = "載入多語系資料失敗";
                break;
            case AttFileErrState.AttachFileIsRequired:
                errMsg = "請上傳檔案";
                break;
            case AttFileErrState.InvalidFileExt:
                errMsg = "不允許的檔案類型";
                break;
            case AttFileErrState.NoInitialize:
                errMsg = "請先執行初始化";
                break;
            case AttFileErrState.DeleteDataFailed:
                errMsg = "刪除附件失敗";
                break;
            case AttFileErrState.DeletePhysicalFileFailed:
                errMsg = "刪除實體檔案失敗";
                break;
            case AttFileErrState.SavePhysicalFileFailed:
                errMsg = "儲存實體檔案失敗";
                break;
            case AttFileErrState.InsertDataFailed:
                errMsg = "新增附件資料失敗";
                break;
            case AttFileErrState.InsertMultiLangDataFailed:
                errMsg = "新增附件多語系資料失敗";
                break;
            case AttFileErrState.UpdateDataFailed:
                errMsg = "更新附件資料失敗";
                break;
            case AttFileErrState.UpdateMultiLangDataFailed:
                errMsg = "更新附件多語系資料失敗";
                break;
            case AttFileErrState.AttIdIsRequired:
                errMsg = "請提供 AttId";
                break;
        }

        return errMsg;
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
                string errMsg = GetErrMsgOfAttFileErrState(attFileMgr.GetErrState());

                if (errMsg == "")
                {
                    errMsg = "儲存附件失敗";
                }

                Master.ShowErrorMsg(errMsg);
            }

            //新增後端操作記錄
            string description = string.Format("．{0}　．儲存附件/Save attach file[{1}][{2}]" +
                "　有檔案/has file[{3}]　結果/result[{4}]",
                Title, txtAttSubjectZhTw.Text, txtAttSubjectEn.Text,
                fuPickedFile.HasFile, result);

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