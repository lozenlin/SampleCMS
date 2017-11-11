using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Role_Config : System.Web.UI.Page
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
            if (c.qsAct == ConfigFormAction.edit && !empAuth.CanEditThisPage()
                || c.qsAct == ConfigFormAction.add && !empAuth.CanAddSubItemInThisPage())
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            LoadUIData();
            DisplayRoleData();
            txtRoleName.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        LoadCopyPrivilegeFromUIData();
    }

    private void LoadCopyPrivilegeFromUIData()
    {
        ddlCopyPrivilegeFrom.Items.Clear();
        DataSet dsRoles = empAuth.GetEmployeeRoleListToSelect();

        if (dsRoles != null)
        {
            // remove admin
            DataView dvRoles = new DataView(dsRoles.Tables[0]);
            dvRoles.RowFilter = "RoleName<>'admin'";

            ddlCopyPrivilegeFrom.DataTextField = "DisplayText";
            ddlCopyPrivilegeFrom.DataValueField = "RoleName";
            ddlCopyPrivilegeFrom.DataSource = dvRoles;
            ddlCopyPrivilegeFrom.DataBind();
        }

        ddlCopyPrivilegeFrom.Items.Insert(0, new ListItem("(不複製)", ""));
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = "新增身分";
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改身分 - id:{0}", c.qsRoleId);
    }

    private void DisplayRoleData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            DataSet dsRole = empAuth.GetEmployeeRoleData(c.qsRoleId);

            if (dsRole != null && dsRole.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsRole.Tables[0].Rows[0];

                txtSortNo.Text = drFirst.ToSafeStr("SortNo");
                txtRoleName.Text = drFirst.ToSafeStr("RoleName");
                txtRoleName.Enabled = false;
                txtRoleDisplayName.Text = drFirst.ToSafeStr("RoleDisplayName");
                CopyPrivilegeArea.Visible = false;

                btnSave.Visible = true;
            }
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            int newSortNo = empAuth.GetEmployeeRoleMaxSortNo() + 10;
            txtSortNo.Text = newSortNo.ToString();

            btnSave.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;

        //新增後端操作記錄
        empAuth.InsertBackEndLogData(new BackEndLogData()
        {
            EmpAccount = c.GetEmpAccount(),
            Description = string.Format("．{0}　．儲存身分/Save role[{1}]", Title, txtRoleName.Text),
            IP = c.GetClientIP()
        });

        try
        {
            txtRoleName.Text = txtRoleName.Text.Trim();
            txtRoleDisplayName.Text = txtRoleDisplayName.Text.Trim();

            RoleParams param = new RoleParams()
            {
                RoleName = txtRoleName.Text,
                RoleDisplayName = txtRoleDisplayName.Text,
                SortNo = Convert.ToInt32(txtSortNo.Text.Trim()),
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {
                if (ddlCopyPrivilegeFrom.SelectedIndex > 0)
                    param.CopyPrivilegeFromRoleName = ddlCopyPrivilegeFrom.SelectedValue;

                result = empAuth.InsertEmployeeRoleData(param);

                if (!result)
                {
                    if (param.HasRoleBeenUsed)
                    {
                        Master.ShowErrorMsg("身分名稱已存在");
                    }
                    else
                    {
                        Master.ShowErrorMsg("新增身分失敗");
                    }
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.RoleId = c.qsRoleId;
                result = empAuth.UpdateEmployeeRoleData(param);

                if (!result)
                {
                    Master.ShowErrorMsg("更新身分失敗");
                }
            }

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Config"), true);
            }
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            Master.ShowErrorMsg(ex.Message);
        }
    }
}