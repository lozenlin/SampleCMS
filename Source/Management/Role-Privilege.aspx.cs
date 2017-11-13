using Common.LogicObject;
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

    private int tempLv1Seqno = 0;

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
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
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
        }
    }

    private void LoadTitle()
    {
        Title = string.Format("授權給身分 - id:{0}", c.qsRoleId);
    }

    private void DisplayOperations()
    {
        DataSet dsTopList = empAuth.GetOperationsTopListWithRoleAuth(ltrRoleName.Text);
        DataSet dsSubList = empAuth.GetOperationsSubListWithRoleAuth(ltrRoleName.Text);

        if (c.IsInRole("admin"))
        {
            //管理者可以看到全部
            foreach (DataRow dr in dsTopList.Tables[0].Rows)
                dr["CanRead"] = true;

            foreach (DataRow dr in dsSubList.Tables[0].Rows)
                dr["CanRead"] = true;
        }

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
        object objIconImageFile = drvTemp["IconImageFile"];
        if (!Convert.IsDBNull(objIconImageFile))
            imgOpItem.Src = string.Format("~/BPimages/icon/{0}", drvTemp.ToSafeStr("IconImageFile"));

        Literal ltrOpItemSubject = (Literal)e.Item.FindControl("ltrOpItemSubject");
        ltrOpItemSubject.Text = opSubject;

        string htmlNotAllowed = "<div><span class='badge badge-secondary pvg-badge' title='無權限'>無權限</span></div>";
        string htmlRead = "<div><span class='badge badge-warning text-white pvg-badge' title='可閱讀'>可閱讀</span></div>";
        string htmlEdit = "<div><span class='badge badge-success pvg-badge' title='可修改'>可修改</span></div>";
        string htmlAdd = "<div><span class='badge badge-info pvg-badge' title='可新增'>可新增</span></div>";
        string htmlDelete = "<div><span class='badge badge-danger pvg-badge' title='可刪除'>可刪除</span></div>";

        Literal ltrPvgOfItem = (Literal)e.Item.FindControl("ltrPvgOfItem");
        HtmlInputHidden hidPvgOfItem = (HtmlInputHidden)e.Item.FindControl("hidPvgOfItem");
        int pvgOfItem = 0;

        if (!canRead)
        {
            ltrPvgOfItem.Text += htmlNotAllowed;
        }

        if (canRead)
        {
            ltrPvgOfItem.Text += htmlRead;
            pvgOfItem |= 1;
            hidPvgOfItem.Value = pvgOfItem.ToString();
        }

        if (canEdit)
        {
            ltrPvgOfItem.Text += htmlEdit;
            pvgOfItem |= 2;
            hidPvgOfItem.Value = pvgOfItem.ToString();
        }

        Literal ltrPvgOfSubitemSelf = (Literal)e.Item.FindControl("ltrPvgOfSubitemSelf");
        HtmlInputHidden hidPvgOfSubitemSelf = (HtmlInputHidden)e.Item.FindControl("hidPvgOfSubitemSelf");
        int pvgOfSubitemSelf = 0;

        if (!canReadSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += htmlNotAllowed;
        }

        if (canReadSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += htmlRead;
            pvgOfSubitemSelf |= 1;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        if (canEditSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += htmlEdit;
            pvgOfSubitemSelf |= 2;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        if (canAddSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += htmlAdd;
            pvgOfSubitemSelf |= 4;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        if (canDelSubItemOfSelf)
        {
            ltrPvgOfSubitemSelf.Text += htmlDelete;
            pvgOfSubitemSelf |= 8;
            hidPvgOfSubitemSelf.Value = pvgOfSubitemSelf.ToString();
        }

        Literal ltrPvgOfSubitemCrew = (Literal)e.Item.FindControl("ltrPvgOfSubitemCrew");
        HtmlInputHidden hidPvgOfSubitemCrew = (HtmlInputHidden)e.Item.FindControl("hidPvgOfSubitemCrew");
        int pvgOfSubitemCrew = 0;

        if (!canReadSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += htmlNotAllowed;
        }

        if (canReadSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += htmlRead;
            pvgOfSubitemCrew |= 1;
            hidPvgOfSubitemCrew.Value = pvgOfSubitemCrew.ToString();
        }

        if (canEditSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += htmlEdit;
            pvgOfSubitemCrew |= 2;
            hidPvgOfSubitemCrew.Value = pvgOfSubitemCrew.ToString();
        }

        if (canDelSubItemOfCrew)
        {
            ltrPvgOfSubitemCrew.Text += htmlDelete;
            pvgOfSubitemCrew |= 8;
            hidPvgOfSubitemCrew.Value = pvgOfSubitemCrew.ToString();
        }

        Literal ltrPvgOfSubitemOthers = (Literal)e.Item.FindControl("ltrPvgOfSubitemOthers");
        HtmlInputHidden hidPvgOfSubitemOthers = (HtmlInputHidden)e.Item.FindControl("hidPvgOfSubitemOthers");
        int pvgOfSubitemOthers = 0;

        if (!canReadSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += htmlNotAllowed;
        }

        if (canReadSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += htmlRead;
            pvgOfSubitemOthers |= 1;
            hidPvgOfSubitemOthers.Value = pvgOfSubitemOthers.ToString();
        }

        if (canEditSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += htmlEdit;
            pvgOfSubitemOthers |= 2;
            hidPvgOfSubitemOthers.Value = pvgOfSubitemOthers.ToString();
        }

        if (canDelSubItemOfOthers)
        {
            ltrPvgOfSubitemOthers.Text += htmlDelete;
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

    }
}