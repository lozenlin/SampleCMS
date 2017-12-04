-- Employee and Authority SP, DB Functions
use SampleCMS
go

----------------------------------------------------------------------------
-- 員工資料
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/13>
-- Test:
-- Description: <取得員工登入用資料>
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
-- Description: <取得員工資料>
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
		e.MdfDate, e.StartDate, e.EndDate, 
		e.ThisLoginTime, e.ThisLoginIP, e.LastLoginTime,
		e.LastLoginIP, e.OwnerAccount, isnull(oe.DeptId, 0) as OwnerDeptId,
		e.PasswordHashed, e.DefaultRandomPassword, isnull(r.RoleDisplayName, N'') + N' (' + isnull(r.RoleName, N'') + N')' as RoleDisplayText
	from dbo.Employee e
		join dbo.EmployeeRole r on e.RoleId=r.RoleId
		left join dbo.Department d on e.DeptId=d.DeptId
		left join dbo.Employee oe on e.OwnerAccount=oe.EmpAccount
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
-- History:
--	2017/11/08, lozen_lin, modify, 增加權限判斷用的參數
--	2017/11/14, lozen_lin, modify, 修正 EndDate 判斷
-- Description: <取得員工清單>
-- Test:
/*
declare @RowCount int
exec dbo.spEmployee_GetList 0, '', 0, 1, 99999, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount

declare @RowCount int
exec dbo.spEmployee_GetList 0, '', 0, 1, 99999, '', 0, 0, 0, 1, 'tester', 0, @RowCount output
select @RowCount
*/
-- =============================================
alter procedure dbo.spEmployee_GetList
@DeptId int=0	-- 0:all
,@Kw nvarchar(52)=''
,@ListMode int=0	--清單內容模式(0:all, 1:normal, 2:access is denied)
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@CanReadSubItemOfOthers bit=1	--可閱讀任何人的子項目
,@CanReadSubItemOfCrew bit=1	--可閱讀同部門的子項目
,@CanReadSubItemOfSelf bit=1	--可閱讀自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
,@RowCount int output
as
begin
	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)

	--條件定義
	set @conditions=N''

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and e.OwnerAccount=@MyAccount
	or e.EmpAccount=@MyAccount) '

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
			set @conditions += N' and e.IsAccessDenied=0 and (r.RoleName=''admin'' or (e.StartDate <= getdate() and getdate() < e.EndDate+1))'
		end
		else if @ListMode=2
		begin
			set @conditions += N' and e.IsAccessDenied=1 or not (r.RoleName=''admin'' or (e.StartDate <= getdate() and getdate() < e.EndDate+1))'
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
,@CanReadSubItemOfOthers bit
,@CanReadSubItemOfCrew bit
,@CanReadSubItemOfSelf bit
,@MyAccount varchar(20)
,@MyDeptId int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @Kw = N'%'+@Kw+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@DeptId
		,@Kw
		,@ListMode
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'DeptName', N'RoleSortNo', N'EmpName', N'EmpAccount', N'StartDate', N'OwnerName')
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
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@BeginNum
		,@EndNum
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/10/27>
-- Description: <取得員工身分名稱>
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
-- Description: <更新員工本次登入資訊>
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
-- Description: <刪除員工資料>
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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/07>
-- Description: <新增員工資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployee_InsertData
@EmpAccount	varchar(20)
,@EmpPassword	varchar(128)
,@EmpName	nvarchar(50)
,@Email	varchar(100)
,@Remarks	nvarchar(200)
,@DeptId	int
,@RoleId	int
,@IsAccessDenied	bit
,@StartDate	datetime
,@EndDate	datetime
,@OwnerAccount	varchar(20)
,@PasswordHashed	bit
,@DefaultRandomPassword	varchar(50)
,@PostAccount	varchar(20)
,@EmpId	int output
as
begin
	if exists(select * from dbo.Employee where EmpAccount=@EmpAccount)
	begin
		raiserror(N'EmpAccount has been used.', 11, 2)
		return
	end

	insert into dbo.Employee(
		EmpAccount, EmpPassword
		,EmpName, Email, Remarks
		,DeptId, RoleId, IsAccessDenied
		,StartDate, EndDate, OwnerAccount
		,PasswordHashed, DefaultRandomPassword, PostAccount
		,PostDate
		)
	values(
		@EmpAccount, @EmpPassword
		,@EmpName, @Email, @Remarks
		,@DeptId, @RoleId, @IsAccessDenied
		,@StartDate, @EndDate, @OwnerAccount
		,@PasswordHashed, @DefaultRandomPassword, @PostAccount
		,getdate()
		)

	set @EmpId=scope_identity()
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/07>
-- Description: <更新員工者資料>
-- Test:
/*
*/
-- =============================================
creaet procedure dbo.spEmployee_UpdateData
@EmpId	int
,@EmpPassword	varchar(128)
,@EmpName	nvarchar(50)
,@Email	varchar(100)
,@Remarks	nvarchar(200)
,@DeptId	int
,@RoleId	int
,@IsAccessDenied	bit
,@StartDate	datetime
,@EndDate	datetime
,@OwnerAccount	varchar(20)
,@PasswordHashed	bit
,@DefaultRandomPassword	varchar(50)
,@MdfAccount	varchar(20)
as
begin
	update dbo.Employee
	set
		EmpPassword=@EmpPassword
		,EmpName=@EmpName
		,Email=@Email
		,Remarks=@Remarks
		,DeptId=@DeptId
		,RoleId=@RoleId
		,IsAccessDenied=@IsAccessDenied
		,StartDate=@StartDate
		,EndDate=@EndDate
		,OwnerAccount=@OwnerAccount
		,PasswordHashed=@PasswordHashed
		,DefaultRandomPassword=@DefaultRandomPassword
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/20>
-- Description: <取得後端操作記錄清單>
-- Test:
/*
declare @RowCount int
exec dbo.spBackEndLog_GetList '1901/1/1', '2017/11/21', '', 1, '', 1, N'', 1, 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
alter procedure dbo.spBackEndLog_GetList
@StartDate datetime
,@EndDate datetime
,@Account varchar(22)=''	-- '':全部
,@IsAccKw bit
,@IP varchar(52)=''	-- '':全部
,@IsIpHeadKw bit
,@DescKw nvarchar(102)=N''	-- '':全部
,@RangeMode int=0	-- 0:全部, 1:登入相關
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@CanReadSubItemOfOthers bit=1	--可閱讀任何人的子項目
,@CanReadSubItemOfCrew bit=1	--可閱讀同部門的子項目
,@CanReadSubItemOfSelf bit=1	--可閱讀自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
,@RowCount int output
as
begin
	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)

	--條件定義
	set @conditions=N' and l.OpDate between @StartDate and @EndDate '

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and l.EmpAccount=@MyAccount) '

	if @Account<>''
	begin
		if @IsAccKw=1
		begin
			set @conditions += N' and l.EmpAccount like @Account '
			set @Account = '%'+@Account+'%'
		end
		else
		begin
			set @conditions += N' and l.EmpAccount=@Account '
		end
	end

	if @IP<>''
	begin
		if @IsIpHeadKw=1
		begin
			set @conditions += N' and l.IP like @IP '
			set @IP = @IP+'%'
		end
		else
		begin
			set @conditions += N' and l.IP=@IP '
		end
	end

	if @DescKw<>''
	begin
		set @conditions += N' and l.Description like @DescKw '
		set @DescKw = N'%'+@DescKw+N'%'
	end

	if @RangeMode=1
	begin
		set @conditions += N' 
and (l.Description like N''%Logged in%''
or l.Description like N''%Logged out%''
or l.Description like N''%帳號登入驗證時發生異常錯誤%''
or l.Description like N''%帳號不存在%''
or l.Description like N''%密碼錯誤%''
or l.Description like N''%帳號停用%''
or l.Description like N''%帳號超出有效範圍%''
or l.Description like N''%帳號登入取得使用者資料時發生異常錯誤%''
) '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.BackEndLog l
	left join dbo.Employee e on l.EmpAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@StartDate datetime
,@EndDate datetime
,@Account varchar(22)
,@IP varchar(52)
,@DescKw nvarchar(102)
,@CanReadSubItemOfOthers bit
,@CanReadSubItemOfCrew bit
,@CanReadSubItemOfSelf bit
,@MyAccount varchar(20)
,@MyDeptId int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	exec sp_executesql @sql, @ParmDefForTotal,
		@StartDate
		,@EndDate
		,@Account
		,@IP
		,@DescKw 
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'OpDate', N'IP', N'EmpName', N'EmpAccount', N'Description')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by OpDate desc'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			l.EmpAccount, e.EmpName, l.Description, 
			l.OpDate, l.IP, e.DeptId
		from dbo.BackEndLog l
			left join dbo.Employee e on l.EmpAccount=e.EmpAccount
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
		@StartDate
		,@EndDate
		,@Account
		,@IP
		,@DescKw 
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@BeginNum
		,@EndNum
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
-- Create date: <2017/11/30>
-- Description: <用超連結網址取得後端作業選項資訊>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spOperations_GetOpInfoByLinkUrl
@LinkUrl nvarchar(100)
as
begin
	select top 1
		o.OpId
	from dbo.Operations o
	where o.LinkUrl=@LinkUrl
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/10/31>
-- History:
--	2017/11/21, lozen_lin, modify, 新增欄位「英文標題」
-- Description: <取得後端作業選項第一層清單和身分授權>
-- Test:
/*
exec dbo.spOperations_GetTopListWithRoleAuth 'user'
*/
-- =============================================
alter procedure dbo.spOperations_GetTopListWithRoleAuth
@RoleName nvarchar(20)
as
begin
	select
		o.OpId, o.OpSubject, o.LinkUrl, 
		o.IsNewWindow, o.IconImageFile, 
		ro.CanRead, ro.CanEdit, ro.CanReadSubItemOfSelf, 
		ro.CanEditSubItemOfSelf, ro.CanAddSubItemOfSelf, ro.CanDelSubItemOfSelf, 
		ro.CanReadSubItemOfCrew, ro.CanEditSubItemOfCrew, ro.CanDelSubItemOfCrew, 
		ro.CanReadSubItemOfOthers, ro.CanEditSubItemOfOthers, ro.CanDelSubItemOfOthers, 
		o.EnglishSubject
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
-- History:
--	2017/11/21, lozen_lin, modify, 新增欄位「英文標題」
-- Description: <取得後端作業選項子清單和身分授權>
-- Test:
/*
exec dbo.spOperations_GetSubListWithRoleAuth 'user'
*/
-- =============================================
alter procedure dbo.spOperations_GetSubListWithRoleAuth
@RoleName nvarchar(20)
as
begin
	select
		o.OpId, o.ParentId, o.OpSubject, 
		o.LinkUrl, o.IsNewWindow, o.IconImageFile, 
		ro.CanRead, ro.CanEdit, ro.CanReadSubItemOfSelf, 
		ro.CanEditSubItemOfSelf, ro.CanAddSubItemOfSelf, ro.CanDelSubItemOfSelf, 
		ro.CanReadSubItemOfCrew, ro.CanEditSubItemOfCrew, ro.CanDelSubItemOfCrew, 
		ro.CanReadSubItemOfOthers, ro.CanEditSubItemOfOthers, ro.CanDelSubItemOfOthers, 
		o.EnglishSubject
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
-- History:
--	2017/11/21, lozen_lin, modify, 新增欄位「英文標題」
--	2017/11/22, lozen_lin, modify, 加上所有欄位
-- Description: <取得後端作業選項資料>
-- Test:
/*
exec dbo.spOperations_GetData 2
*/
-- =============================================
alter procedure dbo.spOperations_GetData
@OpId int
as
begin
	select
		o.OpId, o.ParentId, o.OpSubject, 
		o.LinkUrl, o.IsNewWindow, o.IconImageFile, 
		o.SortNo, o.IsHideSelf, o.CommonClass, 
		o.PostAccount, o.PostDate, o.MdfAccount, 
		o.MdfDate, e.EmpName as PostName, d.DeptName as PostDeptName, 
		o.EnglishSubject
	from dbo.Operations o
		left join dbo.Employee e on o.PostAccount=e.EmpAccount
		left join dbo.Department d on e.DeptId=d.DeptId
	where OpId=@OpId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/21>
