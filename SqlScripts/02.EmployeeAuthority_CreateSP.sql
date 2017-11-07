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
-- History:
--	2017/10/31, lozen_lin, modify, 新增欄位「本次登入時間,本次登入IP,上次登入時間,上次登入IP」
-- Description: <取得後端使用者資料>
-- Test:
/*
exec dbo.spEmployee_GetData 'admin'
*/
-- =============================================
alter procedure dbo.spEmployee_GetData
@EmpAccount varchar(20)
as
begin
	select
		e.EmpId, e.EmpAccount, e.EmpPassword, 
		e.EmpName, e.Email, e.Remarks, 
		e.DeptId, d.DeptName, e.RoleId, 
		r.RoleName, r.RoleDisplayName, e.IsAccessDenied, 
		e.PostAccount, e.PostDate, e.MdfAccount, 
		e.MdfDate, e.StartDate, e.EndDate, 
		e.ThisLoginTime, e.ThisLoginIP, e.LastLoginTime,
		e.LastLoginIP, e.OwnerAccount, e.PasswordHashed, 
		e.DefaultRandomPassword
	from dbo.Employee e
		join dbo.EmployeeRole r on e.RoleId=r.RoleId
		left join dbo.Department d on e.DeptId=d.DeptId
	where e.EmpAccount=@EmpAccount
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/07>
-- Description: <取得員工代碼的帳號>
-- Test:
/*
exec dbo.spEmployee_GetAccountOfId 1
*/
-- =============================================
create procedure dbo.spEmployee_GetAccountOfId
@EmpId int
as
begin
	select EmpAccount
	from dbo.Employee
	where EmpId=@EmpId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/04>
-- Description: <取得後端使用者清單>
-- Test:
/*
declare @RowCount int
exec dbo.spEmployee_GetList 0, '', 0, 1, 99999, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
alter procedure dbo.spEmployee_GetList
@DeptId int=0	-- 0:all
,@Kw nvarchar(52)=''
,@ListMode int=0	--清單內容模式(0:all, 1:normal, 2:access denied)
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@RowCount int output
as
begin
	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)

	--條件定義
	set @conditions=N''
	
	if @DeptId<>0
	begin
		set @conditions += N' and e.DeptId=@DeptId '
	end
	
	if @Kw<>N''
	begin
		set @conditions += N' and (e.EmpAccount like @Kw or e.EmpName like @Kw) '
	end
	
	if @ListMode<>0
	begin
		if @ListMode=1
		begin
			set @conditions += N' and e.IsAccessDenied=0 and (r.RoleName=''admin'' or getdate() between e.StartDate and e.EndDate)'
		end
		else if @ListMode=2
		begin
			set @conditions += N' and e.IsAccessDenied=1 or not (r.RoleName=''admin'' or getdate() between e.StartDate and e.EndDate)'
		end
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.Employee e
	join dbo.EmployeeRole r on e.RoleId=r.RoleId
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@DeptId int
,@Kw nvarchar(52)
,@ListMode int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @Kw = N'%'+@Kw+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@DeptId
		,@Kw
		,@ListMode
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'DeptName', N'RoleSortNo', N'EmpName', N'EmpAccount', N'StartDate')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by DeptId, RoleSortNo, EmpName'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			d.DeptName, e.DeptId, e.EmpId, 
			e.EmpAccount, e.EmpName, e.Email, 
			e.Remarks, e.RoleId, r.SortNo as RoleSortNo, 
			r.RoleName, r.RoleDisplayName, e.IsAccessDenied, 
			oe.EmpName as OwnerName, e.OwnerAccount, e.PostDate, 
			e.MdfAccount, e.MdfDate, isnull(oe.DeptId, 0) as OwnerDeptId, 
			e.StartDate, e.EndDate
		from dbo.Employee e
			join dbo.EmployeeRole r on e.RoleId=r.RoleId
			left join dbo.Department d on e.DeptId=d.DeptId
			left join dbo.Employee oe on e.OwnerAccount=oe.EmpAccount
		where 1=1' + @conditions + N'
	) main 
) result 
where RowNum between @BeginNum and @EndNum 
order by RowNum'

	set @parmDef += N'
,@BeginNum int
,@EndNum int
'
	exec sp_executesql @sql, @parmDef, 
		@DeptId
		,@Kw
		,@ListMode
		,@BeginNum
		,@EndNum
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/10/27>
-- Description: <取得後端使用者角色名稱>
-- Test:
/*
exec dbo.spEmployee_GetRoleName 'admin'
*/
-- =============================================
create procedure dbo.spEmployee_GetRoleName
@EmpAccount varchar(20)
as
begin
	select
		r.RoleName
	from dbo.Employee e
		join dbo.EmployeeRole r on e.RoleId=r.RoleId
	where e.EmpAccount=@EmpAccount
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/10/31>
-- Description: <更新後端使用者本次登入資訊>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployee_UpdateLoginInfo
@EmpAccount varchar(20)
,@ThisLoginIP	varchar(50)
as
begin
	declare @ThisLoginTime	datetime = getdate()

	--備份上次的登入資訊,記錄這次的
	update dbo.Employee
	set LastLoginTime=ThisLoginTime
		,LastLoginIP=ThisLoginIP
		,ThisLoginTime=@ThisLoginTime
		,ThisLoginIP=@ThisLoginIP
	where EmpAccount=@EmpAccount
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/06>
-- Description: <刪除後端使用者資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployee_DeleteData
@EmpId int
as
begin
	delete from dbo.Employee
	where EmpId=@EmpId
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

