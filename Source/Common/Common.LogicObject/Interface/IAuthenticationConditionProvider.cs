
using System;
namespace Common.LogicObject
{
    /// <summary>
    /// 提供驗證用的條件值
    /// </summary>
    public interface IAuthenticationConditionProvider
    {
        /// <summary>
        /// 取得後台網頁所屬的作業代碼
        /// </summary>
        int GetOpIdOfPage();
        /// <summary>
        /// 取得帳號
        /// </summary>
        string GetEmpAccount();
        /// <summary>
        /// 取得身分識別
        /// </summary>
        string GetRoleName();
        /// <summary>
        /// 判斷目前是否為指定的身分識別
        /// </summary>
        bool IsInRole(string roleName);
        /// <summary>
        /// 取得部門代碼
        /// </summary>
        int GetDeptId();
        /// <summary>
        /// 取得網頁代碼
        /// </summary>
        Guid GetArticleId();
    }
}