-- Description: <取得後端作業選項清單>
-- Test:
/*
declare @RowCount int
exec dbo.spOperations_GetList 0, '', N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount

declare @RowCount int
exec dbo.spOperations_GetList 1, '', N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount

*/
-- =============================================
alter procedure dbo.spOperations_GetList
@ParentId int=0	-- 0:root
,@CultureName varchar(10)
,@Kw nvarchar(52)=''
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@CanReadSubItemOfOthers bit=1	--可閱讀任何人的子項目
,@CanReadSubItemOfCrew bit=1	--可閱讀同部門的子項目
,@CanReadSubItemOfSelf bit=1	--可閱讀自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
,@RowCount int output
as
begin
	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)

	--條件定義
	set @conditions=N''
	
	if @ParentId=0
	begin
		set @conditions += N' and o.ParentId is null '
	end
	else
	begin
		set @conditions += N' and o.ParentId=@ParentId '
	end

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and o.PostAccount=@MyAccount) '
	
	if @Kw<>N''
	begin
		set @conditions += N' and (o.OpSubject like @Kw or o.EnglishSubject like @Kw) '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.Operations o
	left join dbo.Employee e on o.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@ParentId int
,@CultureName varchar(10)
,@Kw nvarchar(52)
,@CanReadSubItemOfOthers bit
,@CanReadSubItemOfCrew bit
,@CanReadSubItemOfSelf bit
,@MyAccount varchar(20)
,@MyDeptId int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @Kw = N'%'+@Kw+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@ParentId
		,@CultureName
		,@Kw
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'Subject', N'IsNewWindow', N'CommonClass', N'SortNo')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by SortNo'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			o.OpId, o.ParentId, o.OpSubject
			,o.LinkUrl, o.IsNewWindow, o.IconImageFile
			,o.SortNo, o.IsHideSelf, o.CommonClass
			,o.PostAccount, o.PostDate, o.MdfAccount
			,o.MdfDate, o.EnglishSubject, isnull(e.DeptId, 0) as PostDeptId, 
			case when @CultureName=''en'' then o.EnglishSubject else o.OpSubject end as Subject
		from dbo.Operations o
			left join dbo.Employee e on o.PostAccount=e.EmpAccount
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
		@ParentId
		,@CultureName
		,@Kw
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@BeginNum
		,@EndNum
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/21>
-- History:
--	2017/11/23, lozen_lin, modify, 刪除授權設定
-- Description: <刪除後端作業選項>
-- Test:
/*
*/
-- =============================================
alter procedure dbo.spOperations_DeleteData
@OpId int
as
begin
	if exists(select * from dbo.Operations where ParentId=@OpId)
	begin
		raiserror(N'此作業選項有子項目,不允許刪除', 11, 2)
		return
	end

	begin transaction
	begin try
		--先刪除授權設定
		delete dbo.EmployeeRoleOperationsDesc
		where OpId=@OpId

		delete from dbo.Operations
		where OpId=@OpId

		commit transaction
	end try
	begin catch
		if xact_state()<>0
		begin
			rollback transaction
		end

		--forward error message
		declare @errMessage nvarchar(4000)
		declare @errSeverity int
		declare @errState int

		set @errMessage=error_message()
		set @errSeverity=error_severity()
		set @errState=error_state()

		raiserror(@errMessage, @errSeverity, @errState)
	end catch
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/21>
-- Description: <加大後端作業選項的排序編號>
-- Test:
/*
exec dbo.spOperations_IncreaseSortNo 2, 'admin'
*/
-- =============================================
create procedure dbo.spOperations_IncreaseSortNo
@OpId int
,@MdfAccount varchar(20)
as
begin
	declare @ParentId int
	declare @SortNo int

	select
		@ParentId=ParentId, @SortNo=SortNo
	from dbo.Operations
	where OpId=@OpId

	if @SortNo is null
	begin
		set @SortNo=0
	end

	-- get bigger one
	declare @BiggerSortNo int
	declare @BiggerOpId int

	select top 1
		@BiggerSortNo=SortNo, @BiggerOpId=OpId
	from dbo.Operations
	where (ParentId=@ParentId or @ParentId is null and ParentId is null)
		and OpId<>@OpId
		and SortNo>=@SortNo
	order by SortNo

	-- there is no bigger one, exit
	if @BiggerOpId is null
	begin
		return
	end

	if @BiggerSortNo is null
	begin
		set @BiggerSortNo=0
	end

	-- when the values are the same
	if @SortNo=@BiggerSortNo
	begin
		set @BiggerSortNo += 1
	end

	-- swap
	update dbo.Operations
	set SortNo=@BiggerSortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where OpId=@OpId

	update dbo.Operations
	set SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where OpId=@BiggerOpId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/21>
