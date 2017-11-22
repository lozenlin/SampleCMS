﻿using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Operation_Config : System.Web.UI.Page
{
    protected OperationCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new OperationCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfSubPages();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Authenticate
            if (!c.IsInRole("admin"))
            {
                string jsClose = "closeThisForm();";
                ClientScript.RegisterStartupScript(this.GetType(), "invalid", jsClose, true);
                return;
            }

            LoadUIData();
            DisplayOperationData();
            txtOpSubject.Focus();
        }

        LoadTitle();
    }

    private void LoadUIData()
    {
        LoadCommonClasses();
    }

    private void LoadCommonClasses()
    {
        ddlCommonClasses.Items.Clear();

        Assembly asmLogicObject = Assembly.LoadFrom(string.Format(@"{0}\Bin\Common.LogicObject.dll", Server.MapPath(Request.ApplicationPath)));

        // get exported types of dll
        Type[] exportedTypes = asmLogicObject.GetExportedTypes();

        if (exportedTypes == null)
            return;

        Array.Sort(exportedTypes, (x, y) => x.Name.CompareTo(y.Name));

        // initialize dropdownlist
        ddlCommonClasses.Items.Add(new ListItem("(請選擇)", ""));
        ddlCommonClasses.Items.Add(new ListItem("空白", ""));

        foreach (Type classType in exportedTypes)
        {
            if (classType.Namespace == "Common.LogicObject" && classType.Name.EndsWith("OfBackend"))
            {
                string text = classType.Name;
                string value = classType.Name;

                Attribute descAttr = classType.GetCustomAttribute(typeof(System.ComponentModel.DescriptionAttribute), false);
                
                if (descAttr != null)
                {
                    text = string.Format("{0} ({1})", value, ((System.ComponentModel.DescriptionAttribute)descAttr).Description);
                }

                ddlCommonClasses.Items.Add(new ListItem(text, value));
            }
        }
    }

    private void LoadTitle()
    {
        if (c.qsAct == ConfigFormAction.add)
            Title = string.Format("新增作業選項 - id:{0}", c.qsId);
        else if (c.qsAct == ConfigFormAction.edit)
            Title = string.Format("修改作業選項 - id:{0}", c.qsId);
    }

    private void DisplayOperationData()
    {
        if (c.qsAct == ConfigFormAction.edit)
        {
            DataSet dsOp = empAuth.GetOperationData(c.qsId);

            if (dsOp != null && dsOp.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsOp.Tables[0].Rows[0];

                txtSortNo.Text = drFirst.ToSafeStr("SortNo");
                txtOpSubject.Text = drFirst.ToSafeStr("OpSubject");
                txtEnglishSubject.Text = drFirst.ToSafeStr("EnglishSubject");
                txtIconImageFile.Text = drFirst.ToSafeStr("IconImageFile");
                txtLinkUrl.Text = drFirst.ToSafeStr("LinkUrl");
                chkIsNewWindow.Checked = Convert.ToBoolean(drFirst["IsNewWindow"]);
                chkIsHideSelf.Checked = Convert.ToBoolean(drFirst["IsHideSelf"]);
                txtCommonClass.Text = drFirst.ToSafeStr("CommonClass");

                if (txtCommonClass.Text != "")
                {
                    ddlCommonClasses.SelectedValue = txtCommonClass.Text;
                }

                btnSave.Visible = true;
            }
        }
        else if (c.qsAct == ConfigFormAction.add)
        {
            int newSortNo = empAuth.GetOperationMaxSortNo(c.qsId) + 10;
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
            Description = string.Format("．{0}　．儲存後端作業選項/Save operation[{1}][{2}]", Title, txtOpSubject.Text, txtEnglishSubject.Text),
            IP = c.GetClientIP()
        });

        try
        {
            txtOpSubject.Text = txtOpSubject.Text.Trim();
            txtEnglishSubject.Text = txtEnglishSubject.Text.Trim();
            txtIconImageFile.Text = txtIconImageFile.Text.Trim();
            txtLinkUrl.Text = txtLinkUrl.Text.Trim();
            txtCommonClass.Text = txtCommonClass.Text.Trim();

            OpParams param = new OpParams()
            {
                SortNo = Convert.ToInt32(txtSortNo.Text),
                OpSubject = txtOpSubject.Text,
                EnglishSubject = txtEnglishSubject.Text,
                IconImageFile = txtIconImageFile.Text,
                LinkUrl = txtLinkUrl.Text,
                IsNewWindow = chkIsNewWindow.Checked,
                IsHideSelf = chkIsHideSelf.Checked,
                CommonClass = txtCommonClass.Text,
                PostAccount = c.GetEmpAccount()
            };

            bool result = false;

            if (c.qsAct == ConfigFormAction.add)
            {
                param.ParentId = c.qsId;
                result = empAuth.InsertOperationData(param);

                if (!result)
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_AddFailed);
                }
            }
            else if (c.qsAct == ConfigFormAction.edit)
            {
                param.OpId = c.qsId;
                result = empAuth.UpdateOperaionData(param);

                if (!result)
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_UpdateFailed);
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