----------------------------------------------------------------------------
-- 網頁後端作業選項相關
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/28>
-- Description: <用共用元件類別名稱取得後端作業選項資訊>
-- Test:
/*
exec dbo.spOperations_GetOpInfoByCommonClass ''
*/
-- =============================================
create procedure dbo.spOperations_GetOpInfoByCommonClass
@CommonClass varchar(100)
as
begin
	select top 1
		o.OpId
	from dbo.Operations o
	where o.CommonClass=@CommonClass
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/10/31>
-- Description: <取得後端作業選項第一層清單和角色授權>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spOperations_GetTopListWithRoleAuth
@RoleName nvarchar(20)
as
begin
	select
		o.OpId, o.OpSubject, o.LinkUrl, 
		o.IsNewWindow, o.IconImageFile, 
		ro.CanRead, ro.CanEdit, ro.CanReadSubItemOfSelf, 
		ro.CanEditSubItemOfSelf, ro.CanAddSubItemOfSelf, ro.CanDelSubItemOfSelf, 
		ro.CanReadSubItemOfCrew, ro.CanEditSubItemOfCrew, ro.CanDelSubItemOfCrew, 
		ro.CanReadSubItemOfOthers, ro.CanEditSubItemOfOthers, ro.CanDelSubItemOfOthers
	from dbo.Operations o
		left join dbo.EmployeeRoleOperationsDesc ro on ro.RoleName=@RoleName and o.OpId=ro.OpId
	where o.ParentId is null
		and o.IsHideSelf=0
	order by o.SortNo
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/10/31>
-- Description: <取得後端作業選項子清單和角色授權>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spOperations_GetSubListWithRoleAuth
@RoleName nvarchar(20)
as
begin
	select
		o.OpId, o.ParentId, o.OpSubject, 
		o.LinkUrl, o.IsNewWindow, o.IconImageFile, 
		ro.CanRead, ro.CanEdit, ro.CanReadSubItemOfSelf, 
		ro.CanEditSubItemOfSelf, ro.CanAddSubItemOfSelf, ro.CanDelSubItemOfSelf, 
		ro.CanReadSubItemOfCrew, ro.CanEditSubItemOfCrew, ro.CanDelSubItemOfCrew, 
		ro.CanReadSubItemOfOthers, ro.CanEditSubItemOfOthers, ro.CanDelSubItemOfOthers
	from dbo.Operations o
		join dbo.Operations parent on o.ParentId=parent.OpId and parent.IsHideSelf=0
		left join dbo.EmployeeRoleOperationsDesc ro on ro.RoleName=@RoleName and o.OpId=ro.OpId
	where o.ParentId is not null
		and o.IsHideSelf=0
	order by o.ParentId, o.SortNo
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/02>
-- Description: <取得後端作業選項資料>
-- Test:
/*
exec dbo.spOperations_GetData 2
*/
-- =============================================
create procedure dbo.spOperations_GetData
@OpId int
as
begin
	select
		o.OpId, o.OpSubject, o.LinkUrl, 
		o.IconImageFile, o.PostAccount, o.PostDate, o.MdfAccount, o.MdfDate, 
		e.EmpName as PostName, d.DeptName as PostDeptName
	from dbo.Operations o
		left join dbo.Employee e on o.PostAccount=e.EmpAccount
		left join dbo.Department d on e.DeptId=d.DeptId
	where OpId=@OpId
end
go

----------------------------------------------------------------------------
-- 員工角色後端作業授權相關
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/28>
-- Description: <取得指定作業代碼的後端角色可使用權限>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployeeRoleOperationsDesc_GetDataOfOp
@RoleName nvarchar(20),
@OpId int
as
begin
	select 
		ro.CanRead, ro.CanEdit, ro.CanReadSubItemOfSelf, 
		ro.CanEditSubItemOfSelf, ro.CanAddSubItemOfSelf, ro.CanDelSubItemOfSelf, 
		ro.CanReadSubItemOfCrew, ro.CanEditSubItemOfCrew, ro.CanDelSubItemOfCrew, 
		ro.CanReadSubItemOfOthers, ro.CanEditSubItemOfOthers, ro.CanDelSubItemOfOthers
	from dbo.EmployeeRoleOperationsDesc ro
	where RoleName=@RoleName and OpId=@OpId
end
go

----------------------------------------------------------------------------
-- 員工角色
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/07>
-- Description: <取得選擇用員工角色清單>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployeeRole_GetListToSelect
as
begin
	select
		RoleId, RoleName, RoleDisplayName,
		isnull(RoleDisplayName, N'') + N' ('+isnull(RoleName, N'')+N')' as DisplayText
	from dbo.EmployeeRole
	order by SortNo
end
go

----------------------------------------------------------------------------
-- 部門資料
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/04>
-- Description: <取得選擇用部門清單>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spDepartment_GetListToSelect
as
begin
	select
		DeptId, DeptName
	from dbo.Department
	order by SortNo
end
go



/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/07>
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
