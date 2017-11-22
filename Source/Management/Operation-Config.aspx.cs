using Common.LogicObject;
using System;
using System.Collections.Generic;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}