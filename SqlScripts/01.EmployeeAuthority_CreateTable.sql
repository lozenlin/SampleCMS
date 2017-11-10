-- Employee and Authority tables
use SampleCMS
go

----------------------------------------------------------------------------
-- dbo.Department 部門資料
----------------------------------------------------------------------------
create table dbo.Department(
	DeptId	int	Not Null	identity primary key
	,DeptName	nvarchar(50)	Not Null
	,SortNo	int		
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
)
go

--部門名稱設定唯一值
create unique index IX_Department on dbo.Department(DeptName)
go

--預設內容
set identity_insert dbo.Department on
insert into dbo.Department(DeptId, DeptName)
values(1, 'Default')

set identity_insert dbo.Department off
go

----------------------------------------------------------------------------
-- dbo.EmployeeRole 員工身分
----------------------------------------------------------------------------
create table dbo.EmployeeRole(
	RoleId	int	Not Null	identity primary key
	,RoleName	nvarchar(20)	Not Null
	,RoleDisplayName	nvarchar(20)		
	,SortNo	int		
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
)
go

--身分名稱設定唯一值
create unique index IX_EmployeeRole on dbo.EmployeeRole(RoleName)
go

--預設內容
set identity_insert dbo.EmployeeRole on
insert into dbo.EmployeeRole(
	RoleId, RoleName, RoleDisplayName, SortNo)
values(
	1, 'admin', '系統管理員', 10)
go
insert into dbo.EmployeeRole(
	RoleId, RoleName, RoleDisplayName, SortNo)
values(
	2, 'user', '使用者', 30)
go

set identity_insert dbo.EmployeeRole off
go

----------------------------------------------------------------------------
-- dbo.Employee 員工資料
----------------------------------------------------------------------------
create table dbo.Employee(
	EmpId	int	Not Null	identity primary key
	,EmpAccount	varchar(20)	Not Null	
	,EmpPassword	varchar(128)	Not Null	
	,EmpName	nvarchar(50)		
	,Email	varchar(100)		
	,Remarks	nvarchar(200)		
	,DeptId	int		
	,RoleId	int		
	,IsAccessDenied	bit	Not Null	Default(0)
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
	,StartDate	datetime		
	,EndDate	datetime		
	,OwnerAccount	varchar(20)		
	,IsEmailConfirmed	bit	Not Null	Default(0)
	,EmailConfirmKey	varchar(255)		
	,PasswordHashed	bit	Not Null	Default(0)
	,EmailConfirmKeyDate	datetime		
	,PasswordResetKey	varchar(255)		
	,PasswordResetKeyDate	datetime		
	,DefaultRandomPassword	varchar(50)		
)
go

--	2017/10/31, lozen_lin, modify, 新增欄位「本次登入時間,本次登入IP,上次登入時間,上次登入IP」
alter table dbo.Employee add ThisLoginTime	datetime
alter table dbo.Employee add ThisLoginIP	varchar(50)
alter table dbo.Employee add LastLoginTime	datetime
alter table dbo.Employee add LastLoginIP	varchar(50)
go

-- foreign key
alter table dbo.Employee  with check add constraint FK_Employee_Department foreign key(DeptId)
references dbo.Department(DeptId)
go
alter table dbo.Employee check constraint FK_Employee_Department
go

alter table dbo.Employee  with check add constraint FK_Employee_EmployeeRole foreign key(RoleId)
references dbo.EmployeeRole(RoleId)
go
alter table dbo.Employee check constraint FK_Employee_EmployeeRole
go

--帳號設定唯一值
create unique index IX_Employee on dbo.Employee(EmpAccount)
go

--預設內容
set identity_insert dbo.Employee on
insert into dbo.Employee(
	EmpId, EmpAccount, EmpPassword, 
	EmpName, DeptId, RoleId)
values(
	1, 'admin', 'admin', 
	N'管理者', 1, 1)
go
insert into dbo.Employee(
	EmpId, EmpAccount, EmpPassword, 
	EmpName, DeptId, RoleId, 
	StartDate, EndDate, OwnerAccount)
values(
	2, 'tester', 'tester', 
	N'測試人員', 1, 2, 
	'2017-11-05', '2017-11-05', 'admin')
go
set identity_insert dbo.Employee off
go

-- StartDate, EndDate is today
update dbo.Employee
set StartDate=convert(varchar, getdate(), 111)
where StartDate is null
go

update dbo.Employee
set EndDate=convert(varchar, getdate(), 111)
where EndDate is null
go

----------------------------------------------------------------------------
-- dbo.Operations 網頁後端作業選項
----------------------------------------------------------------------------
create table dbo.Operations(
	OpId	int	Not Null	identity primary key
	,ParentId	int		
	,OpSubject	nvarchar(100)		
	,LinkUrl	nvarchar(100)		
	,IsNewWindow	bit	Not Null	Default(0)
	,IconImageFile	nvarchar(255)		
	,SortNo	int		
	,IsHideSelf	bit	Not Null	Default(0)
	,CommonClass	varchar(100)		
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
)
go

