
namespace Common.LogicObject
{
    /// <summary>
    /// 自訂帳號授權結果
    /// </summary>
    public interface ICustomEmployeeAuthorizationResult
    {
        EmployeeAuthorizationsWithOwnerInfoOfDataExamined InitialAuthorizationResult(bool isTopPageOfOperation, EmployeeAuthorizations authorizations);
    }
}
