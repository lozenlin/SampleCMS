﻿using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Article_Node : BasePage
{
    protected ArticleCommonOfBackend c;
    protected ArticlePublisherLogic artPub;
    protected EmployeeAuthorityLogic empAuth;
    private IHeadUpDisplay hud = null;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new ArticleCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        c.SelectMenuItemToThisPage();

        artPub = new ArticlePublisherLogic(c);

        empAuth = new EmployeeAuthorityLogic(c);
        empAuth.SetCustomEmployeeAuthorizationResult(artPub);
        empAuth.InitialAuthorizationResultOfTopPage();

        hud = Master.GetHeadUpDisplay();
        isBackendPage = true;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ucDataPager.MaxItemCountOfPage = 20;
        ucDataPager.MaxDisplayCountInPageCodeArea = 5;
        ucDataPager.LinkUrlToReload = string.Concat(Request.AppRelativeCurrentExecutionFilePath, "?", Request.QueryString);
        ucDataPager.Initialize(0, 0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RebuildBreadcrumbAndHeadOfHUD();
        Title = hud.GetHeadText() + " - " + Title;

        if (!IsPostBack)
        {
            if (!empAuth.CanOpenThisPage())
            {
                Response.Redirect(c.BACK_END_HOME);
            }

            LoadUIData();
            DisplayArticle();
            DisplayAttachFiles();
        }
        else
        {
            //PostBack
            if (Master.FlagValue != "")
            {
                // message from config-form

                if (Master.FlagValue == "Config")
                {
                    DisplayArticle();
                    Master.RefreshOpMenu();
                }
                else if (Master.FlagValue == "Attach")
                {
                    DisplayAttachFiles();
                }

                Master.FlagValue = "";
            }
        }
    }

    private void RebuildBreadcrumbAndHeadOfHUD()
    {
        StringBuilder sbBreadcrumbWoHome = new StringBuilder(100);

        DataSet dsLevelInfo = artPub.GetArticleMultiLangLevelInfo(c.qsArtId, c.seCultureNameOfBackend);

        if (dsLevelInfo != null && dsLevelInfo.Tables[0].Rows.Count > 0)
        {
            int total = dsLevelInfo.Tables[0].Rows.Count;

            for (int itemNum = total; itemNum >= 1; itemNum--)
            {
                DataRow drArticle = dsLevelInfo.Tables[0].Rows[itemNum - 1];

                Guid articleId = (Guid)drArticle["ArticleId"];
                string articleSubject = drArticle.ToSafeStr("ArticleSubject");
                int articleLevelNo = Convert.ToInt32(drArticle["ArticleLevelNo"]);
                string url = string.Format("Article-Node.aspx?artid={0}", articleId);

                if (itemNum == 1)
                {
                    sbBreadcrumbWoHome.Append(hud.GetBreadcrumbTextItemHtml(articleSubject));
                    // update head of HUD
                    hud.SetHeadText(articleSubject);

                    // get icon of operation
                    OperationHtmlAnchorData anchorData = empAuth.GetOperationHtmlAnchorData(c.GetOpIdOfPage(), false);

                    if (anchorData != null && anchorData.IconImageFileUrl != "")
                    {
                        string iconImageFile = "~/BPImages/icon/" + anchorData.IconImageFileUrl;
                        hud.SetHeadIconImageUrl(iconImageFile);
                    }
                }
                else
                {
                    sbBreadcrumbWoHome.Append(hud.GetBreadcrumbLinkItemHtml(articleSubject, articleSubject, url));

                    if (itemNum == 2)
                    {
                        // set url of BackToParent button
                        string backToParentUrl = "~/" + c.BuildUrlOfListPage(articleId,
                            c.qsKwOfParent, c.qsSortField, c.qsIsSortDesc,
                            StringUtility.GetLastNumOfParents(c.qsPageCodeOfParents), StringUtility.GetNumOfParentsForParent(c.qsPageCodeOfParents), "");

                        hud.SetButtonAttribute(HudButtonNameEnum.BackToParent, HudButtonAttributeEnum.NavigateUrl, backToParentUrl);
                    }
                }
            }
        }

        hud.RebuildBreadcrumb(sbBreadcrumbWoHome.ToString(), true);
    }

    private void LoadUIData()
    {
        btnShowTypeLinkUrl.InnerHtml = Resources.Lang.PageShowTypeOption_URL;
        btnSearch.ToolTip = Resources.Lang.SearchPanel_btnSearch_Hint;
        btnClear.ToolTip = Resources.Lang.SearchPanel_btnClear_Hint;

        //HUD
        if (empAuth.CanEditThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.Edit, true);
            hud.SetButtonAttribute(HudButtonNameEnum.Edit, HudButtonAttributeEnum.JsInNavigateUrl,
                string.Format("popWin('Article-Config.aspx?act={0}&artid={1}', 700, 600);", ConfigFormAction.edit, c.qsArtId));
        }

        if (empAuth.CanAddSubItemInThisPage())
        {
            hud.SetButtonVisible(HudButtonNameEnum.AddNew, true);
            hud.SetButtonAttribute(HudButtonNameEnum.AddNew, HudButtonAttributeEnum.JsInNavigateUrl,
                string.Format("popWin('Article-Config.aspx?act={0}&artid={1}', 700, 600);", ConfigFormAction.add, c.qsArtId));
        }

        //conditions UI

        //condition vlaues
        txtKw.Text = c.qsKw;

        //columns of list
        btnSortArticleSubject.Text = Resources.Lang.Col_Subject;
        hidSortArticleSubject.Text = btnSortArticleSubject.Text;
        btnSortSortNo.Text = Resources.Lang.Col_SortNo;
        hidSortSortNo.Text = btnSortSortNo.Text;
        btnSortStartDate.Text = Resources.Lang.Col_ValidationDate;
        hidSortStartDate.Text = btnSortStartDate.Text;
        btnSortPostDeptName.Text = Resources.Lang.Col_DeptName;
        hidSortPostDeptName.Text = btnSortPostDeptName.Text;

        c.DisplySortableCols(new string[] { 
            "ArticleSubject", "SortNo", "StartDate", 
            "PostDeptName"
        });
    }

    private void DisplayArticle()
    {
        DataSet dsArticle = artPub.GetArticleDataForBackend(c.qsArtId);

        if (dsArticle != null && dsArticle.Tables[0].Rows.Count > 0)
        {
            DataRow drFirst = dsArticle.Tables[0].Rows[0];

            hidParentId.Text = drFirst.ToSafeStr("ParentId");
            hidArticleLevelNo.Text = drFirst.ToSafeStr("ArticleLevelNo");

            ltrValidDateRange.Text = string.Format("{0:yyyy-MM-dd} ~ {1:yyyy-MM-dd}", drFirst["StartDate"], drFirst["EndDate"]);

            int showTypeId = Convert.ToInt32(drFirst["ShowTypeId"]);
            string linkUrl = drFirst.ToSafeStr("LinkUrl");

            switch (showTypeId)
            {
                case 1:
                    // page
                    ltrShowTypeName.Text = Resources.Lang.PageShowTypeOption_Page;
                    break;
                case 2:
                    // to sub-page
                    ltrShowTypeName.Text = Resources.Lang.PageShowTypeOption_ToSubPage;
                    break;
                case 3:
                    // URL
                    ltrShowTypeName.Text = Resources.Lang.PageShowTypeOption_URL;
                    string showTypeLinkUrl = linkUrl;

                    if (showTypeLinkUrl.StartsWith("~/"))
                    {
                        showTypeLinkUrl = showTypeLinkUrl.Replace("~", "..");
                    }

                    btnShowTypeLinkUrl.HRef = showTypeLinkUrl;
                    btnShowTypeLinkUrl.Visible = true;
                    ltrShowTypeName.Visible = false;
                    break;
                case 4:
                    // use control
                    ltrShowTypeName.Text = Resources.Lang.PageShowTypeOption_UseControl;
                    break;
            }

            DataSet dsArticleMultiLang = artPub.GetArticleMultiLangDataForBackend(c.qsArtId, c.seCultureNameOfBackend);

            if (dsArticleMultiLang != null && dsArticleMultiLang.Tables[0].Rows.Count > 0)
            {
                DataRow drMultiLang = dsArticleMultiLang.Tables[0].Rows[0];

                string mdfAccount;
                DateTime mdfDate;

                if (Convert.IsDBNull(drMultiLang["MdfDate"]))
                {
                    mdfAccount = drMultiLang.ToSafeStr("PostAccount");
                    mdfDate = Convert.ToDateTime(drMultiLang["PostDate"]);
                }
                else
                {
                    mdfAccount = drMultiLang.ToSafeStr("MdfAccount");
                    mdfDate = Convert.ToDateTime(drMultiLang["MdfDate"]);
                }

                ltrMdfName.Text = mdfAccount;
                ltrMdfDate.Text = mdfDate.ToString("yyyy-MM-dd");
            }
        }

        DisplaySubitems();
    }

    private void DisplaySubitems()
    {
        ArticleListQueryParams param = new ArticleListQueryParams()
        {
            ParentId = c.qsArtId,
            CultureName = c.seCultureNameOfBackend,
            Kw = c.qsKw
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 0,
            EndNum = 0,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        param.AuthParams = new AuthenticationQueryParams()
        {
            CanReadSubItemOfOthers = empAuth.CanReadSubItemOfOthers(),
            CanReadSubItemOfCrew = empAuth.CanReadSubItemOfCrew(),
            CanReadSubItemOfSelf = empAuth.CanReadSubItemOfSelf(),
            MyAccount = c.GetEmpAccount(),
            MyDeptId = c.GetDeptId()
        };

        // get total of items
        artPub.GetArticleMultiLangListForBackend(param);

        // update pager and get begin end of item numbers
        int itemTotalCount = param.PagedParams.RowCount;
        ucDataPager.Initialize(itemTotalCount, c.qsPageCode);
        if (IsPostBack)
            ucDataPager.RefreshPagerAfterPostBack();

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = ucDataPager.BeginItemNumberOfPage,
            EndNum = ucDataPager.EndItemNumberOfPage,
            SortField = c.qsSortField,
            IsSortDesc = c.qsIsSortDesc
        };

        DataSet dsSubitems = artPub.GetArticleMultiLangListForBackend(param);

        if (dsSubitems != null)
        {
            rptSubitems.DataSource = dsSubitems.Tables[0];
            rptSubitems.DataBind();
        }

        if (c.qsPageCode > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "isSearchPanelCollapsingAtBeginning", "isSearchPanelCollapsingAtBeginning = true;", true);
        }
    }

    protected void rptSubitems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid articleId = (Guid)drvTemp["ArticleId"];
        string articleSubject = drvTemp.ToSafeStr("ArticleSubject");
        bool isShowInLangZhTw = drvTemp.To<bool>("IsShowInLangZhTw", false);
        bool isShowInLangEn = drvTemp.To<bool>("IsShowInLangEn", false);
        bool isHideSelf = Convert.ToBoolean(drvTemp["IsHideSelf"]);
        DateTime startDate = Convert.ToDateTime(drvTemp["StartDate"]);
        DateTime endDate = Convert.ToDateTime(drvTemp["EndDate"]);
        bool dontDelete = Convert.ToBoolean(drvTemp["DontDelete"]);

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
        btnItem.InnerHtml = articleSubject;
        btnItem.Title = articleSubject;
        btnItem.HRef = c.BuildUrlOfListPage(articleId,
            "", c.qsSortField, c.qsIsSortDesc,
            1, StringUtility.GetNumOfParentsForChild(c.qsPageCode, c.qsPageCodeOfParents), c.qsKw);

        HtmlGenericControl ctlIsShowInLangZhTw = (HtmlGenericControl)e.Item.FindControl("ctlIsShowInLangZhTw");
        ctlIsShowInLangZhTw.Attributes["class"] = StringUtility.GetCssClassOfIconIsShowInLang(isShowInLangZhTw);
        ctlIsShowInLangZhTw.Visible = LangManager.IsEnableEditLangZHTW();

        HtmlGenericControl ctlIsShowInLangEn = (HtmlGenericControl)e.Item.FindControl("ctlIsShowInLangEn");
        ctlIsShowInLangEn.Attributes["class"] = StringUtility.GetCssClassOfIconIsShowInLang(isShowInLangEn);
        ctlIsShowInLangEn.Visible = LangManager.IsEnableEditLangEN();

        HtmlTableRow ItemArea = (HtmlTableRow)e.Item.FindControl("ItemArea");

        if (isHideSelf)
        {
            ItemArea.Attributes["class"] = "table-danger";
        }

        HtmlGenericControl ctlArticleState = (HtmlGenericControl)e.Item.FindControl("ctlArticleState");

        if (DateTime.Today < startDate)
        {
            // on schedule
            ctlArticleState.Attributes["class"] = "fa fa-hourglass-start fa-lg text-info";
            ctlArticleState.Attributes["title"] = Resources.Lang.Status_OnSchedule;
        }
        else if (endDate < DateTime.Today)
        {
            // offline
            ctlArticleState.Attributes["class"] = "fa fa-ban fa-lg text-danger";
            ctlArticleState.Attributes["title"] = Resources.Lang.Status_AccessDeniedOrExpired;
            ItemArea.Attributes["class"] = "table-danger";
        }
        else
        {
            // online
            ctlArticleState.Attributes["title"] = Resources.Lang.Status_Normal;
        }

        Literal ltrValidDateRange = (Literal)e.Item.FindControl("ltrValidDateRange");
        ltrValidDateRange.Text = string.Format("{0:yyyy-MM-dd} ~ {1:yyyy-MM-dd}", startDate, endDate);

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Article-Config.aspx?act={0}&artid={1}', 700, 600); return false;", ConfigFormAction.edit, articleId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", articleId.ToString(), articleSubject);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Article_ConfirmDelete_Format + "');",
            articleSubject, articleId);

        if (dontDelete)
        {
            HtmlGenericControl ctlDontDelete = (HtmlGenericControl)e.Item.FindControl("ctlDontDelete");
            ctlDontDelete.Attributes["title"] = Resources.Lang.Status_DontDelete;
            ctlDontDelete.Visible = true;
            btnDelete.Visible = false;
        }

        string ownerAccount = drvTemp.ToSafeStr("PostAccount");
        int ownerDeptId = Convert.ToInt32(drvTemp["PostDeptId"]);

        btnEdit.Visible = empAuth.CanEditThisPage(false, ownerAccount, ownerDeptId);

        if (!empAuth.CanDelThisPage(ownerAccount, ownerDeptId))
        {
            btnDelete.Visible = false;
        }

    }

    protected void rptSubitems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        bool result = false;
        Guid articleId;

        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                articleId = new Guid(args[0]);
                string articleSubject = args[1];

                result = artPub.DeleteArticleData(articleId);

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除網頁內容/Delete article　．代碼/id[{0}]　標題/subject[{1}]　結果/result[{2}]", articleId, articleSubject, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "article-[" + articleId.ToString() + "]-" + articleSubject, result);

                if (!result)
                {
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_DeleteArticleFailed);
                }

                break;
            case "MoveUp":
                articleId = new Guid(e.CommandArgument.ToString());
                result = artPub.DecreaseArticleSortNo(articleId, c.GetEmpAccount());
                break;
            case "MoveDown":
                articleId = new Guid(e.CommandArgument.ToString());
                result = artPub.IncreaseArticleSortNo(articleId, c.GetEmpAccount());
                break;
        }

        if (result)
        {
            DisplayArticle();
            Master.RefreshOpMenu();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtKw.Text = txtKw.Text.Trim();

        Response.Redirect(c.BuildUrlOfListPage(c.qsArtId,
            txtKw.Text, "", false,
            1, c.qsPageCodeOfParents, c.qsKwOfParent));
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(c.BuildUrlOfListPage(c.qsArtId,
            "", "", false,
            1, c.qsPageCodeOfParents, c.qsKwOfParent));
    }

    protected void btnSort_Click(object sender, EventArgs e)
    {
        LinkButton btnSort = (LinkButton)sender;
        string sortField = btnSort.CommandArgument;
        bool isSortDesc = false;
        c.ChangeSortStateToNext(ref sortField, out isSortDesc);

        //重新載入頁面
        Response.Redirect(c.BuildUrlOfListPage(c.qsArtId,
            c.qsKw, sortField, isSortDesc,
            c.qsPageCode, c.qsPageCodeOfParents, c.qsKwOfParent));
    }

    private void DisplayAttachFiles()
    {
        btnUploadAttachFile.Attributes["onclick"] = 
            string.Format("popWin('Article-Attach.aspx?act={0}&artid={1}', 700, 600); return false;", 
                ConfigFormAction.add, c.qsArtId);

        AttachFileListQueryParams param = new AttachFileListQueryParams()
        {
            ArticleId = c.qsArtId,
            CultureName = c.seCultureNameOfBackend,
            Kw = ""
        };

        param.PagedParams = new PagedListQueryParams()
        {
            BeginNum = 1,
            EndNum = 999999999,
            SortField = "",
            IsSortDesc = false
        };

        param.AuthParams = new AuthenticationQueryParams()
        {
            CanReadSubItemOfOthers = empAuth.CanReadSubItemOfOthers(),
            CanReadSubItemOfCrew = empAuth.CanReadSubItemOfCrew(),
            CanReadSubItemOfSelf = empAuth.CanReadSubItemOfSelf(),
            MyAccount = c.GetEmpAccount(),
            MyDeptId = c.GetDeptId()
        };

        DataSet dsAttachFiles = artPub.GetAttachFileMultiLangListForBackend(param);

        if (dsAttachFiles != null)
        {
            rptAttachFiles.DataSource = dsAttachFiles.Tables[0];
            rptAttachFiles.DataBind();
        }
    }

    protected void rptAttachFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drvTemp = (DataRowView)e.Item.DataItem;

        Guid attId = (Guid)drvTemp["AttId"];
        string attSubject = drvTemp.ToSafeStr("AttSubject");
        bool isShowInLangZhTw = drvTemp.To<bool>("IsShowInLangZhTw", false);
        bool isShowInLangEn = drvTemp.To<bool>("IsShowInLangEn", false);
        bool dontDelete = Convert.ToBoolean(drvTemp["DontDelete"]);
        string mdfAccount = "";
        DateTime mdfDate = DateTime.MinValue;

        if(Convert.IsDBNull(drvTemp["MdfDate"]))
        {
            mdfAccount = drvTemp.ToSafeStr("PostAccount");
            mdfDate = Convert.ToDateTime(drvTemp["PostDate"]);
        }
        else
        {
            mdfAccount = drvTemp.ToSafeStr("MdfAccount");
            mdfDate = Convert.ToDateTime(drvTemp["MdfDate"]);
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

        HtmlGenericControl ctlIsShowInLangZhTw = (HtmlGenericControl)e.Item.FindControl("ctlIsShowInLangZhTw");
        ctlIsShowInLangZhTw.Attributes["class"] = StringUtility.GetCssClassOfIconIsShowInLang(isShowInLangZhTw);
        ctlIsShowInLangZhTw.Visible = LangManager.IsEnableEditLangZHTW();

        HtmlGenericControl ctlIsShowInLangEn = (HtmlGenericControl)e.Item.FindControl("ctlIsShowInLangEn");
        ctlIsShowInLangEn.Attributes["class"] = StringUtility.GetCssClassOfIconIsShowInLang(isShowInLangEn);
        ctlIsShowInLangEn.Visible = LangManager.IsEnableEditLangEN();

        Literal ltrMdfAccount = (Literal)e.Item.FindControl("ltrMdfAccount");
        ltrMdfAccount.Text = mdfAccount;

        Literal ltrMdfDate = (Literal)e.Item.FindControl("ltrMdfDate");
        ltrMdfDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", mdfDate);

        string fileSavedName = drvTemp.ToSafeStr("FileSavedName");
        HtmlImage imgExt = (HtmlImage)e.Item.FindControl("imgExt");
        imgExt.Src = "BPimages/FileExtIcon/" + ResUtility.GetExtIconFileName(fileSavedName);
        imgExt.Alt = ResUtility.GetExtIconText(fileSavedName);
        imgExt.Attributes["title"] = imgExt.Alt;

        HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
        btnEdit.Attributes["onclick"] = string.Format("popWin('Article-Attach.aspx?act={0}&attid={1}', 700, 600); return false;", ConfigFormAction.edit, attId);
        btnEdit.Title = Resources.Lang.Main_btnEdit_Hint;

        Literal ltrEdit = (Literal)e.Item.FindControl("ltrEdit");
        ltrEdit.Text = Resources.Lang.Main_btnEdit;

        LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
        btnDelete.CommandArgument = string.Join(",", attId.ToString(), attSubject);
        btnDelete.Text = "<i class='fa fa-trash-o'></i> " + Resources.Lang.Main_btnDelete;
        btnDelete.ToolTip = Resources.Lang.Main_btnDelete_Hint;
        btnDelete.OnClientClick = string.Format("return confirm('" + Resources.Lang.Article_ConfirmDelete_Format + "');",
            attSubject, attId);

        if (dontDelete)
        {
            HtmlGenericControl ctlDontDelete = (HtmlGenericControl)e.Item.FindControl("ctlDontDelete");
            ctlDontDelete.Attributes["title"] = Resources.Lang.Status_DontDelete;
            ctlDontDelete.Visible = true;
            btnDelete.Visible = false;
        }

        HtmlAnchor btnDownload = (HtmlAnchor)e.Item.FindControl("btnDownload");
        btnDownload.HRef = string.Format("~/FileAtt.ashx?attid={0}", attId);

        string ownerAccount = drvTemp.ToSafeStr("PostAccount");
        int ownerDeptId = Convert.ToInt32(drvTemp["PostDeptId"]);

        btnEdit.Visible = empAuth.CanEditThisPage(false, ownerAccount, ownerDeptId);

        if (!empAuth.CanDelThisPage(ownerAccount, ownerDeptId))
        {
            btnDelete.Visible = false;
        }

    }

    protected void rptAttachFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        bool result = false;
        Guid attId;

        switch (e.CommandName)
        {
            case "Del":
                string[] args = e.CommandArgument.ToString().Split(',');
                attId = new Guid(args[0]);
                string attSubject = args[1];

                AttachFileManagerLogic attFileMgr = new AttachFileManagerLogic(this.Context);
                if (!attFileMgr.Initialize(attId, c.qsArtId))
                {
                    string errMsg = ResUtility.GetErrMsgOfAttFileErrState(attFileMgr.GetErrState());

                    if (errMsg == "")
                    {
                        errMsg = "刪除附件失敗";
                    }

                    Master.ShowErrorMsg(errMsg);
                    return;
                }

                result = attFileMgr.DeleteData();

                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = c.GetEmpAccount(),
                    Description = string.Format("．刪除附件/Delete attach file　．代碼/id[{0}]　標題/subject[{1}]　結果/result[{2}]", attId, attSubject, result),
                    IP = c.GetClientIP()
                });

                // log to file
                c.LoggerOfUI.InfoFormat("{0} deletes {1}, result: {2}", c.GetEmpAccount(), "attach file-[" + attId.ToString() + "]-" + attSubject, result);

                if (!result)
                {
                    Master.ShowErrorMsg("刪除附件失敗");
                }

                break;
            case "MoveUp":
                attId = new Guid(e.CommandArgument.ToString());
                result = artPub.DecreaseAttachFileSortNo(attId, c.GetEmpAccount());
                break;
            case "MoveDown":
                attId = new Guid(e.CommandArgument.ToString());
                result = artPub.IncreaseAttachFileSortNo(attId, c.GetEmpAccount());
                break;
        }

        if (result)
        {
            DisplayAttachFiles();
        }
    }
}