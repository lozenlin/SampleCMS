﻿using Common.LogicObject;
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
        LoadExtLimitations();
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

    private void LoadExtLimitations()
    {
        if (attFileMgr.FileExtLimitations != null)
        {
            string extCombined = string.Join(", ", attFileMgr.FileExtLimitations.ToArray());
            ltrExtLimitations.Text = extCombined;
            ExtLimitationsArea.Visible = true;
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

            if (c.qsAct == ConfigFormAction.add)
            {

            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
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