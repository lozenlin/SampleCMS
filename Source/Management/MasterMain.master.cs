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

public partial class MasterMain : System.Web.UI.MasterPage
{
    protected BackendPageCommon c;
    protected EmployeeAuthorityLogic empAuth;

    #region Public properties

    public string FlagValue
    {
        get { return txtFlag.Value; }
        set { txtFlag.Value = value; }
    }

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        c = new BackendPageCommon(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic(c);

        Page.Title = Resources.Lang.BackStageName;
        Page.MaintainScrollPositionOnPostBack = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (c.seLoginEmpData.EmpAccount == null)
        {
            ShowErrorMsg(Resources.Lang.ErrMsg_LostSessionState);
            c.LogOutWhenSessionMissed(Resources.Lang.ErrMsg_LostSessionState);
        }
        
        if (!IsPostBack)
        {
            LoadUIData();
        }

        DisplayOpMenu();
    }

    private void LoadUIData()
    {
        if (c.seLoginEmpData.EmpAccount != null)
        {
            LoginEmployeeData d = c.seLoginEmpData;
            ltrRoleDisplayName.Text = string.Format("{0}({1})", d.RoleDisplayName, d.RoleName);
            ltrDeptName.Text = d.DeptName;
            ltrAccountInfo.Text = string.Format("Hi, {0}({1})", d.EmpName, d.EmpAccount);
            btnAccountSettings.Title = Resources.Lang.Main_btnAccountSettings;
            btnAccountSettings.HRef = "Account-List.aspx";
            btnLogout.Title = Resources.Lang.Main_btnLogout;
            btnLogout.HRef = "Logout.ashx";

            btnEditOperations.Title = Resources.Lang.btnEditOperations_Hint;
        }

        //只有管理者能編輯後端作業選項
        btnEditOperations.Visible = c.IsInRole("admin");
        LineOfCtrl.Visible = btnEditOperations.Visible;
    }

    private void DisplayOpMenu()
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

        rptOpMenu.DataSource = dsTopList.Tables[0];
        rptOpMenu.DataBind();
    }

    protected void rptOpMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int opId = Convert.ToInt32(drvTemp["OpId"]);
        string opSubject = drvTemp["OpSubject"].ToString();
        bool isNewWindow = Convert.ToBoolean(drvTemp["IsNewWindow"]);
        string encodedUrl = drvTemp["LinkUrl"].ToString();
        string linkUrl = c.DecodeUrlOfMenu(encodedUrl);

        HtmlGenericControl OpHeaderArea = (HtmlGenericControl)e.Item.FindControl("OpHeaderArea");
        OpHeaderArea.Attributes.Add("opId", opId.ToString());

        HtmlAnchor btnOpHeader = (HtmlAnchor)e.Item.FindControl("btnOpHeader");
        btnOpHeader.Title = opSubject;

        if (isNewWindow)
        {
            btnOpHeader.Target = "_blank";
            btnOpHeader.Title += "(另開新視窗)";
        }

        if (linkUrl != "")
        {
            if (linkUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                    || linkUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                //外部網址
                if (!isNewWindow)
                    linkUrl = "~/Embedded-Content.aspx?url=" + Server.UrlEncode(encodedUrl);
            }
            else
            {
                linkUrl = "~/" + linkUrl;
            }

            btnOpHeader.HRef = linkUrl;
        }

        HtmlImage imgOpHeader = (HtmlImage)e.Item.FindControl("imgOpHeader");
        imgOpHeader.Alt = opSubject;
        imgOpHeader.Src = "~/BPimages/icon/data.gif";
        object objIconImageFile = drvTemp["IconImageFile"];
        if (!Convert.IsDBNull(objIconImageFile))
            imgOpHeader.Src = string.Format("~/BPimages/icon/{0}", objIconImageFile);

        Literal ltrOpHeaderSubject = (Literal)e.Item.FindControl("ltrOpHeaderSubject");
        ltrOpHeaderSubject.Text = opSubject;

        //檢查授權
        bool canRead = false;

        if (!Convert.IsDBNull(drvTemp["CanRead"]))
            canRead = Convert.ToBoolean(drvTemp["CanRead"]);

        OpHeaderArea.Visible = canRead;

        DataView dvSubList = drvTemp.CreateChildView("JoinTopSub");
        Repeater rptOpItems = (Repeater)e.Item.FindControl("rptOpItems");
        rptOpItems.DataSource = dvSubList;
        rptOpItems.DataBind();
    }

    protected void rptOpItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int opId = Convert.ToInt32(drvTemp["OpId"]);
        string opSubject = drvTemp["OpSubject"].ToString();
        bool isNewWindow = Convert.ToBoolean(drvTemp["IsNewWindow"]);
        string encodedUrl = drvTemp["LinkUrl"].ToString();
        string linkUrl = c.DecodeUrlOfMenu(encodedUrl);

        HtmlGenericControl OpItemArea = (HtmlGenericControl)e.Item.FindControl("OpItemArea");
        OpItemArea.Attributes.Add("opId", opId.ToString());

        HtmlAnchor btnOpItem = (HtmlAnchor)e.Item.FindControl("btnOpItem");
        btnOpItem.Title = opSubject;

        if (isNewWindow)
        {
            btnOpItem.Target = "_blank";
            btnOpItem.Title += "(另開新視窗)";
        }

        if (linkUrl != "")
        {
            if (linkUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                    || linkUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                //外部網址
                if (!isNewWindow)
                    linkUrl = "~/Embedded-Content.aspx?url=" + Server.UrlEncode(encodedUrl);
            }
            else
            {
                linkUrl = "~/" + linkUrl;
            }

            btnOpItem.HRef = linkUrl;
        }

        HtmlImage imgOpItem = (HtmlImage)e.Item.FindControl("imgOpItem");
        imgOpItem.Alt = opSubject;
        imgOpItem.Src = "~/BPimages/icon/data.gif";
        object objIconImageFile = drvTemp["IconImageFile"];
        if (!Convert.IsDBNull(objIconImageFile))
            imgOpItem.Src = string.Format("~/BPimages/icon/{0}", objIconImageFile);

        Literal ltrOpItemSubject = (Literal)e.Item.FindControl("ltrOpItemSubject");
        ltrOpItemSubject.Text = opSubject;

        //檢查授權
        bool canRead = false;

        if (!Convert.IsDBNull(drvTemp["CanRead"]))
            canRead = Convert.ToBoolean(drvTemp["CanRead"]);

        OpItemArea.Visible = canRead;
    }

    #region Public Methods

    public void SetHeadUpDisplayVisible(bool visible)
    {
        ucHeadUpDisplay.Visible = visible;
    }

    public IHeadUpDisplay GetHeadUpDisplay()
    {
        return ucHeadUpDisplay;
    }

    /// <summary>
    /// 顯示錯誤訊息
    /// </summary>
    public void ShowErrorMsg(string value)
    {
        ltrErrMsg.Text = value;
        ErrorMsgArea.Visible = (value != "");

        if (ErrorMsgArea.Visible)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorMsg", "smoothUp();", true);
        }
    }

    #endregion
}
