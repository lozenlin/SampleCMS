using Common.LogicObject;
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

        }

        LoadTitle();
    }

    private void LoadUIData()
    {

        SetupLangRelatedFields();
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

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format("新增網頁附件 - 網頁id:{0}", c.qsArtId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改網頁附件 - 附件id:{0}", c.qsAttId);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}