-- Description: <減小後端作業選項的排序編號>
-- Test:
/*
exec dbo.spOperations_DecreaseSortNo 2, 'admin'
*/
-- =============================================
create procedure dbo.spOperations_DecreaseSortNo
@OpId int
,@MdfAccount varchar(20)
as
begin
	declare @ParentId int
	declare @SortNo int

	select
		@ParentId=ParentId, @SortNo=SortNo
	from dbo.Operations
	where OpId=@OpId

	if @SortNo is null
	begin
		set @SortNo=0
	end

	-- get smaller one
	declare @SmallerSortNo int
	declare @SmallerOpId int

	select top 1
		@SmallerSortNo=SortNo, @SmallerOpId=OpId
	from dbo.Operations
	where (ParentId=@ParentId or @ParentId is null and ParentId is null)
		and OpId<>@OpId
		and SortNo<=@SortNo
	order by SortNo desc

	-- there is no smaller one, exit
	if @SmallerOpId is null
	begin
		return
	end

	if @SmallerSortNo is null
	begin
		set @SmallerSortNo=0
	end

	-- when the values are the same
	if @SortNo=@SmallerSortNo
	begin
		set @SortNo += 1
	end

	-- swap
	update dbo.Operations
	set SortNo=@SmallerSortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where OpId=@OpId

	update dbo.Operations
	set SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where OpId=@SmallerOpId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/22>
