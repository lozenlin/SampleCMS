﻿using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Operation_Node : BasePage
{
    protected OperationCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new OperationCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        c.SelectMenuItem(c.qsId.ToString(), "");

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfTopPage();

        hud = Master.GetHeadUpDisplay();
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!c.IsInRole("admin"))
            {
                Response.Redirect(c.BACK_END_HOME);
            }

            LoadUIData();
            DisplayOperation();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {
                    DisplayOperation();
                }

                Master.FlagValue = "";
            }
        }
    }

    private void LoadUIData()
    {
        //HUD
        hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
        hud.SetButtonAttribute(HudButtonNameEnum.AddNew, HudButtonAttributeEnum.JsInNavigateUrl,
            string.Format("popWin('Operation-Config.aspx?act={0}', 700, 600);", ConfigFormAction.add));

        if (c.qsId > 0)
        {
            hud.SetButtonVisible(HudButtonNameEnum.Edit, true);
            hud.SetButtonAttribute(HudButtonNameEnum.Edit, HudButtonAttributeEnum.JsInNavigateUrl,
                string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600);", ConfigFormAction.edit, c.qsId));
        }

        //conditions UI

        //condition vlaues

        //columns of list

        c.DisplySortableCols(new string[] { 
            "Subject", "IsNewWindow", "CommonClass", 
            "SortNo"
        });
    }

    private void DisplayOperation()
    {
        DisplaySubitems();
        DisplayProperties();
    }

    private void DisplaySubitems()
    {
        SubitemArea.Visible = true;

        OpListQueryParams opParams = new OpListQueryParams()
        {
            ParentId = c.qsId,
            CultureName = new LangManager().GetCultureName(c.seLangNoOfBackend.ToString()),
            Kw = ""
        };

        opParams.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 1,
            EndNum = 999999999,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        DataSet dsSubitems = empAuth.GetOperationList(opParams);

        if (dsSubitems != null)
        {
            rptSubitems.DataSource = dsSubitems.Tables[0];
            rptSubitems.DataBind();
        }
    }

    protected void rptSubitems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        int opId = Convert.ToInt32(drvTemp["opId"]);
        string subject = drvTemp.ToSafeStr("Subject");
        bool isNewWindow = Convert.ToBoolean(drvTemp["IsNewWindow"]);
        string iconImageFile = drvTemp.ToSafeStr("IconImageFile");
        bool isHideSelf = Convert.ToBoolean(drvTemp["IsHideSelf"]);
        string commonClass = drvTemp.ToSafeStr("CommonClass");

        HtmlTableRow ItemArea = (HtmlTableRow)e.Item.FindControl("ItemArea");

        if (isHideSelf)
        {
            ItemArea.Attributes["class"] = "table-danger";
        }

        LinkButton btnMoveDown = (LinkButton)e.Item.FindControl("btnMoveDown");
        LinkButton btnMoveUp = (LinkButton)e.Item.FindControl("btnMoveUp");
        int total = drvTemp.DataView.Count;
        int itemNum = e.Item.ItemIndex + 1;

        if (itemNum == 1)
        {
            btnMoveUp.Visible = false;
        }

        if (itemNum == total)
        {
            btnMoveDown.Visible = false;
        }

        if (c.qsSortField != "")
        {
            btnMoveUp.Visible = false;
            btnMoveDown.Visible = false;
        }

        HtmlAnchor btnItem = (HtmlAnchor)e.Item.FindControl("btnItem");

        btnItem.Title = subject;
        btnItem.HRef = string.Format("Operation-Node.aspx?id={0}", opId);

        HtmlImage imgItem = (HtmlImage)e.Item.FindControl("imgItem");

        if (iconImageFile != "")
        {
            imgItem.Src = "BPimages/icon/" + iconImageFile;
        }

        Literal ltrSubject = (Literal)e.Item.FindControl("ltrSubject");
        ltrSubject.Text = subject;

        Literal ltrIsNewWindow = (Literal)e.Item.FindControl("ltrIsNewWindow");
        ltrIsNewWindow.Text = isNewWindow ? "是" : "";

        Literal ltrCommonClass = (Literal)e.Item.FindControl("ltrCommonClass");
        ltrCommonClass.Text = string.IsNullOrEmpty(commonClass) ? "" : "已指定";

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600); return false;", ConfigFormAction.edit, opId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", opId.ToString(), subject);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + "確定刪除[{0}]?" + "');",
            subject);
    }

    protected void rptSubitems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        bool result = false;

        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                int opId = Convert.ToInt32(args[0]);
                string subject = args[1];
                OpParams param = new OpParams() { OpId = opId };

                result = empAuth.DeleteOperationData(param);

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除作業選項/Delete operation　．代碼/id[{0}]　標題/subject[{1}]　結果/result[{2}]", opId, subject, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "op-" + opId.ToString() + "-" + subject, result);

                if (result)
                {
                    Master.RefreshOpMenu();
                }
                else
                {
                    if (param.IsThereSubitemOfOp)
                        Master.ShowErrorMsg("此作業選項有子項目，不允許刪除");
                    else
                        Master.ShowErrorMsg("刪除作業選項失敗");
                }

                break;
            case "MoveUp":
                break;
            case "MoveDown":
                break;
        }

        if (result)
        {
            DisplaySubitems();
        }
    }

    private void DisplayProperties()
    {
        
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        Response.Redirect(c.BuildUrlOfListPage(c.qsId, sortField, isSortDesc));
    }
}