--反查OpId用
create index IX_Operations_LinkUrl on dbo.Operations(LinkUrl)
go

create index IX_Operations_CommonClass on dbo.Operations(CommonClass)
go

--預設內容 (todo by lozen)
set identity_insert dbo.Operations on
insert dbo.Operations (OpId, ParentId, OpSubject, LinkUrl, IsNewWindow, IconImageFile, SortNo, IsHideSelf, CommonClass, PostAccount, PostDate, MdfAccount, MdfDate)
 values (1, null, N'系統管理功能', N'', 0, N'vectory_mini/basic/028.png', 10, 0, '', 'admin', getdate(), null, null)
insert dbo.Operations (OpId, ParentId, OpSubject, LinkUrl, IsNewWindow, IconImageFile, SortNo, IsHideSelf, CommonClass, PostAccount, PostDate, MdfAccount, MdfDate)
 values (2, 1, N'帳號管理', N'Account-List.aspx', 0, N'vectory_mini/basic/028.png', 10, 0, 'AccountCommonOfBackend', 'admin', getdate(), null, null)
insert dbo.Operations (OpId, ParentId, OpSubject, LinkUrl, IsNewWindow, IconImageFile, SortNo, IsHideSelf, CommonClass, PostAccount, PostDate, MdfAccount, MdfDate)
 values (3, 1, N'身分權限管理', N'Role-List.aspx', 0, N'vectory_mini/basic/028.png', 10, 0, 'RoleCommonOfBackend', 'admin', getdate(), null, null)
insert dbo.Operations (OpId, ParentId, OpSubject, LinkUrl, IsNewWindow, IconImageFile, SortNo, IsHideSelf, CommonClass, PostAccount, PostDate, MdfAccount, MdfDate)
 values (4, 1, N'部門管理', N'Department-List.aspx', 0, N'vectory_mini/basic/028.png', 10, 0, 'DepartmentCommonOfBackend', 'admin', getdate(), null, null)

set identity_insert dbo.Operations off
go

----------------------------------------------------------------------------
-- dbo.EmployeeRoleOperationsDesc 員工身分後端作業授權關聯表
----------------------------------------------------------------------------
create table dbo.EmployeeRoleOperationsDesc(
	RoleName	nvarchar(20)	Not Null	
	,OpId	int	Not Null	
	,CanRead	bit	Not Null	Default(0)
	,CanEdit	bit	Not Null	Default(0)
	,CanReadSubItemOfSelf	bit	Not Null	Default(0)
	,CanEditSubItemOfSelf	bit	Not Null	Default(0)
	,CanAddSubItemOfSelf	bit	Not Null	Default(0)
	,CanDelSubItemOfSelf	bit	Not Null	Default(0)
	,CanReadSubItemOfCrew	bit	Not Null	Default(0)
	,CanEditSubItemOfCrew	bit	Not Null	Default(0)
	,CanDelSubItemOfCrew	bit	Not Null	Default(0)
	,CanReadSubItemOfOthers	bit	Not Null	Default(0)
	,CanEditSubItemOfOthers	bit	Not Null	Default(0)
	,CanDelSubItemOfOthers	bit	Not Null	Default(0)
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
	,constraint PK_EmployeeRoleOperationsDesc primary key clustered(RoleName, OpId)
)
go

-- foreign key
alter table dbo.EmployeeRoleOperationsDesc with check add constraint FK_EmployeeRoleOperationsDesc_EmployeeRole foreign key(RoleName)
references dbo.EmployeeRole(RoleName)
go
alter table dbo.EmployeeRoleOperationsDesc check constraint FK_EmployeeRoleOperationsDesc_EmployeeRole
go

alter table dbo.EmployeeRoleOperationsDesc with check add constraint FK_EmployeeRoleOperationsDesc_Operations foreign key(OpId)
references dbo.Operations(OpId)
go
alter table dbo.EmployeeRoleOperationsDesc check constraint FK_EmployeeRoleOperationsDesc_Operations
go

--預設內容(todo by lozen)
go

----------------------------------------------------------------------------
-- dbo.BackEndLog 後端操作記錄
----------------------------------------------------------------------------
create table dbo.BackEndLog(
	Seqno	int	Not Null	identity primary key
	,EmpAccount	varchar(20)		
	,Description	nvarchar(4000)		
	,OpDate	datetime		
	,IP	varchar(50)		
)
go

--加快查詢速度用
create index IX_BackEndLog_OpDate on dbo.BackEndLog( OpDate )
	include(EmpAccount, IP) with (fillfactor=70)
go

