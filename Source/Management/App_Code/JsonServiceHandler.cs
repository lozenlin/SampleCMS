﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.LogicObject;
using System.Web.SessionState;

/// <summary>
/// 暫存身分的權限
/// </summary>
public class TemporarilyStoreRolePrivilege : JsonServiceHandlerAbstract
{
    protected RoleCommonOfBackend c;

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
        string roleName = GetParamValue("roleName");
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
}