-- History:
--	2017/11/23, lozen_lin, modify, add IconImageFile
-- Description: <取得後端作業選項階層資訊>
-- Test:
/*
exec dbo.spOperations_GetLevelInfo 1
exec dbo.spOperations_GetLevelInfo 2
*/
-- =============================================
alter procedure dbo.spOperations_GetLevelInfo
@OpId int
as
begin
	create table #tbl(
		LevelNum int
		,OpId int
		,OpSubject nvarchar(100)
		,EnglishSubject nvarchar(100)
		,IconImageFile nvarchar(255)
	)

	declare @LevelNum int=0
	declare @CurOpId int=@OpId
	declare @ParentId int=0
	declare @OpSubject nvarchar(100)
	declare @EnglishSubject nvarchar(100)
	declare @IconImageFile nvarchar(255)

	while 1=1
	begin
		select
			@ParentId=ParentId, @OpSubject=OpSubject, @EnglishSubject=EnglishSubject, 
			@IconImageFile=IconImageFile
		from dbo.Operations
		where OpId=@CurOpId

		set @LevelNum -= 1
		
		insert into #tbl(
			LevelNum, OpId, OpSubject, 
			EnglishSubject, IconImageFile)
		values(
			@LevelNum, @CurOpId, @OpSubject,
			@EnglishSubject, @IconImageFile
			)

		if @ParentId is null
		begin
			break
		end

		set @CurOpId=@ParentId
	end

	set @LevelNum = abs(@LevelNum)

	update #tbl
	set LevelNum += @LevelNum+1

	select * from #tbl
	order by LevelNum desc

	drop table #tbl
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/22>
-- Description: <取得後端作業選項最大排序編號>
-- Test:
/*
exec dbo.spOperations_GetMaxSortNo 0
exec dbo.spOperations_GetMaxSortNo 1
*/
-- =============================================
create procedure dbo.spOperations_GetMaxSortNo
@ParentId int=0	-- 0:root
as
begin
	select
		isnull(max(SortNo), 0) as MaxSortNo
	from dbo.Operations
	where (@ParentId=0 and ParentId is null
		or ParentId=@ParentId)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/22>
