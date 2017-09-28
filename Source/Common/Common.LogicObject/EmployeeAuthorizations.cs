﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// 授權項目
    /// </summary>
    public class EmployeeAuthorizations
    {
        /// <summary>
        /// 可閱讀該項目
        /// </summary>
        public bool CanRead = false;
        /// <summary>
        /// 可修改該項目
        /// </summary>
        public bool CanEdit = false;
        /// <summary>
        /// 可閱讀自己的子項目
        /// </summary>
        public bool CanReadSubItemOfSelf = false;
        /// <summary>
        /// 可修改自己的子項目
        /// </summary>
        public bool CanEditSubItemOfSelf = false;
        /// <summary>
        /// 可新增自己的子項目
        /// </summary>
        public bool CanAddSubItemOfSelf = false;
        /// <summary>
        /// 可刪除自己的子項目
        /// </summary>
        public bool CanDelSubItemOfSelf = false;
        /// <summary>
        /// 可閱讀同部門的子項目
        /// </summary>
        public bool CanReadSubItemOfCrew = false;
        /// <summary>
        /// 可修改同部門的子項目
        /// </summary>
        public bool CanEditSubItemOfCrew = false;
        /// <summary>
        /// 可刪除同部門的子項目
        /// </summary>
        public bool CanDelSubItemOfCrew = false;
        /// <summary>
        /// 可閱讀任何人的子項目
        /// </summary>
        public bool CanReadSubItemOfOthers = false;
        /// <summary>
        /// 可修改任何人的子項目
        /// </summary>
        public bool CanEditSubItemOfOthers = false;
        /// <summary>
        /// 可刪除任何人的子項目
        /// </summary>
        public bool CanDelSubItemOfOthers = false;

        /// <summary>
        /// 授權項目
        /// </summary>
        public EmployeeAuthorizations()
        {
        }
    }

    /// <summary>
    /// 授權項目與目標資料的擁有者資訊
    /// </summary>
    public class EmployeeAuthorizationsWithOwnerInfoOfDataExamined : EmployeeAuthorizations
    {
        /// <summary>
        /// 目標資料的擁有者帳號
        /// </summary>
        public string OwnerAccountOfDataExamined = "";
        /// <summary>
        /// 目標資料的擁有者部門
        /// </summary>
        public int OwnerDeptIdOfDataExamined = 0;

        /// <summary>
        /// 授權項目與目標資料的擁有者資訊
        /// </summary>
        public EmployeeAuthorizationsWithOwnerInfoOfDataExamined()
            : base()
        {
        }
    }
}
