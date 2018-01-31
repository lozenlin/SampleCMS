using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Config : System.Web.UI.Page
{
    protected AccountCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    /// <summary>
    /// 使用嚴格的密碼規則
    /// </summary>
    private bool useStrictPasswordRule = false;

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
        rfvEmpAccount.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvEmpName.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        rfvPsw.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        revPsw.ErrorMessage = "*" + Resources.Lang.ErrMsg_PswNotQualified;
        cuvPsw.ErrorMessage = "*" + Resources.Lang.ErrMsg_PswNotQualified;
        ltrPswComment.Text = "(" + Resources.Lang.Account_PswComment + ")";
        btnGenPsw.ToolTip = Resources.Lang.Account_btnGenPsw_Hint;
        rfvPswConfirm.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covPswConfirm.ErrorMessage = "*" + Resources.Lang.ErrMsg_PswConfirmNotMatch;
        rfvEmail.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        revEmail.ErrorMessage = "*" + Resources.Lang.ErrMsg_WrongFormat;
        chkIsAccessDenied.Text = Resources.Lang.Account_chkIsAccessDenied;
        rfvStartDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covStartDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_InvalidDate;
        rfvEndDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covEndDate.ErrorMessage = "*" + Resources.Lang.ErrMsg_InvalidDate;

        if (c.seLangNoOfBackend != 1)
        {
            Master.EnableDatepickerTW = false;
        }

        InitPasswordRule();
        LoadDeptUIData();
        LoadRolesUIData();
    }

    private void InitPasswordRule()
    {
        if (useStrictPasswordRule)
        {
            cuvPsw.Enabled = true;
            PswRuleNotice.InnerHtml = Resources.Lang.PswRuleNotice_StrictRule;
        }
        else
        {
            revPsw.Enabled = true;
            revPsw.ValidationExpression = StringUtility.GetPswSimpleRuleValidationExpression();
            PswRuleNotice.InnerHtml = Resources.Lang.PswRuleNotice_LooseRule;
        }
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

            // move admin to last
            ListItem liAdmin = ddlRoles.Items.FindByValue("1");
            if (liAdmin != null)
            {
                ddlRoles.Items.Remove(liAdmin);
                ddlRoles.Items.Add(liAdmin);
            }
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = Resources.Lang.Account_Title_AddNew;
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.Account_Title_Edit_Format, c.qsEmpId);
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
                string empAccount = drFirst.ToSafeStr("EmpAccount");

                //account
                txtEmpAccount.Text = drFirst.ToSafeStr("EmpAccount");
                txtEmpAccount.Enabled = false;

                //name
                txtEmpName.Text = drFirst.ToSafeStr("EmpName");

                //password
                rfvPsw.Enabled = false;
                hidEmpPasswordOri.Text = drFirst.ToSafeStr("EmpPassword");
                hidPasswordHashed.Text = drFirst.ToSafeStr("PasswordHashed");
                hidDefaultRandomPassword.Text = drFirst.ToSafeStr("DefaultRandomPassword");

                //email
                txtEmail.Text = drFirst.ToSafeStr("Email");

                //remarks
                txtRemarks.Text = drFirst.ToSafeStr("Remarks");

                // is access denied
                chkIsAccessDenied.Checked = Convert.ToBoolean(drFirst["IsAccessDenied"]);
                ltrIsAccessDenied.Text = chkIsAccessDenied.Checked ? Resources.Lang.Account_IsAccessDenied_Checked : Resources.Lang.Account_IsAccessDenied_Unchecked;

                //valid date
                txtStartDate.Text = string.Format("{0:yyyy-MM-dd}", drFirst["StartDate"]);
                txtEndDate.Text = string.Format("{0:yyyy-MM-dd}", drFirst["EndDate"]);
                ltrDateRange.Text = txtStartDate.Text + " ~ " + txtEndDate.Text;

                if (empAccount == "admin")
                {
                    DateRangeArea.Visible = false;
                }

                //department
                ddlDept.SelectedValue = drFirst.ToSafeStr("DeptId");
                if (ddlDept.SelectedItem != null)
                    ltrDept.Text = ddlDept.SelectedItem.Text;

                //role
                curRoleId = Convert.ToInt32(drFirst["RoleId"]);
                ddlRoles.SelectedValue = curRoleId.ToString();
                ltrRoles.Text = drFirst.ToSafeStr("RoleDisplayText");

                //owner
                txtOwnerAccount.Text = drFirst.ToSafeStr("OwnerAccount");
                ltrOwnerAccount.Text = txtOwnerAccount.Text;

                isOwner = empAuth.CanEditThisPage(false, drFirst.ToSafeStr("OwnerAccount"), Convert.ToInt32(drFirst["OwnerDeptId"]));

                //modification info
                ltrPostAccount.Text = drFirst.ToSafeStr("PostAccount");
                ltrPostDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", drFirst["PostDate"]);

                if (!Convert.IsDBNull(drFirst["MdfDate"]))
                {
                    ltrMdfAccount.Text = drFirst.ToSafeStr("MdfAccount");
                    ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", drFirst["MdfDate"]);
                }

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
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AccountHasBeenUsed);
                    }
                    else
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                    }
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.EmpId = c.qsEmpId;
                result = empAuth.UpdateEmployeeData(param);

                if (!result)
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateFailed);
                }
            }

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", StringUtility.GetNoticeOpenerJs("Config"), true);
            }

            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = c.GetEmpAccount(),
                Description = string.Format("．{0}　．儲存帳號/Save account[{1}]　EmpId[{2}]　結果/result[{3}]", Title, txtEmpAccount.Text, param.EmpId, result),
                IP = c.GetClientIP()
            });
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            Master.ShowErrorMsg(ex.Message);
        }
    }

    protected void cuvPsw_ServerValidate(object source, ServerValidateEventArgs args)
    {
        bool isValidPsw = true;

        //內文至少包含下列種類

        //特殊符號
        if (!Regex.IsMatch(args.Value, @"[~`!@#$%^&*()\-_=+,\.<>;':""\[\]{}\\|?/]{1,1}"))
        {
            isValidPsw = false;
        }

        //英文字母大寫
        if (!Regex.IsMatch(args.Value, @"[A-Z]+"))
        {
            isValidPsw = false;
        }

        //及小寫
        if (!Regex.IsMatch(args.Value, @"[a-z]+"))
        {
            isValidPsw = false;
        }

        //數字
        if (!Regex.IsMatch(args.Value, @"[0-9]+"))
        {
            isValidPsw = false;
        }

        //最少12字
        if (args.Value.Length < 12)
        {
            isValidPsw = false;
        }

        args.IsValid = isValidPsw;
    }

    protected void btnGenPsw_Click(object sender, EventArgs e)
    {
        txtPsw.TextMode = TextBoxMode.SingleLine;
        txtPswConfirm.TextMode = TextBoxMode.SingleLine;

        if (useStrictPasswordRule)
            txtPsw.Text = StringUtility.GenerateStrictPasswordValue(12);
        else
            txtPsw.Text = StringUtility.GenerateLoosePasswordValue(10);

        txtPswConfirm.Text = txtPsw.Text;
        txtPsw.ReadOnly = true;
        PswConfirmArea.Visible = false;

        ////密碼暫時另存一份在備註
        //string remarkWoOldPsw = Regex.Replace(txtRemarks.Text, Resources.Lang.Account_lblDefaultPsw + @"[!@#$%^&*0-9A-Za-z]+", "");
        //txtRemarks.Text = Resources.Lang.Account_lblDefaultPsw + txtPsw.Text + remarkWoOldPsw;

        hidDefaultRandomPassword.Text = txtPsw.Text;
    }
}