-- Description: <新增後端作業選項>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spOperations_InsertData
@ParentId	int	-- 0: set null value
,@OpSubject	nvarchar(100)
,@LinkUrl	nvarchar(100)
,@IsNewWindow	bit
,@IconImageFile	nvarchar(255)
,@SortNo	int
,@IsHideSelf	bit
,@CommonClass	varchar(100)
,@PostAccount	varchar(20)
,@EnglishSubject	nvarchar(100)
,@OpId	int output
as
begin
	if @ParentId=0
	begin
		set @ParentId = null
	end

	insert into dbo.Operations(
		ParentId, OpSubject, LinkUrl, 
		IsNewWindow, IconImageFile, SortNo, 
		IsHideSelf, CommonClass, PostAccount, 
		PostDate, EnglishSubject
		)
	values(
		@ParentId, @OpSubject, @LinkUrl, 
		@IsNewWindow, @IconImageFile, @SortNo, 
		@IsHideSelf, @CommonClass, @PostAccount, 
		getdate(), @EnglishSubject
		)

	set @OpId = scope_identity()
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/22>
-- Description: <更新後端作業選項>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spOperations_UpdateData
@OpId	int
,@OpSubject	nvarchar(100)
,@LinkUrl	nvarchar(100)
,@IsNewWindow	bit
,@IconImageFile	nvarchar(255)
,@SortNo	int
,@IsHideSelf	bit
,@CommonClass	varchar(100)
,@MdfAccount	varchar(20)
,@EnglishSubject	nvarchar(100)
as
begin
	update dbo.Operations
	set
		OpSubject=@OpSubject
		,LinkUrl=@LinkUrl
		,IsNewWindow=@IsNewWindow
		,IconImageFile=@IconImageFile
		,SortNo=@SortNo
		,IsHideSelf=@IsHideSelf
		,CommonClass=@CommonClass
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
		,EnglishSubject=@EnglishSubject
	where OpId=@OpId
