﻿using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Config : System.Web.UI.Page
{
    protected AccountCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new AccountCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfSubPages();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Authenticate
            if (c.qsAct == ConfigFormAction.edit && !(empAuth.CanEditThisPage() || c.IsMyAccount())
                || c.qsAct == ConfigFormAction.add && !empAuth.CanAddSubItemInThisPage())
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            LoadUIData();
            DisplayAccountData();
            txtEmpAccount.Focus();
        }
        else
        {
            if (txtPsw.Text.Trim() != "")
            {
                rfvPswConfirm.Enabled = true;
            }
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        if (c.seLangNoOfBackend != 1)
        {
            Master.EnableDatepickerTW = false;
        }

        LoadDeptUIData();
        LoadRolesUIData();
    }

    private void LoadDeptUIData()
    {
        ddlDept.Items.Clear();
        DataSet dsDept = empAuth.GetDepartmentListToSelect();

        if (dsDept != null)
        {
            ddlDept.DataTextField = "DeptName";
            ddlDept.DataValueField = "DeptId";
            ddlDept.DataSource = dsDept.Tables[0];
            ddlDept.DataBind();
        }
    }

    private void LoadRolesUIData()
    {
        ddlRoles.Items.Clear();
        DataSet dsRoles = empAuth.GetEmployeeRoleListToSelect();

        if (dsRoles != null)
        {
            ddlRoles.DataTextField = "DisplayText";
            ddlRoles.DataValueField = "RoleId";
            ddlRoles.DataSource = dsRoles.Tables[0];
            ddlRoles.DataBind();
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = "新增帳號";
        else if (c.qsAct == ConfigFormAction.edit)
            Title = "修改帳號 - Id:" + c.qsEmpId.ToString();
    }

    private void DisplayAccountData()
    {
        bool isOwner = false;
        int curRoleId = 0;

        if (c.qsAct == ConfigFormAction.edit)
        {
            DataSet dsAccount = empAuth.GetEmployeeData(c.qsEmpId);

            if (dsAccount != null && dsAccount.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsAccount.Tables[0].Rows[0];
                string empAccount = drFirst["EmpAccount"].ToString();

                //account
                txtEmpAccount.Text = drFirst["EmpAccount"].ToString();
                txtEmpAccount.Enabled = false;

                //name
                txtEmpName.Text = drFirst["EmpName"].ToString();

                //password
                rfvPsw.Enabled = false;
                hidEmpPasswordOri.Text = drFirst["EmpPassword"].ToString();
                hidPasswordHashed.Text = drFirst["PasswordHashed"].ToString();
                hidDefaultRandomPassword.Text = drFirst["DefaultRandomPassword"].ToString();

                //email
                txtEmail.Text = drFirst["Email"].ToString();

                //remarks
                txtRemarks.Text = drFirst["Remarks"].ToString();

                // is access denied
                chkIsAccessDenied.Checked = Convert.ToBoolean(drFirst["IsAccessDenied"]);
                ltrIsAccessDenied.Text = chkIsAccessDenied.Checked ? "已停權" : "未勾選";

                //valid date
                txtStartDate.Text = string.Format("{0:yyyy-MM-dd}", drFirst["StartDate"]);
                txtEndDate.Text = string.Format("{0:yyyy-MM-dd}", drFirst["EndDate"]);
                ltrDateRange.Text = txtStartDate.Text + " ~ " + txtEndDate.Text;

                if (empAccount == "admin")
                {
                    DateRangeArea.Visible = false;
                }

                //department
                ddlDept.SelectedValue = drFirst["DeptId"].ToString();
                if (ddlDept.SelectedItem != null)
                    ltrDept.Text = ddlDept.SelectedItem.Text;

                //role
                curRoleId = Convert.ToInt32(drFirst["RoleId"]);
                ddlRoles.SelectedValue = curRoleId.ToString();
                ltrRoles.Text = drFirst["RoleDisplayText"].ToString();

                //owner
                txtOwnerAccount.Text = drFirst["OwnerAccount"].ToString();
                ltrOwnerAccount.Text = txtOwnerAccount.Text;

                isOwner = empAuth.CanEditThisPage(false, drFirst["OwnerAccount"].ToString(), Convert.ToInt32(drFirst["OwnerDeptId"]));
                btnSave.Visible = true; 
            }
        }
        else
        {
            //add

            txtStartDate.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Today);
            DateTime endDate = DateTime.Today.AddYears(10);
            txtEndDate.Text = string.Format("{0:yyyy-MM-dd}", endDate);

            txtOwnerAccount.Text = c.GetEmpAccount();
            ltrOwnerAccount.Text = txtOwnerAccount.Text;

            isOwner = true;
            btnSave.Visible = true;
        }

        // owner privilege
        if (isOwner)
        {
            chkIsAccessDenied.Visible = true;
            ltrIsAccessDenied.Visible = false;

            DateRangeEditCtrl.Visible = true;
            ltrDateRange.Visible = false;

            ddlDept.Visible = true;
            ltrDept.Visible = false;

            ddlRoles.Visible = true;
            ltrRoles.Visible = false;
        }

        // role-admin privilege
        if (c.IsInRole("admin"))
        {
            //owner
            txtOwnerAccount.Visible = true;
            ltrOwnerAccount.Visible = false;
        }
        else
        {
            // only role-admin can assigns role-admin to another (但是,保留已經是role-admin的選項)
            if (curRoleId != 1)
            {
                ListItem liAdmin = ddlRoles.Items.FindByValue("1");
                if (liAdmin != null)
                {
                    ddlRoles.Items.Remove(liAdmin);
                }
            }
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
            Description = string.Format("．{0}　．儲存帳號[{0}]", Title, txtEmpAccount.Text),
            IP = c.GetClientIP()
        });

        try
        {
            txtEmpAccount.Text = txtEmpAccount.Text.Trim();
            txtPsw.Text = txtPsw.Text.Trim();
            string passwordValue = hidEmpPasswordOri.Text;
            bool passwordHashed = Convert.ToBoolean(hidPasswordHashed.Text);

            if (txtPsw.Text != "")
            {
                passwordValue = HashUtility.GetPasswordHash(txtPsw.Text);
                passwordHashed = true;
            }

            txtEmpName.Text = txtEmpName.Text.Trim();
            txtEmail.Text = txtEmail.Text.Trim();
            txtRemarks.Text = txtRemarks.Text.Trim();
            txtOwnerAccount.Text = txtOwnerAccount.Text.Trim();

            AccountParams param = new AccountParams()
            {
                EmpAccount = txtEmpAccount.Text,
                EmpPassword = passwordValue,
                EmpName = txtEmpName.Text,
                Email = txtEmail.Text,
                Remarks = txtRemarks.Text,
                DeptId = Convert.ToInt32(ddlDept.SelectedValue),
                RoleId = Convert.ToInt32(ddlRoles.SelectedValue),
                IsAccessDenied = chkIsAccessDenied.Checked,
                StartDate = Convert.ToDateTime(txtStartDate.Text),
                EndDate = Convert.ToDateTime(txtEndDate.Text),
                OwnerAccount = txtOwnerAccount.Text,
                PasswordHashed = passwordHashed,
                DefaultRandomPassword = hidDefaultRandomPassword.Text,
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {
                result = empAuth.InsertEmployeeData(param);

                if(!result)
                {
                    if (param.HasAccountBeenUsed)
                    {
                        Master.ShowErrorMsg("帳號已被使用，請更換");
                    }
                    else
                    {
                        Master.ShowErrorMsg("新增失敗");
                    }
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.EmpId = c.qsEmpId;
                result = empAuth.UpdateEmployeeData(param);

                if (!result)
                {
                    Master.ShowErrorMsg("更新失敗");
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