using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Operation_Node : BasePage
{
    protected OperationCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    private bool useEnglishSubject = false;
    private int levelNumOfThis = 0;
    private int maxLevelNum = 2;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new OperationCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        c.SelectMenuItem(c.qsId.ToString(), "");

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.InitialAuthorizationResultOfTopPage();

        hud = Master.GetHeadUpDisplay();
        isBackendPage = true;

        if (c.seCultureNameOfBackend == "en")
        {
            useEnglishSubject = true;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RebuildBreadcrumbAndHeadOfHUD();
        Title = hud.GetHeadText() + " - " + Title;

        if (!IsPostBack)
        {
            if (!(c.IsInRole("admin") || c.IsInRole("guest")))
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
                    Master.RefreshOpMenu();
                }

                Master.FlagValue = "";
            }
        }
    }

    private void RebuildBreadcrumbAndHeadOfHUD()
    {
        string pageName = Resources.Lang.PageName_Operations;
        string pageUrl = "Operation-Node.aspx";

        if (c.qsId == 0)
        {
            //root
            hud.RebuildBreadcrumb(pageName, false);
            hud.SetHeadText(pageName);
        }
        else
        {
            StringBuilder sbBreadcrumbWoHome = new StringBuilder(100);

            // add root link
            sbBreadcrumbWoHome.Append(hud.GetBreadcrumbLinkItemHtml(pageName, pageName, pageUrl));
            // set url of BackToParent button
            hud.SetButtonAttribute(HudButtonNameEnum.BackToParent, HudButtonAttributeEnum.NavigateUrl, "~/" + pageUrl);

            DataSet dsLevelInfo = empAuth.GetOperationLevelInfo(c.qsId);

            if (dsLevelInfo != null && dsLevelInfo.Tables[0].Rows.Count > 0)
            {
                int total = dsLevelInfo.Tables[0].Rows.Count;

                for (int itemNum = total; itemNum >= 1; itemNum--)
                {
                    DataRow drOp = dsLevelInfo.Tables[0].Rows[itemNum - 1];
                    string opSubject = drOp.ToSafeStr("OpSubject");
                    string englishSubject = drOp.ToSafeStr("EnglishSubject");
                    int opId = Convert.ToInt32(drOp["OpId"]);
                    string url = string.Format("{0}?id={1}", pageUrl, opId);
                    int levelNum = Convert.ToInt32(drOp["LevelNum"]);
                    string iconImageFile = drOp.ToSafeStr("IconImageFile");

                    if (useEnglishSubject && !string.IsNullOrEmpty(englishSubject))
                    {
                        opSubject = englishSubject;
                    }

                    if (itemNum == 1)
                    {
                        levelNumOfThis = levelNum;
                        sbBreadcrumbWoHome.Append(hud.GetBreadcrumbTextItemHtml(opSubject));
                        // update head of HUD
                        hud.SetHeadText(opSubject);

                        if (!string.IsNullOrEmpty(iconImageFile))
                        {
                            iconImageFile = "~/BPImages/icon/" + iconImageFile;
                            hud.SetHeadIconImageUrl(iconImageFile);
                        }
                    }
                    else
                    {
                        sbBreadcrumbWoHome.Append(hud.GetBreadcrumbLinkItemHtml(opSubject, opSubject, url));

                        if (itemNum == 2)
                        {
                            // set url of BackToParent button
                            hud.SetButtonAttribute(HudButtonNameEnum.BackToParent, HudButtonAttributeEnum.NavigateUrl, "~/" + url);
                        }
                    }
                }
            }

            hud.RebuildBreadcrumb(sbBreadcrumbWoHome.ToString(), true);
        }
    }

    private void LoadUIData()
    {
        //HUD
        if (c.IsInRole("admin"))
        {
            if (levelNumOfThis < maxLevelNum)
            {
                hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
                hud.SetButtonAttribute(HudButtonNameEnum.AddNew, HudButtonAttributeEnum.JsInNavigateUrl,
                    string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600);", ConfigFormAction.add, c.qsId));
            }

            if (c.qsId > 0)
            {
                hud.SetButtonVisible(HudButtonNameEnum.Edit, true);
                hud.SetButtonAttribute(HudButtonNameEnum.Edit, HudButtonAttributeEnum.JsInNavigateUrl,
                    string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600);", ConfigFormAction.edit, c.qsId));
            }
        }

        //conditions UI

        //condition vlaues

        //columns of list
        btnSortSubject.Text = Resources.Lang.Col_Subject;
        hidSortSubject.Text = btnSortSubject.Text;
        btnSortIsNewWindow.Text = Resources.Lang.Col_OpenInNewWindow;
        hidSortIsNewWindow.Text = btnSortIsNewWindow.Text;
        btnSortCommonClass.Text = Resources.Lang.Col_CommonClass;
        hidSortCommonClass.Text = btnSortCommonClass.Text;
        btnSortSortNo.Text = Resources.Lang.Col_SortNo;
        hidSortSortNo.Text = btnSortSortNo.Text;

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
        if (levelNumOfThis >= maxLevelNum)
        {
            return;
        }

        SubitemArea.Visible = true;

        OpListQueryParams param = new OpListQueryParams()
        {
            ParentId = c.qsId,
            CultureName = c.seCultureNameOfBackend,
            Kw = ""
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 1,
            EndNum = 999999999,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        DataSet dsSubitems = empAuth.GetOperationList(param);

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
        btnMoveDown.ToolTip = Resources.Lang.btnMoveDown;

        LinkButton btnMoveUp = (LinkButton)e.Item.FindControl("btnMoveUp");
        btnMoveUp.ToolTip = Resources.Lang.btnMoveUp;

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
        ltrIsNewWindow.Text = isNewWindow ? Resources.Lang.IsNewWindow_Yes : Resources.Lang.IsNewWindow_No;

        Literal ltrCommonClass = (Literal)e.Item.FindControl("ltrCommonClass");
        string commonClassBadge = string.Format("<span class='badge badge-secondary' title='{0}'>{1}</span>", commonClass, Resources.Lang.CommonClass_HasValue);
        ltrCommonClass.Text = string.IsNullOrEmpty(commonClass) ? "" : commonClassBadge;

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Operation-Config.aspx?act={0}&id={1}', 700, 600); return false;", ConfigFormAction.edit, opId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", opId.ToString(), subject);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Operation_ConfirmDelete_Format + "');",
            subject);

        if (!c.IsInRole("admin"))
        {
            btnMoveDown.Visible = false;
            btnMoveUp.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }
    }

    protected void rptSubitems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        bool result = false;
        int opId = 0;

        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                opId = Convert.ToInt32(args[0]);
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

                if (!result)
                {
                    if (param.IsThereSubitemOfOp)
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_ThereIsSubitemofOp);
                    else
                        Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeleteOperationFailed);
                }

                break;
            case "MoveUp":
                opId = Convert.ToInt32(e.CommandArgument);
                result = empAuth.DecreaseOperationSortNo(opId, c.GetEmpAccount());
                break;
            case "MoveDown":
                opId = Convert.ToInt32(e.CommandArgument);
                result = empAuth.IncreaseOperationSortNo(opId, c.GetEmpAccount());
                break;
        }

        if (result)
        {
            DisplaySubitems();
            Master.RefreshOpMenu();
        }
    }

    private void DisplayProperties()
    {
        if (c.qsId == 0)
        {
            return;
        }

        PropertyArea.Visible = true;

        if (levelNumOfThis >= maxLevelNum)
        {
            PropertyDivider.Visible = false;
        }

        DataSet dsOp = empAuth.GetOperationData(c.qsId);

        if (dsOp != null && dsOp.Tables[0].Rows.Count > 0)
        {
            DataRow drFirst = dsOp.Tables[0].Rows[0];

            string iconImageFile = drFirst.ToSafeStr("IconImageFile");

            if (iconImageFile != "")
            {
                imgIcon.Src = "BPimages/icon/" + iconImageFile;
            }

            ltrLinkUrl.Text = drFirst.ToSafeStr("LinkUrl");

            bool isNewWindow = Convert.ToBoolean(drFirst["IsNewWindow"]);
            ltrIsNewWindow.Text = isNewWindow ? Resources.Lang.IsNewWindow_Yes : Resources.Lang.IsNewWindow_No;

            bool isHideSelf = Convert.ToBoolean(drFirst["IsHideSelf"]);
            ltrIsHideSelf.Text = isHideSelf ? Resources.Lang.IsHideSelf_Hide : Resources.Lang.IsHideSelf_Show;

            ltrCommonClass.Text = drFirst.ToSafeStr("CommonClass");

            string mdfAccount = drFirst.ToSafeStr("mdfAccount");
            DateTime mdfDate;

            if (Convert.IsDBNull(drFirst["MdfDate"]))
            {
                mdfAccount = drFirst.ToSafeStr("PostAccount");
                mdfDate = Convert.ToDateTime(drFirst["PostDate"]);
            }
            else
            {
                mdfDate = Convert.ToDateTime(drFirst["MdfDate"]);
            }

            ltrMdfAccount.Text = mdfAccount;
            ltrMdfDate.Text = mdfDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
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