end
go

----------------------------------------------------------------------------
-- 員工身分後端作業授權相關
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/09/28>
-- Description: <取得指定作業代碼的後端身分可使用權限>
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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/15>
-- Description: <儲存員工身分後端作業授權>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployeeRoleOperationsDesc_SaveData
@RoleName	nvarchar(20)
,@OpId	int
,@CanRead	bit
,@CanEdit	bit
,@CanReadSubItemOfSelf	bit
,@CanEditSubItemOfSelf	bit
,@CanAddSubItemOfSelf	bit
,@CanDelSubItemOfSelf	bit
,@CanReadSubItemOfCrew	bit
,@CanEditSubItemOfCrew	bit
,@CanDelSubItemOfCrew	bit
,@CanReadSubItemOfOthers	bit
,@CanEditSubItemOfOthers	bit
,@CanDelSubItemOfOthers	bit
,@PostAccount	varchar(20)
as
begin
	if exists(select * from dbo.EmployeeRoleOperationsDesc where RoleName=@RoleName and OpId=@OpId)
	begin
		update dbo.EmployeeRoleOperationsDesc
		set
			CanRead=@CanRead
			,CanEdit=@CanEdit
			,CanReadSubItemOfSelf=@CanReadSubItemOfSelf
			,CanEditSubItemOfSelf=@CanEditSubItemOfSelf
			,CanAddSubItemOfSelf=@CanAddSubItemOfSelf
			,CanDelSubItemOfSelf=@CanDelSubItemOfSelf
			,CanReadSubItemOfCrew=@CanReadSubItemOfCrew
			,CanEditSubItemOfCrew=@CanEditSubItemOfCrew
			,CanDelSubItemOfCrew=@CanDelSubItemOfCrew
			,CanReadSubItemOfOthers=@CanReadSubItemOfOthers
			,CanEditSubItemOfOthers=@CanEditSubItemOfOthers
			,CanDelSubItemOfOthers=@CanDelSubItemOfOthers
			,MdfAccount=@PostAccount
			,MdfDate=getdate()
		where RoleName=@RoleName and OpId=@OpId
	end
	else
	begin
		insert into dbo.EmployeeRoleOperationsDesc(
			RoleName, OpId, CanRead, 
			CanEdit, CanReadSubItemOfSelf, CanEditSubItemOfSelf, 
			CanAddSubItemOfSelf, CanDelSubItemOfSelf, CanReadSubItemOfCrew, 
			CanEditSubItemOfCrew, CanDelSubItemOfCrew, CanReadSubItemOfOthers, 
			CanEditSubItemOfOthers, CanDelSubItemOfOthers, PostAccount, 
			PostDate
			)
		values(
			@RoleName, @OpId, @CanRead, 
			@CanEdit, @CanReadSubItemOfSelf, @CanEditSubItemOfSelf, 
			@CanAddSubItemOfSelf, @CanDelSubItemOfSelf, @CanReadSubItemOfCrew, 
			@CanEditSubItemOfCrew, @CanDelSubItemOfCrew, @CanReadSubItemOfOthers, 
			@CanEditSubItemOfOthers, @CanDelSubItemOfOthers, @PostAccount, 
			getdate()
			)
	end
end
go


