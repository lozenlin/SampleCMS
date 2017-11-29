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
create procedure dbo.spArticle_GetDataForBackend
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
		a.MdfAccount, a.MdfDate
	from dbo.Article a
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




/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/29>
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
