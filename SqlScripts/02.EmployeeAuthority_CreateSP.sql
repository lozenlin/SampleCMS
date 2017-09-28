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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/25>
-- Description: <取得後端使用者清單(test)>
-- Test:
/*
declare @RowCount int
exec dbo.spEmployee_GetList 0, '', 2, 1, 99999, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
create procedure dbo.spEmployee_GetList
@DeptId int=0,
@SearchName nvarchar(50)='',
@ListMode int=2,--清單內容模式(0:正常, 1:已停權, 2:全部)
@BeginNum int,
@EndNum int,
@SortField nvarchar(20)='',
@IsSortDesc bit=0,
@RowCount int output
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
		if @DeptId=-1
		begin
			set @conditions += N' and e.DeptId is null '
		end
		else
		begin
			set @conditions += N' and e.DeptId=@DeptId '
		end
	end
	
	if @SearchName<>N''
	begin
		set @conditions += N' and (e.EmpAccount like @SearchName or e.EmpName like @SearchName) '
	end
	
	if @ListMode<>2
	begin
		if @ListMode=0
		begin
			set @conditions += N' and e.IsAccessDenied=0 '
		end
		else if @ListMode=1
		begin
			set @conditions += N' and e.IsAccessDenied=1 '
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
,@SearchName nvarchar(50)
,@ListMode int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @SearchName = N'%'+@SearchName+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@DeptId=@DeptId, 
		@SearchName=@SearchName, 
		@ListMode=@ListMode, 
		@RowCount=@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in ('RoleId')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by case when DeptId is null then 2 else 1 end, DeptId, RoleSortNo, EmpName'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			d.DeptName, e.DeptId, e.EmpId, 
			e.EmpAccount, e.EmpName, 
			e.Email, e.Remarks, e.RoleId, 
			r.SortNo as RoleSortNo, 
			r.RoleName, r.RoleDisplayName, e.IsAccessDenied, 
			oe.EmpName as OwnerName, e.OwnerAccount, e.PostDate, 
			e.MdfAccount, e.MdfDate, oe.DeptId as OwnerDeptId, 
			e.StartDate, e.EndDate
		from dbo.Employee e
			join dbo.EmployeeRole r on e.RoleId=r.RoleId
			left join dbo.Department d on e.DeptId=d.DeptId
			left join dbo.Employee oe on e.OwnerAccount=oe.EmpAccount
		where 1=1' + @conditions + N' ) main 
) result 
where RowNum between @BeginNum and @EndNum 
order by RowNum'

	set @parmDef += N'
,@BeginNum int
,@EndNum int
'
	exec sp_executesql @sql, @parmDef, 
		@DeptId=@DeptId, 
		@SearchName=@SearchName, 
		@ListMode=@ListMode, 
		@BeginNum=@BeginNum,
		@EndNum=@EndNum
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



/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/28>
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