----------------------------------------------------------------------------
-- 員工身分
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/07>
-- Description: <取得選擇用員工身分清單>
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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/10>
-- History:
--	2017/11/15, lozen_lin, modify, 增加權限判斷用的參數
--	2017/11/21, lozen_lin, modify, 修正權限判斷
-- Description: <取得員工身分清單>
-- Test:
/*
declare @RowCount int
exec dbo.spEmployeeRole_GetList N'', 1, 20, N'', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
alter procedure dbo.spEmployeeRole_GetList
@Kw nvarchar(52)=''
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@CanReadSubItemOfOthers bit=1	--可閱讀任何人的子項目
,@CanReadSubItemOfCrew bit=1	--可閱讀同部門的子項目
,@CanReadSubItemOfSelf bit=1	--可閱讀自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
,@RowCount int output
as
begin
	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)

	--條件定義
	set @conditions=N''
	
	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and r.PostAccount=@MyAccount) '

	if @Kw<>N''
	begin
		set @conditions += N' and (r.RoleName like @Kw or r.RoleDisplayName like @Kw) '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.EmployeeRole r
	left join dbo.Employee e on r.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@Kw nvarchar(52)
,@CanReadSubItemOfOthers bit
,@CanReadSubItemOfCrew bit
,@CanReadSubItemOfSelf bit
,@MyAccount varchar(20)
,@MyDeptId int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @Kw = N'%'+@Kw+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@Kw
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'RoleName', N'RoleDisplayName', N'SortNo', N'EmpTotal')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by SortNo'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			r.RoleId, r.RoleName, r.RoleDisplayName,
			r.SortNo, r.PostAccount, isnull(e.DeptId, 0) as PostDeptId,
			(select count(*) from dbo.Employee where RoleId=r.RoleId) as EmpTotal
		from dbo.EmployeeRole r
			left join dbo.Employee e on r.PostAccount=e.EmpAccount
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
		@Kw
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@BeginNum
		,@EndNum
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/10>
-- Description: <刪除員工身分>
-- Test:
/*
exec dbo.spEmployeeRole_DeleteData 2
*/
-- =============================================
alter procedure dbo.spEmployeeRole_DeleteData
@RoleId int
as
begin
	-- 檢查帳號總數
	if exists(select * from dbo.Employee where RoleId=@RoleId)
	begin
		raiserror(N'身分已有帳號使用,不允許刪除', 11, 2)
		return
	end

	begin transaction
	begin try
		--先刪除授權設定
		delete dbo.EmployeeRoleOperationsDesc
		where RoleName=(select RoleName from dbo.EmployeeRole where RoleId=@RoleId)

		delete dbo.EmployeeRole
		where RoleId=@RoleId

		commit transaction
	end try
	begin catch
		if xact_state()<>0
		begin
			rollback transaction
		end

		--forward error message
		declare @errMessage nvarchar(4000)
		declare @errSeverity int
		declare @errState int

		set @errMessage=error_message()
		set @errSeverity=error_severity()
		set @errState=error_state()

		raiserror(@errMessage, @errSeverity, @errState)
	end catch
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/11>
-- Description: <取得員工身分最大排序編號>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployeeRole_GetMaxSortNo
as
begin
	select isnull(max(SortNo), 0) as MaxSortNo
	from dbo.EmployeeRole
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/11>
-- Description: <取得員工身分資料>
-- Test:
/*
*/
-- =============================================
alter procedure dbo.spEmployeeRole_GetData
@RoleId int
as
begin
	select
		r.RoleId, r.RoleName, r.RoleDisplayName
		,r.SortNo, r.PostAccount, r.PostDate
		,r.MdfAccount, r.MdfDate, isnull(e.DeptId, 0) as PostDeptId
	from dbo.EmployeeRole r
		left join dbo.Employee e on r.PostAccount=e.EmpAccount
	where r.RoleId=@RoleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/11>
