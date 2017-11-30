-- Article publisher SP, DB Functions
use SampleCMS
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
create procedure dbo.spArticleMultiLang_GetDataForbackend
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
-- Description: <更新網頁內容>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticle_UpdateData
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
	if exists(select * from dbo.Article where ParentId=@ParentId and ArticleAlias=@ArticleAlias)
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
