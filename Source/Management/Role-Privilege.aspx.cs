using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Role_Privilege : System.Web.UI.Page
{
    protected RoleCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    protected enum PvgTagNameEnum
    {
        NotAllowed,
        Read,
        Edit,
        Add,
        Delete
    }

    private int tempLv1Seqno = 0;
    private string tagHtmlNotAllowed;
    private string tagHtmlRead;
    private string tagHtmlEdit;
    private string tagHtmlAdd;
    private string tagHtmlDelete;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new RoleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        empAuth = new EmployeeAuthorityLogic(c);
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
            DisplayOperations();
            c.ClearRoleDataOfRoleOpPvgs(ltrRoleName.Text);
        }

        LoadTitle();
    }

    protected string GetTagHtml(PvgTagNameEnum pvgTagName)
    {
        string html = "";

        switch (pvgTagName)
        {
            case PvgTagNameEnum.NotAllowed:
                html = tagHtmlNotAllowed;
                break;
            case PvgTagNameEnum.Read:
                html = tagHtmlRead;
                break;
            case PvgTagNameEnum.Edit:
                html = tagHtmlEdit;
                break;
            case PvgTagNameEnum.Add:
                html = tagHtmlAdd;
                break;
            case PvgTagNameEnum.Delete:
                html = tagHtmlDelete;
                break;
        }

        return html;
    }

    private void LoadUIData()
    {
        tagHtmlNotAllowed = string.Format("<div><span class=\"badge badge-secondary pvg-badge\" title=\"{0}\">{1}</span></div>",
            Resources.Lang.Privilege_NotAllowed_Hint, Resources.Lang.Privilege_NotAllowed);
        tagHtmlRead = string.Format("<div><span class=\"badge badge-warning text-white pvg-badge\" title=\"{0}\">{1}</span></div>",
            Resources.Lang.Privilege_Read_Hint, Resources.Lang.Privilege_Read);
        tagHtmlEdit = string.Format("<div><span class=\"badge badge-success pvg-badge\" title=\"{0}\">{1}</span></div>",
            Resources.Lang.Privilege_Edit_Hint, Resources.Lang.Privilege_Edit);
        tagHtmlAdd = string.Format("<div><span class=\"badge badge-info pvg-badge\" title=\"{0}\">{1}</span></div>",
            Resources.Lang.Privilege_Add_Hint, Resources.Lang.Privilege_Add);
        tagHtmlDelete = string.Format("<div><span class=\"badge badge-primary pvg-badge\" title=\"{0}\">{1}</span></div>",
            Resources.Lang.Privilege_Delete_Hint, Resources.Lang.Privilege_Delete);

        LoadRoleInfoUIData();

    }

    private void LoadRoleInfoUIData()
    {
        DataSet dsRole = empAuth.GetEmployeeRoleData(c.qsRoleId);

        if (dsRole != null && dsRole.Tables[0].Rows.Count > 0)
        {
            DataRow drFirst = dsRole.Tables[0].Rows[0];

            ltrRoleDisplayName.Text = drFirst.ToSafeStr("RoleDisplayName");
            ltrRoleName.Text = drFirst.ToSafeStr("RoleName");
            hidRoleId.Value = c.qsRoleId.ToString();
        }
    }

    private void LoadTitle()
    {
        Title = string.Format(Resources.Lang.Role_Title_Grant_Format, c.qsRoleId);
    }

    private void DisplayOperations()
    {
        DataSet dsTopList = empAuth.GetOperationsTopListWithRoleAuth(ltrRoleName.Text);
        DataSet dsSubList = empAuth.GetOperationsSubListWithRoleAuth(ltrRoleName.Text);

        // move sub list table into dsTopList to join
        DataTable dtSubList = dsSubList.Tables[0];
        dtSubList.TableName = "SubList";
        dsSubList.Tables.Remove(dtSubList);
        dsSubList.Dispose();
        dsTopList.Tables.Add(dtSubList);

        DataRelation dataRel = dsTopList.Relations.Add("JoinTopSub", dsTopList.Tables[0].Columns["OpId"], dtSubList.Columns["ParentId"]);
        dataRel.Nested = true;

        rptOperations.DataSource = dsTopList.Tables[0];
        rptOperations.DataBind();

        btnSave.Visible = true;
    }

    protected void rptOperations_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int opId = Convert.ToInt32(drvTemp["OpId"]);
        string opSubject = drvTemp.ToSafeStr("OpSubject");
        bool canRead = drvTemp.To<bool>("CanRead", false);
        bool canEdit = drvTemp.To<bool>("CanEdit", false);
        bool canReadSubItemOfSelf = drvTemp.To<bool>("CanReadSubItemOfSelf", false);
        bool canEditSubItemOfSelf = drvTemp.To<bool>("CanEditSubItemOfSelf", false);
        bool canAddSubItemOfSelf = drvTemp.To<bool>("CanAddSubItemOfSelf", false);
        bool canDelSubItemOfSelf = drvTemp.To<bool>("CanDelSubItemOfSelf", false);
        bool canReadSubItemOfCrew = drvTemp.To<bool>("CanReadSubItemOfCrew", false);
        bool canEditSubItemOfCrew = drvTemp.To<bool>("CanEditSubItemOfCrew", false);
        bool canDelSubItemOfCrew = drvTemp.To<bool>("CanDelSubItemOfCrew", false);
        bool canReadSubItemOfOthers = drvTemp.To<bool>("CanReadSubItemOfOthers", false);
        bool canEditSubItemOfOthers = drvTemp.To<bool>("CanEditSubItemOfOthers", false);
        bool canDelSubItemOfOthers = drvTemp.To<bool>("CanDelSubItemOfOthers", false);

        string lastMdfAccount = "";
        DateTime? lastMdfDate = null;

        if (!Convert.IsDBNull(drvTemp["MdfDate"]))
        {
            lastMdfAccount = drvTemp.ToSafeStr("MdfAccount");
            lastMdfDate = Convert.ToDateTime(drvTemp["MdfDate"]);
        }
        else if (!Convert.IsDBNull(drvTemp["PostDate"]))
        {
            lastMdfAccount = drvTemp.ToSafeStr("PostAccount");
            lastMdfDate = Convert.ToDateTime(drvTemp["PostDate"]);
        }

        HtmlTableRow OpArea = (HtmlTableRow)e.Item.FindControl("OpArea");
        OpArea.Attributes.Add("opid", opId.ToString());

        Literal ltrSeqno = (Literal)e.Item.FindControl("ltrSeqno");

        if (sender == rptOperations)
        {
            OpArea.Attributes["class"] += " lv1";
            tempLv1Seqno = e.Item.ItemIndex + 1;
            ltrSeqno.Text = tempLv1Seqno.ToString();
        }
        else
        {
            OpArea.Attributes["class"] += " lv2";
            ltrSeqno.Text = string.Format("{0}-{1}", tempLv1Seqno, e.Item.ItemIndex + 1);
        }

        HtmlImage imgOpItem = (HtmlImage)e.Item.FindControl("imgOpItem");
        imgOpItem.Alt = opSubject;
        imgOpItem.Src = "~/BPimages/icon/data.gif";
        string iconImageFile = drvTemp.ToSafeStr("IconImageFile");
        if (!string.IsNullOrEmpty(iconImageFile))
            imgOpItem.Src = string.Format("~/BPimages/icon/{0}", iconImageFile);

        Literal ltrOpItemSubject = (Literal)e.Item.FindControl("ltrOpItemSubject");
        ltrOpItemSubject.Text = opSubject;

        if (lastMdfDate.HasValue)
        {
            Literal ltrLastUpdateInfo = (Literal)e.Item.FindControl("ltrLastUpdateInfo");

            string modificationInfo = string.Format(
                "<span class='mdf-info text-info' title='{0}: {1}, {2:yyyy-MM-dd HH:mm:ss}'><i class='fa fa-info-circle'></i></span>",
                Resources.Lang.Col_LastUpdate, lastMdfAccount, lastMdfDate);
            ltrLastUpdateInfo.Text = " " + modificationInfo;
        }

        Literal ltrPvgOfItem = (Literal)e.Item.FindControl("ltrPvgOfItem");
        HtmlInputHidden hidPvgOfItem = (HtmlInputHidden)e.Item.FindControl("hidPvgOfItem");
        int pvgOfItem = 0;

        if (!canRead)
        {
            ltrPvgOfItem.Text += tagHtmlNotAllowed;
        }

        if (canRead)
        {
            ltrPvgOfItem.Text += tagHtmlRead;
            pvgOfItem |= 1;
            hidPvgOfItem.Value = pvgOfItem.ToString();
        }

        if (canEdit)
        {
            ltrPvgOfItem.Text += tagHtmlEdit;
            pvgOfItem |= 2;
            hidPvgOfItem.Value = pvgOfItem.ToString();
        }

        Literal ltrPvgOfSubitemSelf = (Literal)e.Item.FindControl("ltrPvgOfSubitemSelf");
        HtmlInputHidden hidPvgOfSubitemSelf = (HtmlInputHidden)e.Item.FindControl("hidPvgOfSubitemSelf");
        int pvgOfSubitemSelf = 0;

        if (!canReadSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += tagHtmlNotAllowed;
        }

        if (canReadSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += tagHtmlRead;
            pvgOfSubitemSelf |= 1;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        if (canEditSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += tagHtmlEdit;
            pvgOfSubitemSelf |= 2;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        if (canAddSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += tagHtmlAdd;
            pvgOfSubitemSelf |= 4;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        if (canDelSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += tagHtmlDelete;
            pvgOfSubitemSelf |= 8;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        Literal ltrPvgOfSubitemCrew = (Literal)e.Item.FindControl("ltrPvgOfSubitemCrew");
        HtmlInputHidden hidPvgOfSubitemCrew = (HtmlInputHidden)e.Item.FindControl("hidPvgOfSubitemCrew");
        int pvgOfSubitemCrew = 0;

        if (!canReadSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += tagHtmlNotAllowed;
        }

        if (canReadSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += tagHtmlRead;
            pvgOfSubitemCrew |= 1;
            hidPvgOfSubitemCrew.Value = pvgOfSubitemCrew.ToString();
        }

        if (canEditSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += tagHtmlEdit;
            pvgOfSubitemCrew |= 2;
            hidPvgOfSubitemCrew.Value = pvgOfSubitemCrew.ToString();
        }

        if (canDelSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += tagHtmlDelete;
            pvgOfSubitemCrew |= 8;
            hidPvgOfSubitemCrew.Value = pvgOfSubitemCrew.ToString();
        }

        Literal ltrPvgOfSubitemOthers = (Literal)e.Item.FindControl("ltrPvgOfSubitemOthers");
        HtmlInputHidden hidPvgOfSubitemOthers = (HtmlInputHidden)e.Item.FindControl("hidPvgOfSubitemOthers");
        int pvgOfSubitemOthers = 0;

        if (!canReadSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += tagHtmlNotAllowed;
        }

        if (canReadSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += tagHtmlRead;
            pvgOfSubitemOthers |= 1;
            hidPvgOfSubitemOthers.Value = pvgOfSubitemOthers.ToString();
        }

        if (canEditSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += tagHtmlEdit;
            pvgOfSubitemOthers |= 2;
            hidPvgOfSubitemOthers.Value = pvgOfSubitemOthers.ToString();
        }

        if (canDelSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += tagHtmlDelete;
            pvgOfSubitemOthers |= 8;
            hidPvgOfSubitemOthers.Value = pvgOfSubitemOthers.ToString();
        }

        Repeater rptSubOperations = (Repeater)e.Item.FindControl("rptSubOperations");

        if (rptSubOperations != null)
        {
            DataView dvSubList = drvTemp.CreateChildView("JoinTopSub");
            rptSubOperations.DataSource = dvSubList;
            rptSubOperations.DataBind();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Master.ShowErrorMsg("");

        if (!c.seRoleOpPvgs.Any(p => string.Compare(p.RoleName, ltrRoleName.Text) == 0))
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_ThereIsnotAnyChangesOfPvg);
            return;
        }

        //取得異動的權限
        List<RoleOpPvg> changes = c.seRoleOpPvgs.Where(p => string.Compare(p.RoleName, ltrRoleName.Text) == 0).ToList();
        c.ClearRoleDataOfRoleOpPvgs(ltrRoleName.Text);

        try
        {
            bool result = empAuth.SaveListOfEmployeeRolePrivileges(new RolePrivilegeParams()
            {
                RoleName = ltrRoleName.Text,
                pvgChanges = changes,
                PostAccount = c.GetEmpAccount()
            });

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Privilege"), true);
            }
            else
            {
                Master.ShowErrorMsg(Resources.Lang.ErrMsg_RolePrivilegeSaveFailed);
            }

            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = c.GetEmpAccount(),
                Description = string.Format("．{0}　．儲存權限/Save privileges　．身分/Role[{1}]　．異動數量/changes[{2}]　結果/result[{3}]", Title, ltrRoleName.Text, changes.Count, result),
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