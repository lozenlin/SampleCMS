using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Department_Config : System.Web.UI.Page
{
    protected DepartmentCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new DepartmentCommonOfBackend(this.Context, this.ViewState);
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
            DisplayDepartmentData();
            txtDeptName.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        rfvSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;
        covSortNo.ErrorMessage = "*" + Resources.Lang.ErrMsg_IntegerOnly;
        rfvDeptName.ErrorMessage = "*" + Resources.Lang.ErrMsg_Required;

    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = Resources.Lang.Dept_Title_AddNew;
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format(Resources.Lang.Dept_Title_Edit_Format, c.qsId);
    }

    private void DisplayDepartmentData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            DataSet dsDept = empAuth.GetDepartmentData(c.qsId);

            if (dsDept != null && dsDept.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsDept.Tables[0].Rows[0];

                txtSortNo.Text = drFirst.ToSafeStr("SortNo");
                txtDeptName.Text = drFirst.ToSafeStr("DeptName");

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
        else if (c.qsAct == ConfigFormAction.add)
        {
            int newSortNo = empAuth.GetDepartmentMaxSortNo() + 10;
            txtSortNo.Text = newSortNo.ToString();

            btnSave.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;

        try
        {
            txtDeptName.Text = txtDeptName.Text.Trim();

            DeptParams param = new DeptParams()
            {
                DeptName = txtDeptName.Text,
                SortNo = Convert.ToInt32(txtSortNo.Text),
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {

                result = empAuth.InsertDepartmentData(param);

                if (!result)
                {
                    if (param.HasDeptNameBeenUsed)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeptNameHasBeenUsed);
                    }
                    else
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                    }
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.DeptId = c.qsId;
                result = empAuth.UpdateDepartmentData(param);

                if (!result)
                {
                    if (param.HasDeptNameBeenUsed)
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeptNameHasBeenUsed);
                    }
                    else
                    {
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateFailed);
                    }
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
                Description = string.Format("．{0}　．儲存部門/Save department[{1}]　DeptId[{2}]　結果/result[{3}]", Title, txtDeptName.Text, param.DeptId, result),
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