-- Description: <新增員工身分資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployeeRole_InsertData
@RoleName	nvarchar(20)
,@RoleDisplayName	nvarchar(20)
,@SortNo	int
,@PostAccount	varchar(20)
,@CopyPrivilegeFromRoleName nvarchar(20)
,@RoleId	int output
as
begin
	if exists(select * from dbo.EmployeeRole where RoleName=@RoleName)
	begin
		raiserror(N'身分名稱已存在', 11, 2)
		return
	end

	insert into dbo.EmployeeRole(
		RoleName, RoleDisplayName, SortNo, 
		PostAccount, PostDate
		)
	values(
		@RoleName, @RoleDisplayName, @SortNo, 
		@PostAccount, getdate()
		)

	set @RoleId = scope_identity()

	-- copy privilege
	if @CopyPrivilegeFromRoleName<>''
	begin
		insert into dbo.EmployeeRoleOperationsDesc(
			RoleName, OpId, CanRead, 
			CanEdit, CanReadSubItemOfSelf, CanEditSubItemOfSelf, 
			CanAddSubItemOfSelf, CanDelSubItemOfSelf, CanReadSubItemOfCrew, 
			CanEditSubItemOfCrew, CanDelSubItemOfCrew, CanReadSubItemOfOthers, 
			CanEditSubItemOfOthers, CanDelSubItemOfOthers, PostAccount, 
			PostDate
			)
			select 
				@RoleName, OpId, CanRead, 
				CanEdit, CanReadSubItemOfSelf, CanEditSubItemOfSelf, 
				CanAddSubItemOfSelf, CanDelSubItemOfSelf, CanReadSubItemOfCrew, 
				CanEditSubItemOfCrew, CanDelSubItemOfCrew, CanReadSubItemOfOthers, 
				CanEditSubItemOfOthers, CanDelSubItemOfOthers, @PostAccount, 
				getdate()
			from dbo.EmployeeRoleOperationsDesc
			where RoleName=@CopyPrivilegeFromRoleName
	end
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/11>
-- Description: <更新員工身分資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spEmployeeRole_UpdateData
@RoleId	int
,@RoleDisplayName	nvarchar(20)
,@SortNo	int
,@MdfAccount	varchar(20)
as
begin
	update dbo.EmployeeRole
	set RoleDisplayName=@RoleDisplayName
		,SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where RoleId=@RoleId
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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/16>
-- History:
--	2017/11/21, lozen_lin, modify, 修正權限判斷
-- Description: <取得部門清單>
-- Test:
/*
declare @RowCount int
exec dbo.spDepartment_GetList N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
alter procedure dbo.spDepartment_GetList
@Kw nvarchar(52)=''
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@CanReadSubItemOfOthers bit=1	--可閱讀任何人的子項目
,@CanReadSubItemOfCrew bit=1	--可閱讀同部門的子項目
,@CanReadSubItemOfSelf bit=1	--可閱讀自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
,@RowCount int output
as
begin
	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)

	--條件定義
	set @conditions=N''
	
	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and d.PostAccount=@MyAccount) '

	if @Kw<>N''
	begin
		set @conditions += N' and d.DeptName like @Kw '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.Department d
	left join dbo.Employee e on d.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@Kw nvarchar(52)
,@CanReadSubItemOfOthers bit
,@CanReadSubItemOfCrew bit
,@CanReadSubItemOfSelf bit
,@MyAccount varchar(20)
,@MyDeptId int
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @Kw = N'%'+@Kw+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@Kw
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'DeptName', N'SortNo', N'EmpTotal')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by SortNo'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			d.DeptId, d.DeptName, d.SortNo, 
			d.PostAccount, isnull(e.DeptId, 0) as PostDeptId,
			(select count(*) from dbo.Employee where DeptId=d.DeptId) as EmpTotal
		from dbo.Department d
			left join dbo.Employee e on d.PostAccount=e.EmpAccount
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
		@Kw
		,@CanReadSubItemOfOthers
		,@CanReadSubItemOfCrew
		,@CanReadSubItemOfSelf
		,@MyAccount
		,@MyDeptId
		,@BeginNum
		,@EndNum
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/17>
-- Description: <刪除部門資料>
-- Test:
/*
exec dbo.spDepartment_DeleteData 1
*/
-- =============================================
create procedure dbo.spDepartment_DeleteData
@DeptId int
as
begin
	-- 檢查帳號總數
	if exists(select * from dbo.Employee where DeptId=@DeptId)
	begin
		raiserror(N'部門已有帳號使用,不允許刪除', 11, 2)
		return
	end

	delete dbo.Department
	where DeptId=@DeptId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/17>
-- Description: <取得部門資料>
-- Test:
/*
exec dbo.spDepartment_GetData 1
*/
-- =============================================
create procedure dbo.spDepartment_GetData
@DeptId int
as
begin
	select
		d.DeptId, d.DeptName, d.SortNo,
		d.PostAccount, d.PostDate, d.MdfAccount,
		d.MdfDate, isnull(e.DeptId, 0) as PostDeptId
	from dbo.Department d
		left join dbo.Employee e on d.PostAccount=e.EmpAccount
	where d.DeptId=@DeptId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/17>
-- Description: <取得部門最大排序編號>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spDepartment_GetMaxSortNo
as
begin
	select isnull(max(SortNo), 0) as MaxSortNo
	from dbo.Department
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/17>
-- Description: <新增部門資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spDepartment_InsertData
@DeptName	nvarchar(50)
,@SortNo	int
,@PostAccount	varchar(20)
,@DeptId	int output
as
begin
	if exists(select * from dbo.Department where DeptName=@DeptName)
	begin
		raiserror(N'部門名稱已存在', 11, 2)
		return
	end

	insert into dbo.Department(
		DeptName, SortNo, PostAccount,
		PostDate
		)
	values(
		@DeptName, @SortNo, @PostAccount,
		getdate()
		)

	set @DeptId=scope_identity()
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/17>
-- Description: <更新部門資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spDepartment_UpdateData
@DeptId	int
,@DeptName	nvarchar(50)
,@SortNo	int
,@MdfAccount	varchar(20)
as
begin
	if exists(select * from dbo.Department where DeptId<>@DeptId and DeptName=@DeptName)
	begin
		raiserror(N'部門名稱已存在', 11, 2)
		return
	end

	update dbo.Department
	set DeptName=@DeptName
		,SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where DeptId=@DeptId
end
go



/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
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
