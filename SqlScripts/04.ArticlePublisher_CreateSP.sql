-- Article publisher SP, DB Functions
use SampleCMS
go

-- db functions
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
-- Description: <網頁內容在指定語系是否顯示>
-- Test:
/*
select dbo.fnArticle_IsShowInLang('00000000-0000-0000-0000-000000000000', 'zh-TW')
*/
-- =============================================
create function dbo.fnArticle_IsShowInLang(
@ArticleId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select 
		@IsShowInLang=IsShowInLang
	from dbo.ArticleMultiLang
	where ArticleId=@ArticleId
		and CultureName=@CultureName

	return @IsShowInLang
end
go

-- sp
go
----------------------------------------------------------------------------
-- 網頁內容
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/29>
-- Description: <取得後台用網頁內容資料>
-- Test:
/*
*/
-- =============================================
alter procedure dbo.spArticle_GetDataForBackend
@ArticleId uniqueidentifier
as
begin
	select
		a.ParentId, a.ArticleLevelNo, a.ArticleAlias,
		a.BannerPicFileName, a.LayoutModeId, a.ShowTypeId, 
		a.LinkUrl, a.LinkTarget, a.ControlName, 
		a.SubItemControlName, a.IsHideSelf, a.IsHideChild, 
		a.StartDate, a.EndDate, a.SortNo, 
		a.DontDelete, a.PostAccount, a.PostDate, 
		a.MdfAccount, a.MdfDate, isnull(e.DeptId, 0) as PostDeptId
	from dbo.Article a
		join dbo.Employee e on a.PostAccount=e.EmpAccount
	where a.ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/29>
-- Description: <取得後台用網頁內容的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleMultiLang_GetDataForBackend
@ArticleId uniqueidentifier
,@CultureName varchar(10)
as
begin
	select
		am.ArticleSubject, am.ArticleContext, am.ReadCount, 
		am.IsShowInLang, am.PostAccount, am.PostDate, 
		am.MdfAccount, am.MdfDate
	from dbo.ArticleMultiLang am
	where am.ArticleId=@ArticleId
		and am.CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/29>
-- Description: <取得網頁內容最大排序編號>
-- Test:
/*
exec dbo.spArticle_GetMaxSortNo '00000000-0000-0000-0000-000000000000'
*/
-- =============================================
create procedure dbo.spArticle_GetMaxSortNo
@ParentId uniqueidentifier
as
begin
	select
		isnull(max(SortNo), 0) as MaxSortNo
	from dbo.Article
	where ParentId=@ParentId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- Description: <新增網頁內容>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticle_InsertData
@ArticleId	uniqueidentifier
,@ParentId	uniqueidentifier
,@ArticleAlias	varchar(50)
,@BannerPicFileName	nvarchar(255)
,@LayoutModeId	int
,@ShowTypeId	int
,@LinkUrl	nvarchar(2048)
,@LinkTarget	varchar(10)
,@ControlName	varchar(100)
,@SubItemControlName	varchar(100)
,@IsHideSelf	bit
,@IsHideChild	bit
,@StartDate	datetime
,@EndDate	datetime
,@SortNo	int
,@DontDelete	bit
,@PostAccount	varchar(20)
as
begin
	declare @ArticleLevelNo	int

	-- check id
	if exists(select * from dbo.Article where ArticleId=@ArticleId)
	begin
		raiserror(N'ArticleId has been used.', 11, 2)
		return
	end

	-- check alias
	if exists(select * from dbo.Article where ParentId=@ParentId and ArticleAlias=@ArticleAlias)
	begin
		raiserror(N'ArticleAlias has been used.', 11, 3)
		return
	end

	-- get parent  info
	select
		@ArticleLevelNo=ArticleLevelNo
	from dbo.Article
	where ArticleId=@ParentId

	set @ArticleLevelNo += 1

	insert into dbo.Article(
		ArticleId, ParentId, ArticleLevelNo, 
		ArticleAlias, BannerPicFileName, LayoutModeId, 
		ShowTypeId, LinkUrl, LinkTarget, 
		ControlName, SubItemControlName, IsHideSelf, 
		IsHideChild, StartDate, EndDate, 
		SortNo, DontDelete, PostAccount, 
		PostDate
		)
	values(
		@ArticleId, @ParentId, @ArticleLevelNo, 
		@ArticleAlias, @BannerPicFileName, @LayoutModeId, 
		@ShowTypeId, @LinkUrl, @LinkTarget, 
		@ControlName, @SubItemControlName, @IsHideSelf, 
		@IsHideChild, @StartDate, @EndDate, 
		@SortNo, @DontDelete, @PostAccount, 
		getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- Description: <新增網頁內容的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleMultiLang_InsertData
@ArticleId	uniqueidentifier
,@CultureName	varchar(10)
,@ArticleSubject	nvarchar(200)
,@ArticleContext	nvarchar(max)
,@IsShowInLang	bit
,@PostAccount	varchar(20)
as
begin
	insert into dbo.ArticleMultiLang(
		ArticleId, CultureName, ArticleSubject, 
		ArticleContext, IsShowInLang, PostAccount, 
		PostDate
		)
	values(
		@ArticleId, @CultureName, @ArticleSubject, 
		@ArticleContext, @IsShowInLang, @PostAccount, 
		getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- History:
--	2017/12/01, lozen_lin, modify, 修正別名誤判問題
-- Description: <更新網頁內容>
-- Test:
/*
*/
-- =============================================
alter procedure dbo.spArticle_UpdateData
@ArticleId	uniqueidentifier
,@ArticleAlias	varchar(50)
,@BannerPicFileName	nvarchar(255)
,@LayoutModeId	int
,@ShowTypeId	int
,@LinkUrl	nvarchar(2048)
,@LinkTarget	varchar(10)
,@ControlName	varchar(100)
,@SubItemControlName	varchar(100)
,@IsHideSelf	bit
,@IsHideChild	bit
,@StartDate	datetime
,@EndDate	datetime
,@SortNo	int
,@DontDelete	bit
,@MdfAccount	varchar(20)
as
begin
	declare @ParentId uniqueidentifier

	select 
		@ParentId=ParentId
	from dbo.Article
	where ArticleId=@ArticleId

	-- check alias
	if exists(select * from dbo.Article where ParentId=@ParentId and ArticleId<>@ArticleId and ArticleAlias=@ArticleAlias)
	begin
		raiserror(N'ArticleAlias has been used.', 11, 3)
		return
	end

	update dbo.Article
	set 
		ArticleAlias=@ArticleAlias
		,BannerPicFileName=@BannerPicFileName
		,LayoutModeId=@LayoutModeId
		,ShowTypeId=@ShowTypeId
		,LinkUrl=@LinkUrl
		,LinkTarget=@LinkTarget
		,ControlName=@ControlName
		,SubItemControlName=@SubItemControlName
		,IsHideSelf=@IsHideSelf
		,IsHideChild=@IsHideChild
		,StartDate=@StartDate
		,EndDate=@EndDate
		,SortNo=@SortNo
		,DontDelete=@DontDelete
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- Description: <更新網頁內容的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleMultiLang_UpdateData
@ArticleId	uniqueidentifier
,@CultureName	varchar(10)
,@ArticleSubject	nvarchar(200)
,@ArticleContext	nvarchar(max)
,@IsShowInLang	bit
,@MdfAccount	varchar(20)
as
begin
	update dbo.ArticleMultiLang
	set
		ArticleSubject=@ArticleSubject
		,ArticleContext=@ArticleContext
		,IsShowInLang=@IsShowInLang
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where ArticleId=@ArticleId
		and CultureName=@CultureName

end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
-- Description: <取得後台用指定語系的網頁內容清單>
-- Test:
/*
declare @RowCount int
exec dbo.spArticleMultiLang_GetListForBackend '00000000-0000-0000-0000-000000000000', 'zh-TW', N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
alter procedure dbo.spArticleMultiLang_GetListForBackend
@ParentId	uniqueidentifier
,@CultureName	varchar(10)
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
	set @conditions=N' and a.ParentId=@ParentId and am.CultureName=@CultureName '

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and am.PostAccount=@MyAccount) '
	
	if @Kw<>N''
	begin
		set @conditions += N' and am.ArticleSubject like @Kw '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.ArticleMultiLang am
	join dbo.Article a on am.ArticleId=a.ArticleId
	left join dbo.Employee e on am.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@ParentId	uniqueidentifier
,@CultureName	varchar(10)
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

	if @SortField in (N'ArticleSubject', N'SortNo', N'StartDate', N'PostDeptName')
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
			am.ArticleId, am.CultureName, am.ArticleSubject, 
			am.ArticleContext, am.ReadCount, 
			dbo.fnArticle_IsShowInLang(am.ArticleId, ''zh-TW'') as IsShowInLangZhTw,
			dbo.fnArticle_IsShowInLang(am.ArticleId, ''en'') as IsShowInLangEn, 
			am.PostAccount, am.PostDate, am.MdfAccount,
			am.MdfDate, isnull(e.DeptId, 0) as PostDeptId, d.DeptName as PostDeptName, 
			a.IsHideSelf, a.IsHideChild, a.StartDate, 
			a.EndDate, a.SortNo, a.DontDelete
		from dbo.ArticleMultiLang am
			join dbo.Article a on am.ArticleId=a.ArticleId
			left join dbo.Employee e on am.PostAccount=e.EmpAccount
			left join dbo.Department d on e.DeptId=d.DeptId
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
-- Create date: <2017/12/01>
-- Description: <刪除網頁內容>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticle_DeleteData
@ArticleId uniqueidentifier
as
begin
	begin transaction
	begin try
		-- delete multi language data
		delete from dbo.ArticleMultiLang
		where ArticleId=@ArticleId

		-- delete main data
		delete from dbo.Article
		where ArticleId=@ArticleId

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
-- Create date: <2017/12/01>
-- Description: <加大網頁內容的排序編號>
-- Test:
/*
exec dbo.spArticle_IncreaseSortNo '343a6e5f-5ab5-4f4a-81c6-4ae990df9ce8', 'admin'
*/
-- =============================================
create procedure dbo.spArticle_IncreaseSortNo
@ArticleId uniqueidentifier
,@MdfAccount varchar(20)
as
begin
	declare @ParentId uniqueidentifier
	declare @SortNo int

	select
		@ParentId=ParentId, @SortNo=SortNo
	from dbo.Article
	where ArticleId=@ArticleId

	if @SortNo is null
	begin
		set @SortNo=0
	end

	-- get bigger one
	declare @BiggerSortNo int
	declare @BiggerArticleId uniqueidentifier

	select top 1
		@BiggerSortNo=SortNo, @BiggerArticleId=ArticleId
	from dbo.Article
	where ParentId=@ParentId
		and ArticleId<>@ArticleId
		and SortNo>=@SortNo
	order by SortNo

	-- there is no bigger one, exit
	if @BiggerArticleId is null
	begin
		return
	end

	if @BiggerSortNo is null
	begin
		set @BiggerSortNo += 1
	end

	-- when the values area the same
	if @SortNo=@BiggerSortNo
	begin
		set @BiggerSortNo += 1
	end

	-- swap
	update dbo.Article
	set SortNo=@BiggerSortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where ArticleId=@ArticleId

	update dbo.Article
	set SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where ArticleId=@BiggerArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
-- Description: <減小網頁內容的排序編號>
-- Test:
/*
exec dbo.spArticle_DecreaseSortNo '343a6e5f-5ab5-4f4a-81c6-4ae990df9ce8', 'admin'
*/
-- =============================================
create procedure dbo.spArticle_DecreaseSortNo
@ArticleId uniqueidentifier
,@MdfAccount varchar(20)
as
begin
	declare @ParentId uniqueidentifier
	declare @SortNo int

	select
		@ParentId=ParentId, @SortNo=SortNo
	from dbo.Article
	where ArticleId=@ArticleId

	if @SortNo is null
	begin
		set @SortNo=0
	end

	-- get smaller one
	declare @SmallerSortNo int
	declare @SmallerArticleId uniqueidentifier

	select top 1
		@SmallerSortNo=SortNo, @SmallerArticleId=ArticleId
	from dbo.Article
	where ParentId=@ParentId
		and ArticleId<>@ArticleId
		and SortNo<=@SortNo
	order by SortNo desc

	-- there is no smaller one, exit
	if @SmallerArticleId is null
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
	update dbo.Article
	set SortNo=@SmallerSortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where ArticleId=@ArticleId

	update dbo.Article
	set SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where ArticleId=@SmallerArticleId
end
go




/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
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
