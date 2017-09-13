-- Employee and Authority SP, DB Functions
use SampleCMS
go

----------------------------------------------------------------------------
-- 後端使用者相關
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/13>
-- Test:
-- Description: <取得後端使用者登入用資料>
/*
exec dbo.spEmployee_GetDataToLogin 'admin'
*/
-- =============================================
create procedure dbo.spEmployee_GetDataToLogin
@EmpAccount varchar(20)
as
begin
	select
		e.EmpPassword, e.IsAccessDenied, e.StartDate, 
		e.EndDate, e.PasswordHashed, r.RoleName
	from dbo.Employee e
		join dbo.EmployeeRole r on e.RoleId=r.RoleId
	where e.EmpAccount=@EmpAccount
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/13>
-- Description: <取得後端使用者資料>
-- Test:
/*
exec dbo.spEmployee_GetData 'admin'
*/
-- =============================================
create procedure dbo.spEmployee_GetData
@EmpAccount varchar(20)
as
begin
	select
		e.EmpId, e.EmpAccount, e.EmpPassword, 
		e.EmpName, e.Email, e.Remarks, 
		e.DeptId, d.DeptName, e.RoleId, 
		r.RoleName, r.RoleDisplayName, e.IsAccessDenied, 
		e.PostAccount, e.PostDate, e.MdfAccount, 
		e.MdfDate, e.StartDate, e.EndDate
	from dbo.Employee e
		join dbo.EmployeeRole r on e.RoleId=r.RoleId
		left join dbo.Department d on e.DeptId=d.DeptId
	where e.EmpAccount=@EmpAccount
end
go

----------------------------------------------------------------------------
-- 後端操作記錄
----------------------------------------------------------------------------
go

-- =============================================
-- Author:		<lozen_lin>
-- Create date: <2017/09/13>
-- Description:	<新增後端操作記錄>
-- =============================================
create procedure dbo.spBackEndLog_InsertData
@EmpAccount varchar(20)
,@Description nvarchar(4000)
,@IP varchar(50)
as
begin
	insert into dbo.BackEndLog(
		EmpAccount, Description, OpDate, IP
		)
	values(
		@EmpAccount, @Description, getdate(), @IP
		)
end
go



/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/13>
-- Description: <xxxxxxxxxxxxxxxxxx>
-- Test:
/*
*/
-- =============================================
create procedure xxxxx

as
begin

end
go

*/
