using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.LogicObject;
using System.Web.SessionState;
using Common.Utility;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace JsonService
{
    /// <summary>
    /// 暫存身分的權限
    /// </summary>
    public class TemporarilyStoreRolePrivilege : JsonServiceHandlerAbstract, ICustomEmployeeAuthorizationResult
    {
        protected RoleCommonOfBackend c;
        protected EmployeeAuthorityLogic empAuth;

        private string roleName;
        private int roleId;

        /// <summary>
        /// 暫存身分的權限
        /// </summary>
        public TemporarilyStoreRolePrivilege(HttpContext context)
            : base(context)
        {
            c = new RoleCommonOfBackend(context, null);
            c.InitialLoggerOfUI(this.GetType());
        }

        public override ClientResult ProcessRequest()
        {
            ClientResult cr = null;
            roleName = GetParamValue("roleName");
            string strOpId = GetParamValue("opId");
            int opId = 0;
            string strItemVal = GetParamValue("itemVal");
            int itemVal = 0;
            string strSelfVal = GetParamValue("selfVal");
            int selfVal = 0;
            string strCrewVal = GetParamValue("crewVal");
            int crewVal = 0;
            string strOthersVal = GetParamValue("othersVal");
            int othersVal = 0;
            string strAddVal = GetParamValue("addVal");
            bool addVal = false;
            string strRoleId = GetParamValue("roleId");
            roleId = 0;

            if (!int.TryParse(strOpId, out opId))
                throw new Exception("opId is invalid");
            if (!int.TryParse(strItemVal, out itemVal))
                throw new Exception("itemVal is invalid");
            if (!int.TryParse(strSelfVal, out selfVal))
                throw new Exception("selfVal is invalid");
            if (!int.TryParse(strCrewVal, out crewVal))
                throw new Exception("crewVal is invalid");
            if (!int.TryParse(strOthersVal, out othersVal))
                throw new Exception("othersVal is invalid");
            if (!bool.TryParse(strAddVal, out addVal))
                throw new Exception("addVal is invalid");
            if (!int.TryParse(strRoleId, out roleId))
                throw new Exception("roleId is invalid");

            // authenticate
            empAuth = new EmployeeAuthorityLogic(c);
            empAuth.SetCustomEmployeeAuthorizationResult(this);
            empAuth.InitialAuthorizationResultOfSubPages();

            if (!empAuth.CanEditThisPage())
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid authentication"
                };

                return cr;
            }

            // check limitation
            if (itemVal == 0 && selfVal > 0)
                selfVal = 0;

            if (selfVal < 2 && addVal == true)
                addVal = false;

            if (crewVal > selfVal)
                crewVal = selfVal;

            if (othersVal > crewVal)
                othersVal = crewVal;

            RoleOpPvg newPvg = GetRoleOpPvg(roleName, opId, itemVal,
                selfVal, crewVal, othersVal,
                addVal);

            // save into list in the session
            RoleOpPvg oldPvg = c.seRoleOpPvgs.Find(pvg => string.Compare(pvg.RoleName, roleName, true) == 0 && pvg.OpId == opId);

            if (oldPvg != null)
            {
                oldPvg.PvgOfItem = newPvg.PvgOfItem;
                oldPvg.PvgOfSubitemSelf = newPvg.PvgOfSubitemSelf;
                oldPvg.PvgOfSubitemCrew = newPvg.PvgOfSubitemCrew;
                oldPvg.PvgOfSubitemOthers = newPvg.PvgOfSubitemOthers;
            }
            else
            {
                c.seRoleOpPvgs.Add(newPvg);
            }

            cr = new ClientResult()
            {
                b = true,
                o = newPvg
            };

            return cr;
        }

        private RoleOpPvg GetRoleOpPvg(string roleName, int opId, int itemVal,
            int selfVal, int crewVal, int othersVal,
            bool addVal)
        {
            int pvgOfItem = GetCompositeValueOfPvg(itemVal, false);
            int pvgOfSubitemSelf = GetCompositeValueOfPvg(selfVal, addVal);
            int pvgOfSubitemCrew = GetCompositeValueOfPvg(crewVal, false);
            int pvgOfSubitemOthers = GetCompositeValueOfPvg(othersVal, false);

            RoleOpPvg roleOpPvg = new RoleOpPvg()
            {
                RoleName = roleName,
                OpId = opId,
                PvgOfItem = pvgOfItem,
                PvgOfSubitemSelf = pvgOfSubitemSelf,
                PvgOfSubitemCrew = pvgOfSubitemCrew,
                PvgOfSubitemOthers = pvgOfSubitemOthers
            };

            return roleOpPvg;
        }

        private int GetCompositeValueOfPvg(int uiVal, bool addVal)
        {
            int pvg = 0;

            //read
            if (uiVal >= 1)
                pvg |= 1;

            //edit
            if (uiVal >= 2)
                pvg |= 2;

            //add
            if (addVal)
                pvg |= 4;

            //delete
            if (uiVal >= 3)
                pvg |= 8;

            return pvg;
        }

        #region ICustomEmployeeAuthorizationResult

        public EmployeeAuthorizationsWithOwnerInfoOfDataExamined InitialAuthorizationResult(bool isTopPageOfOperation, EmployeeAuthorizations authorizations)
        {
            EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = new EmployeeAuthorizationsWithOwnerInfoOfDataExamined(authorizations);

            if (!isTopPageOfOperation)
            {
                // get owner info for config-form
                DataSet ds = empAuth.GetEmployeeRoleData(roleId);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drFirst = ds.Tables[0].Rows[0];

                    if (drFirst.ToSafeStr("RoleName") == roleName)
                    {
                        authAndOwner.OwnerAccountOfDataExamined = drFirst.ToSafeStr("PostAccount");
                        authAndOwner.OwnerDeptIdOfDataExamined = Convert.ToInt32(drFirst["PostDeptId"]);
                    }
                }
            }

            return authAndOwner;
        }

        #endregion
    }

    public class ArticleAjaxeHandler : JsonServiceHandlerAbstract
    {
        public ArticleAjaxeHandler(HttpContext context)
            : base(context)
        {
        }

        public override ClientResult ProcessRequest()
        {
            return null;
        }

        protected bool HandleAuthToken(string token, string empAccount, out ArticleAjaxAuthData authData)
        {
            bool isValidToken = true;
            authData = null;

            if (string.IsNullOrEmpty(token))
            {
                isValidToken = false;
            }

            // decrypt token
            if (isValidToken)
            {
                try
                {
                    string aesKeyOfBP = ConfigurationManager.AppSettings["AesKeyOfBP"];
                    string basicIV = ConfigurationManager.AppSettings["AesIV"];
                    string authJson = AesUtility.Decrypt(token, aesKeyOfBP, basicIV);
                    authData = JsonConvert.DeserializeObject<ArticleAjaxAuthData>(authJson);
                }
                catch (Exception ex)
                {
                    logger.Error("", ex);
                    isValidToken = false;
                }
            }

            // check account
            if (isValidToken)
            {
                if (empAccount != authData.EmpAccount)
                    isValidToken = false;
            }

            // check postDate
            if (isValidToken)
            {
                if ((DateTime.Now - authData.PostDate).TotalHours >= 24)
                    isValidToken = false;
            }

            return isValidToken;
        }
    }

    /// <summary>
    /// 更新網頁內容的指定區域是否在前台顯示
    /// </summary>
    public class UpdateArticleIsAreaShowInFrontStage : ArticleAjaxeHandler
    {
        protected BackendPageCommon c;

        /// <summary>
        /// 更新網頁內容的指定區域是否在前台顯示
        /// </summary>
        public UpdateArticleIsAreaShowInFrontStage(HttpContext context)
            : base(context)
        {
            c = new BackendPageCommon(context, null);
            c.InitialLoggerOfUI(this.GetType());
        }

        public override ClientResult ProcessRequest()
        {
            ClientResult cr = null;

            string mdfAccount = c.GetEmpAccount();

            if (string.IsNullOrEmpty(mdfAccount))
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid login status"
                };

                return cr;
            }

            string token = GetParamValue("token");
            ArticleAjaxAuthData authData = null;

            if (!HandleAuthToken(token, c.GetEmpAccount(), out authData))
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid token"
                };

                return cr;
            }

            string artId = GetParamValue("artId");
            Guid articleId;

            if (!Guid.TryParse(artId, out articleId))
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid artId"
                };

                return cr;
            }

            string areaName = GetParamValue("areaName");
            bool isShow = Convert.ToBoolean(GetParamValue("isShow"));
            ArticlePublisherLogic artPub = new ArticlePublisherLogic();

            ArticleUpdateIsAreaShowInFrontStageParams param = new ArticleUpdateIsAreaShowInFrontStageParams()
            {
                ArticleId = articleId,
                AreaName = areaName,
                IsShowInFrontStage = isShow,
                MdfAccount = mdfAccount, 
                AuthUpdateParams = new AuthenticationUpdateParams()
                {
                    CanEditSubItemOfOthers = authData.CanEditSubItemOfOthers,
                    CanEditSubItemOfCrew = authData.CanEditSubItemOfCrew,
                    CanEditSubItemOfSelf = authData.CanEditSubItemOfSelf,
                    MyAccount = c.GetEmpAccount(),
                    MyDeptId = c.GetDeptId()
                }
            };

            bool result = artPub.UpdateArticleIsAreaShowInFrontStage(param);

            if (result)
            {
                cr = new ClientResult() { b = true };
            }
            else
            {
                cr = new ClientResult() { b = false, err = "update failed" };
            }

            return cr;
        }
    }

    /// <summary>
    /// 更新網頁內容的前台子項目排序欄位
    /// </summary>
    public class UpdateArticleSortFieldOfFrontStage : ArticleAjaxeHandler
    {
        protected BackendPageCommon c;

        /// <summary>
        /// 更新網頁內容的前台子項目排序欄位
        /// </summary>
        public UpdateArticleSortFieldOfFrontStage(HttpContext context)
            : base(context)
        {
            c = new BackendPageCommon(context, null);
            c.InitialLoggerOfUI(this.GetType());
        }

        public override ClientResult ProcessRequest()
        {
            ClientResult cr = null;

            string mdfAccount = c.GetEmpAccount();

            if (string.IsNullOrEmpty(mdfAccount))
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid login status"
                };

                return cr;
            }

            string token = GetParamValue("token");
            ArticleAjaxAuthData authData = null;

            if (!HandleAuthToken(token, c.GetEmpAccount(), out authData))
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid token"
                };

                return cr;
            }

            string artId = GetParamValue("artId");
            Guid articleId;

            if (!Guid.TryParse(artId, out articleId))
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "invalid artId"
                };

                return cr;
            }

            string sortField = GetParamValue("sortField");
            string strIsSortDesc = GetParamValue("isSortDesc");
            bool isSortDesc = false;

            if (strIsSortDesc == "")
            {
                strIsSortDesc = isSortDesc.ToString();
            }
            else
            {
                isSortDesc = Convert.ToBoolean(strIsSortDesc);
            }

            if (sortField == "")
            {
                strIsSortDesc = "";
            }

            ArticlePublisherLogic artPub = new ArticlePublisherLogic();

            ArticleUpdateSortFieldOfFrontStageParams param = new ArticleUpdateSortFieldOfFrontStageParams()
            {
                ArticleId = articleId,
                SortFieldOfFrontStage = sortField,
                IsSortDescOfFrontStage = isSortDesc,
                MdfAccount = mdfAccount,
                AuthUpdateParams = new AuthenticationUpdateParams()
                {
                    CanEditSubItemOfOthers = authData.CanEditSubItemOfOthers,
                    CanEditSubItemOfCrew = authData.CanEditSubItemOfCrew,
                    CanEditSubItemOfSelf = authData.CanEditSubItemOfSelf,
                    MyAccount = c.GetEmpAccount(),
                    MyDeptId = c.GetDeptId()
                }
            };

            bool result = artPub.UpdateArticleSortFieldOfFrontStage(param);

            if (result)
            {
                SortFieldInfo sortFieldInfo = new SortFieldInfo()
                {
                    sortField = sortField,
                    isSortDesc = strIsSortDesc
                };

                cr = new ClientResult()
                {
                    b = true,
                    o = sortFieldInfo
                };
            }
            else
            {
                cr = new ClientResult() { b = false, err = "update failed" };
            }

            return cr;

        }

        public class SortFieldInfo
        {
            public string sortField;
            public string isSortDesc;
        }
    }
}