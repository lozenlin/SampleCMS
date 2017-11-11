using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Role_Privilege : System.Web.UI.Page
{
    protected RoleCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

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
        DataSet dsTopList = empAuth.GetOperationsTopListWithRoleAuth(c.GetRoleName());
        DataSet dsSubList = empAuth.GetOperationsSubListWithRoleAuth(c.GetRoleName());

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

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}