-- Article publisher SP, DB Functions
-- use SampleCMS
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

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/07>
-- Description: <附件檔案在指定語系是否顯示>
-- Test:
/*
*/
-- =============================================
create function dbo.fnAttachFile_IsShowInLang(
@AttId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select
		@IsShowInLang=IsShowInLang
	from dbo.AttachFileMultiLang
	where AttId=@AttId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <網頁照片在指定語系是否顯示>
-- Test:
/*
select dbo.fnArticlePicture_IsShowInLang('C2FC6EE9-D018-4A0C-B927-3362DDB5D902', 'zh-TW')
*/
-- =============================================
create function dbo.fnArticlePicture_IsShowInLang(
@PicId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select
		@IsShowInLang=IsShowInLang
	from dbo.ArticlePictureMultiLang
	where PicId=@PicId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/13>
-- Description: <網頁影片在指定語系是否顯示>
-- Test:
/*
select dbo.fnArticleVideo_IsShowInLang('68480C66-8F2F-4CEB-BC21-CD770B34B2F4', 'zh-TW')
*/
-- =============================================
create function dbo.fnArticleVideo_IsShowInLang(
@VidId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select
		@IsShowInLang=IsShowInLang
	from dbo.ArticleVideoMultiLang
	where VidId=@VidId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:		<http://lazycoders.blogspot.tw/2007/06/stripping-html-from-text-in-sql-server.html>
-- Create date: <2018/01/08>
-- Description:	<移除Html碼>
-- Test:
/*
*/
-- =============================================
create function dbo.fnStripHTML
(
@HTMLText nvarchar(MAX)
)
returns nvarchar(MAX)
as
begin
	DECLARE @Start  int
	DECLARE @End    int
	DECLARE @Length int

	SET @Start = CHARINDEX('<script', @HTMLText)
	SET @End = CHARINDEX('</script>', @HTMLText, CHARINDEX('<script', @HTMLText))
	SET @Length = (@End - @Start) + 9

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
		SET @Start = CHARINDEX('<script', @HTMLText)
		SET @End = CHARINDEX('</script>', @HTMLText, CHARINDEX('<script', @HTMLText))
		SET @Length = (@End - @Start) + 9
	END

	SET @Start = CHARINDEX('<style', @HTMLText)
	SET @End = CHARINDEX('</style>', @HTMLText, CHARINDEX('<style', @HTMLText))
	SET @Length = (@End - @Start) + 8

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
		SET @Start = CHARINDEX('<style', @HTMLText)
		SET @End = CHARINDEX('</style>', @HTMLText, CHARINDEX('<style', @HTMLText))
		SET @Length = (@End - @Start) + 8
	END

	-- Replace the HTML entity &amp; with the '&' character (this needs to be done first, as
	-- '&' might be double encoded as '&amp;amp;')
	SET @Start = CHARINDEX('&amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
		SET @Start = CHARINDEX('&amp;', @HTMLText)
		SET @End = @Start + 4
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &lt; with the '<' character
	SET @Start = CHARINDEX('&lt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '<')
		SET @Start = CHARINDEX('&lt;', @HTMLText)
		SET @End = @Start + 3
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &gt; with the '>' character
	SET @Start = CHARINDEX('&gt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '>')
		SET @Start = CHARINDEX('&gt;', @HTMLText)
		SET @End = @Start + 3
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &amp; with the '&' character
	SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
		SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
		SET @End = @Start + 4
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &nbsp; with the ' ' character
	SET @Start = CHARINDEX('&nbsp;', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, ' ')
		SET @Start = CHARINDEX('&nbsp;', @HTMLText)
		SET @End = @Start + 5
		SET @Length = (@End - @Start) + 1
	END

	-- Replace any <br> tags with a newline
	SET @Start = CHARINDEX('<br>', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, 'CHAR(13) + CHAR(10)')	--本來 CHAR(13) + CHAR(10) 前後沒加單引號
		SET @Start = CHARINDEX('<br>', @HTMLText)
		SET @End = @Start + 3
		SET @Length = (@End - @Start) + 1
	END

	-- Replace any <br/> tags with a newline
	SET @Start = CHARINDEX('<br/>', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, 'CHAR(13) + CHAR(10)')
		SET @Start = CHARINDEX('<br/>', @HTMLText)
		SET @End = @Start + 4
		SET @Length = (@End - @Start) + 1
	END

	-- Replace any <br /> tags with a newline
	SET @Start = CHARINDEX('<br />', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, 'CHAR(13) + CHAR(10)')
		SET @Start = CHARINDEX('<br />', @HTMLText)
		SET @End = @Start + 5
		SET @Length = (@End - @Start) + 1
	END

	-- Remove anything between <whatever> tags
	SET @Start = CHARINDEX('<', @HTMLText)
	SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
		SET @Start = CHARINDEX('<', @HTMLText)
		SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
		SET @Length = (@End - @Start) + 1
	END

	RETURN LTRIM(RTRIM(@HTMLText))

end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- Description: <建立搜尋結果的足跡用資料>
-- Test:
/*
select dbo.fnBuildBreadcrumbData('759CE382-7669-48A2-8B0E-230F65597AC3', 'zh-TW')
select dbo.fnBuildBreadcrumbData('00000000-0000-0000-0000-000000000000', 'zh-TW')
*/
-- =============================================
create function dbo.fnBuildBreadcrumbData
(
@ArticleId uniqueidentifier
,@CultureName varchar(10)
)
returns nvarchar(4000)
begin
	declare @BreadcrumbData nvarchar(4000)
	declare @ArticleSubject nvarchar(200)

	--先加自己
	select
		@ArticleSubject=am.ArticleSubject
	from dbo.Article a
		left join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId
	where a.ArticleId=@ArticleId 
		and am.CultureName=@CultureName
	
	set @BreadcrumbData = @ArticleSubject+','+cast(@ArticleId as varchar(36))

	declare @ParentId uniqueidentifier
	declare @newParenId uniqueidentifier
	--取得上層網頁代碼
	select @ParentId=ParentId
	from dbo.Article
	where ArticleId=@ArticleId

	--一個一個往前抓
	while @ParentId is not null and @ParentId<>'00000000-0000-0000-0000-000000000000'
	begin
		select
			@ArticleSubject=am.ArticleSubject
		from dbo.Article a
			left join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId 
		where a.ArticleId=@ParentId
			and am.CultureName=@CultureName
	
		set @BreadcrumbData=@ArticleSubject+','+cast(@ParentId as varchar(36))+','+@BreadcrumbData

		--取得上層網頁代碼
		set @newParenId=null
		
		select @newParenId=ParentId
		from dbo.Article
		where ArticleId=@ParentId
		
		set @ParentId=@newParenId
	end

	return @BreadcrumbData
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- Description: <取得階層1的網頁代碼>
-- Test:
/*
select dbo.fnGetLv1ArticleId('759CE382-7669-48A2-8B0E-230F65597AC3')
select dbo.fnGetLv1ArticleId('00000000-0000-0000-0000-000000000000')
*/
-- =============================================
create function dbo.fnGetLv1ArticleId
(
@ArticleId uniqueidentifier
)
returns uniqueidentifier
begin
	declare @Lv1Id uniqueidentifier
	set @Lv1Id=@ArticleId

	declare @ArticleLevelNo int
	--取得目前階層
	select @ArticleLevelNo=ArticleLevelNo
	from dbo.Article
	where ArticleId=@ArticleId

	--一個一個往前抓
	while @ArticleLevelNo>1
	begin
		select @Lv1Id=ParentId
		from dbo.Article
		where ArticleId=@Lv1Id

		set @ArticleLevelNo=@ArticleLevelNo-1
	end
	
	return @Lv1Id
end
go

-- =============================================
-- Author:		<lozen_lin>
-- Create date: <2018/01/09>
-- Description:	<字串串接轉為表格>
/*
select * from dbo.fnStringToTable(N',', N'xxx')
select * from  dbo.fnStringToTable(N',', N'aaa,bbb,')
select * from  dbo.fnStringToTable(N',', N'aaa , bbb , c')
*/
-- =============================================
create function dbo.fnStringToTable
(
@SplitterSymbol nvarchar(100)	= N','
,@String nvarchar(4000)
)
returns @tblResult table(
	Token nvarchar(4000)
)
as
begin
	declare @iSplitter int = charindex(@SplitterSymbol, @String)

	if @iSplitter=0
	begin
		insert into @tblResult(Token)
		values(@String)

		return
	end

	declare @Token nvarchar(4000)
	declare @iStart int = 1
	declare @iEnd int = @iSplitter-1

	while @iStart <= len(@String)
	begin
		set @Token = substring(@String, @iStart, @iEnd-@iStart+1)

		insert into @tblResult(Token)
		values (ltrim(rtrim(@Token)))

		if @iEnd = len(@String)
		begin
			break
		end

		set @iStart = @iSplitter+1
		set @iSplitter = charindex(@SplitterSymbol, @String, @iStart)	--超過總長度也會回傳0
		set @iEnd = @iSplitter-1

		if @iSplitter=0
		begin
			set @iEnd = len(@String)
		end
	end

	return
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
-- History:
--	2017/12/19, lozen_lin, 增加額外設定用的欄位
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
		a.MdfAccount, a.MdfDate, isnull(e.DeptId, 0) as PostDeptId, 
		a.SubjectAtBannerArea, a.PublishDate, a.IsShowInUnitArea, 
		a.IsShowInSitemap, a.SortFieldOfFrontStage, a.IsSortDescOfFrontStage, 
		a.IsListAreaShowInFrontStage, a.IsAttAreaShowInFrontStage, a.IsPicAreaShowInFrontStage, 
		a.IsVideoAreaShowInFrontStage, a.SubItemLinkUrl
	from dbo.Article a
		join dbo.Employee e on a.PostAccount=e.EmpAccount
	where a.ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/29>
-- History:
--	2017/12/19, lozen_lin, 增加額外設定用的欄位
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
		am.MdfAccount, am.MdfDate, am.Subtitle, 
		am.PublisherName
	from dbo.ArticleMultiLang am
	where am.ArticleId=@ArticleId
		and am.CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/23>
-- Description: <取得前台用網頁內容資料>
-- Test:
/*
exec dbo.spArticle_GetDataForFrontend '23661d48-17e7-4c45-bb11-8ec29be941c3', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticle_GetDataForFrontend
@ArticleId uniqueidentifier
,@CultureName varchar(10)
as
begin
	select
		a.ParentId, a.ArticleLevelNo, a.ArticleAlias, 
		a.BannerPicFileName, a.LayoutModeId, a.ShowTypeId, 
		a.LinkUrl, a.LinkTarget, a.ControlName, 
		a.IsHideSelf, a.IsHideChild, a.StartDate, 
		a.EndDate, a.SortNo, a.PostAccount, a.PostDate, 
		a.MdfAccount, a.MdfDate, a.SubjectAtBannerArea, 
		a.PublishDate, a.IsShowInUnitArea, a.IsShowInSitemap, 
		a.SortFieldOfFrontStage, a.IsSortDescOfFrontStage, a.IsListAreaShowInFrontStage, 
		a.IsAttAreaShowInFrontStage, a.IsPicAreaShowInFrontStage, a.IsVideoAreaShowInFrontStage, 
		am.ArticleSubject, am.ArticleContext, am.ReadCount, 
		am.IsShowInLang, am.Subtitle, am.PublisherName
	from dbo.Article a
		left join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId and am.CultureName=@CultureName
	where a.ArticleId=@ArticleId
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
-- History:
--	2017/12/05, lozen_lin, modify, @ArticleAlias 重覆判斷範圍改為所有文章
--	2017/12/19, lozen_lin, 增加額外設定用的欄位
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
,@SubjectAtBannerArea	bit
,@PublishDate	datetime
,@IsShowInUnitArea	bit
,@IsShowInSitemap	bit
,@SortFieldOfFrontStage	varchar(50)
,@IsSortDescOfFrontStage	bit
,@IsListAreaShowInFrontStage	bit
,@IsAttAreaShowInFrontStage	bit
,@IsPicAreaShowInFrontStage	bit
,@IsVideoAreaShowInFrontStage	bit
,@SubItemLinkUrl	nvarchar(2048)
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
	if exists(select * from dbo.Article where ArticleAlias=@ArticleAlias)
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
		PostDate, SubjectAtBannerArea, PublishDate, 
		IsShowInUnitArea, IsShowInSitemap, SortFieldOfFrontStage, 
		IsSortDescOfFrontStage, IsListAreaShowInFrontStage, IsAttAreaShowInFrontStage, 
		IsPicAreaShowInFrontStage, IsVideoAreaShowInFrontStage, SubItemLinkUrl
		)
	values(
		@ArticleId, @ParentId, @ArticleLevelNo, 
		@ArticleAlias, @BannerPicFileName, @LayoutModeId, 
		@ShowTypeId, @LinkUrl, @LinkTarget, 
		@ControlName, @SubItemControlName, @IsHideSelf, 
		@IsHideChild, @StartDate, @EndDate, 
		@SortNo, @DontDelete, @PostAccount, 
		getdate(), @SubjectAtBannerArea, @PublishDate, 
		@IsShowInUnitArea, @IsShowInSitemap, @SortFieldOfFrontStage, 
		@IsSortDescOfFrontStage, @IsListAreaShowInFrontStage, @IsAttAreaShowInFrontStage, 
		@IsPicAreaShowInFrontStage, @IsVideoAreaShowInFrontStage, @SubItemLinkUrl
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- History:
--	2017/12/19, lozen_lin, 增加額外設定用的欄位
--	2017/12/26, lozen_lin, 增加欄位「純文字的網頁內容」
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
,@Subtitle	nvarchar(500)
,@PublisherName	nvarchar(50)
,@TextContext	nvarchar(max)
as
begin
	insert into dbo.ArticleMultiLang(
		ArticleId, CultureName, ArticleSubject, 
		ArticleContext, IsShowInLang, PostAccount, 
		PostDate, Subtitle, PublisherName, 
		TextContext
		)
	values(
		@ArticleId, @CultureName, @ArticleSubject, 
		@ArticleContext, @IsShowInLang, @PostAccount, 
		getdate(), @Subtitle, @PublisherName, 
		@TextContext
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- History:
--	2017/12/01, lozen_lin, modify, 修正別名誤判問題
--	2017/12/05, lozen_lin, modify, @ArticleAlias 重覆判斷範圍改為所有文章
--	2017/12/19, lozen_lin, 增加額外設定用的欄位
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
,@SubjectAtBannerArea	bit
,@PublishDate	datetime
,@IsShowInUnitArea	bit
,@IsShowInSitemap	bit
,@SortFieldOfFrontStage	varchar(50)
,@IsSortDescOfFrontStage	bit
,@IsListAreaShowInFrontStage	bit
,@IsAttAreaShowInFrontStage	bit
,@IsPicAreaShowInFrontStage	bit
,@IsVideoAreaShowInFrontStage	bit
,@SubItemLinkUrl	nvarchar(2048)
as
begin
	declare @ParentId uniqueidentifier

	select 
		@ParentId=ParentId
	from dbo.Article
	where ArticleId=@ArticleId

	-- check alias
	if exists(select * from dbo.Article where ArticleId<>@ArticleId and ArticleAlias=@ArticleAlias)
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
		,SubjectAtBannerArea=@SubjectAtBannerArea
		,PublishDate=@PublishDate
		,IsShowInUnitArea=@IsShowInUnitArea
		,IsShowInSitemap=@IsShowInSitemap
		,SortFieldOfFrontStage=@SortFieldOfFrontStage
		,IsSortDescOfFrontStage=@IsSortDescOfFrontStage
		,IsListAreaShowInFrontStage=@IsListAreaShowInFrontStage
		,IsAttAreaShowInFrontStage=@IsAttAreaShowInFrontStage
		,IsPicAreaShowInFrontStage=@IsPicAreaShowInFrontStage
		,IsVideoAreaShowInFrontStage=@IsVideoAreaShowInFrontStage
		,SubItemLinkUrl=@SubItemLinkUrl
	where ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/11/30>
-- History:
--	2017/12/19, lozen_lin, 增加額外設定用的欄位
--	2017/12/26, lozen_lin, 增加欄位「純文字的網頁內容」
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
,@Subtitle	nvarchar(500)
,@PublisherName	nvarchar(50)
,@TextContext	nvarchar(max)
as
begin
	update dbo.ArticleMultiLang
	set
		ArticleSubject=@ArticleSubject
		,ArticleContext=@ArticleContext
		,IsShowInLang=@IsShowInLang
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
		,Subtitle=@Subtitle
		,PublisherName=@PublisherName
		,TextContext=@TextContext
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
create procedure dbo.spArticleMultiLang_GetListForBackend
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
			am.ArticleId, am.ArticleSubject, am.ReadCount, 
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
-- Create date: <2017/12/25>
-- History:
--	2017/12/26, lozen_lin, 增加欄位「純文字的網頁內容」
-- Description: <取得前台用的有效網頁內容清單>
-- Test:
/*
declare @RowCount int
exec dbo.spArticle_GetValidListForFrontend '00000000-0000-0000-0000-000000000000', 'zh-TW', N'', 1, 20, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
create procedure dbo.spArticle_GetValidListForFrontend
@ParentId	uniqueidentifier
,@CultureName	varchar(10)
,@Kw nvarchar(52)=''
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
	set @conditions=N' and a.ParentId=@ParentId and am.CultureName=@CultureName
and a.IsHideSelf=0
and a.StartDate <= getdate() and getdate() < a.EndDate+1
and am.IsShowInLang=1
'
	
	if @Kw<>N''
	begin
		set @conditions += N' and am.ArticleSubject like @Kw '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.ArticleMultiLang am
	join dbo.Article a on am.ArticleId=a.ArticleId
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@ParentId	uniqueidentifier
,@CultureName	varchar(10)
,@Kw nvarchar(52)
'

	set @parmDefForTotal = @parmDef + N',@RowCount int output'

	set @Kw = N'%'+@Kw+N'%'

	exec sp_executesql @sql, @parmDefForTotal, 
		@ParentId
		,@CultureName
		,@Kw
		,@RowCount output

	--取得指定排序和範圍的結果

	--指定排序
	declare @SortExp nvarchar(200)
	set @SortExp=N' order by '

	if @SortField in (N'StartDate', N'SortNo', N'PostDate', N'MdfDate', N'PublishDate', N'ArticleSubject')
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
			am.ArticleId, am.ArticleSubject, am.PublisherName,
			a.ArticleAlias, a.ShowTypeId, a.LinkUrl, 
			a.LinkTarget, a.StartDate, a.SortNo, 
			a.PostDate, a.MdfDate, a.PublishDate, 
			am.TextContext
		from dbo.ArticleMultiLang am
			join dbo.Article a on am.ArticleId=a.ArticleId
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
		,@BeginNum
		,@EndNum
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
-- History:
--	2018/01/31, lozen_lin, modify, 增加刪除附件、照片、影片
--	2018/01/31, lozen_lin, modify, 修正 ArticlePicture 錯誤關聯
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
		-- delete attachment
		delete from afm
		from dbo.AttachFileMultiLang afm
			join dbo.AttachFile af on afm.AttId=af.AttId
		where af.ArticleId=@ArticleId

		delete from dbo.AttachFile
		where ArticleId=@ArticleId

		-- delete picture
		delete from apm
		from dbo.ArticlePictureMultiLang apm
			join dbo.ArticlePicture ap on apm.PicId=ap.PicId
		where ap.ArticleId=@ArticleId

		delete from dbo.ArticlePicture
		where ArticleId=@ArticleId

		-- delete video
		delete from avm
		from dbo.ArticleVideoMultiLang avm
			join dbo.ArticleVideo av on avm.VidId=av.VidId
		where av.ArticleId=@ArticleId

		delete from dbo.ArticleVideo
		where ArticleId=@ArticleId

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

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
-- History:
--	2017/12/26, lozen_lin, modify, 列出有效日期
--	2018/01/05, lozen_lin, modify, 修正問題「應該找不到資料時卻多出一筆」
-- Description: <取得指定語系的網頁內容階層資料>
-- Test:
/*
exec dbo.spArticleMultiLang_GetLevelInfo '759CE382-7669-48A2-8B0E-230F65597AC3', 'zh-TW'
exec dbo.spArticleMultiLang_GetLevelInfo '759CE382-7669-48A2-8B0E-230F65597AC3', 'en'
exec dbo.spArticleMultiLang_GetLevelInfo '00000000-0000-0000-0000-000000000000', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticleMultiLang_GetLevelInfo
@ArticleId uniqueidentifier
,@CultureName varchar(10)
as
begin
	create table #tbl(
		ArticleId	uniqueidentifier
		,ArticleSubject	nvarchar(200)
		,ArticleLevelNo	int
		,ShowTypeId	int
		,LinkUrl	nvarchar(2048)
		,LinkTarget	varchar(10)
		,IsHideSelf	bit
		,IsShowInLang	bit
		,StartDate datetime
		,EndDate datetime
	)

	declare @CurArticleId	uniqueidentifier = @ArticleId
	declare @ParentId uniqueidentifier
	declare @ArticleSubject	nvarchar(200)
	declare @ArticleLevelNo	int
	declare @ShowTypeId	int
	declare @LinkUrl	nvarchar(2048)
	declare @LinkTarget	varchar(10)
	declare @IsHideSelf	bit
	declare @IsShowInLang	bit
	declare @StartDate datetime
	declare @EndDate datetime

	while 1=1
	begin
		select
			@ParentId=a.ParentId, @ArticleSubject=am.ArticleSubject, @ArticleLevelNo=a.ArticleLevelNo,
			@ShowTypeId=a.ShowTypeId, @LinkUrl=a.LinkUrl, @LinkTarget=a.LinkTarget, 
			@IsHideSelf=a.IsHideSelf, @IsShowInLang=am.IsShowInLang, @StartDate=a.StartDate,
			@EndDate=a.EndDate
		from dbo.ArticleMultiLang am
			join dbo.Article a on am.ArticleId=a.ArticleId
		where am.ArticleId=@CurArticleId
			and am.CultureName=@CultureName

		if @@rowcount > 0
		begin
			insert into #tbl(
				ArticleId, ArticleSubject, ArticleLevelNo
				,ShowTypeId, LinkUrl, LinkTarget
				,IsHideSelf, IsShowInLang, StartDate
				,EndDate
				)
			values(
				@CurArticleId, @ArticleSubject, @ArticleLevelNo
				,@ShowTypeId, @LinkUrl, @LinkTarget
				,@IsHideSelf, @IsShowInLang, @StartDate
				,@EndDate
				)
		end

		if @ParentId is null
		begin
			break;
		end

		set @CurArticleId=@ParentId
	end

	select * from #tbl
	order by ArticleLevelNo desc
	
	drop table #tbl 
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/20>
-- History:
--	2018/01/27, lozen_lin, modify, 增加權限判斷
-- Description: <更新網頁內容的指定區域是否在前台顯示>
-- Test:
/*
exec dbo.spArticle_UpdateIsAreaShowInFrontStage '00000000-0000-0000-0000-000000000000', 'ListArea', 1, 'admin', 0, 0, 1, 'admin', 0
*/
-- =============================================
create procedure dbo.spArticle_UpdateIsAreaShowInFrontStage
@ArticleId uniqueidentifier
,@AreaName varchar(20)
,@IsShowInFrontStage bit
,@MdfAccount varchar(20)
,@CanEditSubItemOfOthers bit=1	--可修改任何人的子項目
,@CanEditSubItemOfCrew bit=1	--可修改同部門的子項目
,@CanEditSubItemOfSelf bit=1	--可修改自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
as
begin
	declare @colName nvarchar(50)
	set @colName = case @AreaName
						when 'ListArea' then N'IsListAreaShowInFrontStage'
						when 'AttArea' then N'IsAttAreaShowInFrontStage'
						when 'PicArea' then N'IsPicAreaShowInFrontStage'
						when 'VideoArea' then N'IsVideoAreaShowInFrontStage'
						else N''
					end

	if @colName=N''
	begin
		raiserror(N'@AreaName is invalid.', 11, 1)
		return
	end

	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)

	set @sql = N'
update a
set a.' + @colName + N' = @IsShowInFrontStage
	,a.MdfAccount=@MdfAccount
	,a.MdfDate=getdate()
from dbo.Article a
	left join dbo.Employee e on a.PostAccount=e.EmpAccount
where a.ArticleId=@ArticleId
	and (@CanEditSubItemOfOthers=1
		or @CanEditSubItemOfCrew=1 and e.DeptId=@MyDeptId
		or @CanEditSubItemOfSelf=1 and a.PostAccount=@MyAccount)
'

	set @parmDef = N'
@ArticleId uniqueidentifier
,@IsShowInFrontStage bit
,@MdfAccount varchar(20)
,@CanEditSubItemOfOthers bit
,@CanEditSubItemOfCrew bit
,@CanEditSubItemOfSelf bit
,@MyAccount varchar(20)
,@MyDeptId int
'

	exec sp_executesql @sql, @parmDef,
		@ArticleId
		,@IsShowInFrontStage
		,@MdfAccount
		,@CanEditSubItemOfOthers
		,@CanEditSubItemOfCrew
		,@CanEditSubItemOfSelf
		,@MyAccount
		,@MyDeptId

	if @@rowcount=0
	begin
		raiserror(N'update failed', 11, 1)
	end
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/20>
-- History:
--	2018/01/27, lozen_lin, modify, 增加權限判斷
-- Description: <更新網頁內容的前台子項目排序欄位>
-- Test:
/*
exec dbo.spArticle_UpdateSortFieldOfFrontStage '2def0e4f-c47e-4679-8e1a-6084e2e72dd6', 'SortNo', 0, 'admin', 1, 1, 1, 'admin', 0
*/
-- =============================================
create procedure dbo.spArticle_UpdateSortFieldOfFrontStage
@ArticleId uniqueidentifier
,@SortFieldOfFrontStage	varchar(50)
,@IsSortDescOfFrontStage	bit
,@MdfAccount varchar(20)
,@CanEditSubItemOfOthers bit=1	--可修改任何人的子項目
,@CanEditSubItemOfCrew bit=1	--可修改同部門的子項目
,@CanEditSubItemOfSelf bit=1	--可修改自己的子項目
,@MyAccount varchar(20)=''
,@MyDeptId int=0
as
begin
	update a
	set a.SortFieldOfFrontStage=@SortFieldOfFrontStage
		,a.IsSortDescOfFrontStage=@IsSortDescOfFrontStage
		,a.MdfAccount=@MdfAccount
		,a.MdfDate=getdate()
	from dbo.Article a
		left join dbo.Employee e on a.PostAccount=e.EmpAccount
	where a.ArticleId=@ArticleId
		and (@CanEditSubItemOfOthers=1
			or @CanEditSubItemOfCrew=1 and e.DeptId=@MyDeptId
			or @CanEditSubItemOfSelf=1 and a.PostAccount=@MyAccount)

	if @@rowcount=0
	begin
		raiserror(N'update failed', 11, 1)
	end
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/23>
-- Description: <依網址別名取得網頁代碼>
-- Test:
/*
exec dbo.spArticle_GetArticleIdByAlias 'test1'
*/
-- =============================================
create procedure dbo.spArticle_GetArticleIdByAlias
@ArticleAlias	varchar(50)
as
begin
	select top 1
		ArticleId
	from dbo.Article
	where ArticleAlias=@ArticleAlias
	order by PostDate
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/23>
-- Description: <依超連結網址取得網頁代碼>
-- Test:
/*
exec dbo.spArticle_GetArticleIdByLinkUrl '~/Sitemap.aspx'
*/
-- =============================================
create procedure dbo.spArticle_GetArticleIdByLinkUrl
@LinkUrl	nvarchar(2048)
as
begin
	select top 1
		ArticleId
	from dbo.Article
	where ShowTypeId=3 /* URL */
		and LinkUrl=@LinkUrl
	order by PostDate
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/23>
-- History:
--	2018/01/23, lozen_lin, 結果儲存方式從暫存表改為變數
-- Description: <取得指定網頁內容的前幾層網頁代碼>
-- Test:
/*
exec dbo.spArticle_GetTopLevelIds '759CE382-7669-48A2-8B0E-230F65597AC3'
exec dbo.spArticle_GetTopLevelIds 'b1d34d29-255a-42a2-b9af-c33911bcde9a'
*/
-- =============================================
create procedure dbo.spArticle_GetTopLevelIds
@ArticleId uniqueidentifier
as
begin
	declare @Lv1Id uniqueidentifier
		,@Lv2Id uniqueidentifier
		,@Lv3Id uniqueidentifier

	declare @CurLevelNo int
	declare @CurArticleId uniqueidentifier = @ArticleId
	declare @ParentId uniqueidentifier

	while 1=1
	begin
		select
			@CurLevelNo=ArticleLevelNo, 
			@ParentId=ParentId
		from dbo.Article
		where ArticleId=@CurArticleId

		if @CurLevelNo = 1
		begin
			set @Lv1Id=@CurArticleId
		end
		else if @CurLevelNo = 2
		begin
			set @Lv2Id=@CurArticleId
		end
		else if @CurLevelNo = 3
		begin
			set @Lv3Id=@CurArticleId
		end

		if (@curLevelNo is null or @curLevelNo<=1) break

		set @CurArticleId=@ParentId
	end

	select @Lv1Id as Lv1Id
		,@Lv2Id as Lv2Id
		,@Lv3Id as Lv3Id
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/27>
-- Description: <增加網頁內容的多國語系資料被點閱次數>
-- Test:
/*
exec dbo.spArticleMultiLang_IncreaseReadCount '00000000-0000-0000-0000-000000000000', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticleMultiLang_IncreaseReadCount
@ArticleId	uniqueidentifier
,@CultureName	varchar(10)
as
begin
	update dbo.ArticleMultiLang
	set ReadCount += 1
	where ArticleId=@ArticleId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/27>
-- Description: <取得使用在單元區的有效網頁清單>
-- Test:
/*
exec dbo.spArticle_GetValidListForUnitArea '00000000-0000-0000-0000-000000000000', 'zh-TW', 1
exec dbo.spArticle_GetValidListForUnitArea '23661D48-17E7-4C45-BB11-8EC29BE941C3', 'zh-TW', 0
*/
-- =============================================
create procedure dbo.spArticle_GetValidListForUnitArea
@ParentId uniqueidentifier
,@CultureName varchar(10)
,@IsShowInUnitArea bit
as
begin
	select
		am.ArticleId, am.ArticleSubject, a.ArticleAlias, 
		a.ShowTypeId, a.LinkUrl, a.LinkTarget, 
		a.IsHideChild
	from dbo.ArticleMultiLang am
		join dbo.Article a on am.ArticleId=a.ArticleId
	where a.ParentId=@ParentId
		and am.CultureName=@CultureName
		and a.IsHideSelf=0
		and a.StartDate <= getdate() and getdate() < a.EndDate+1
		and am.IsShowInLang=1
		and a.IsShowInUnitArea=@IsShowInUnitArea
	order by a.SortNo
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/02>
-- Description: <取得使用在側邊區塊的有效網頁清單>
-- Test:
/*
exec dbo.spArticle_GetValidListForSideSection '00000000-0000-0000-0000-000000000000', 'zh-TW'
exec dbo.spArticle_GetValidListForSideSection '23661D48-17E7-4C45-BB11-8EC29BE941C3', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticle_GetValidListForSideSection
@ParentId uniqueidentifier
,@CultureName varchar(10)
as
begin
	select
		am.ArticleId, am.ArticleSubject, a.ArticleAlias, 
		a.ShowTypeId, a.LinkUrl, a.LinkTarget, 
		a.IsHideChild
	from dbo.ArticleMultiLang am
		join dbo.Article a on am.ArticleId=a.ArticleId
	where a.ParentId=@ParentId
		and am.CultureName=@CultureName
		and a.IsHideSelf=0
		and a.StartDate <= getdate() and getdate() < a.EndDate+1
		and am.IsShowInLang=1
	order by a.SortNo
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/04>
-- Description: <取得使用在網站導覽的有效網頁清單>
-- Test:
/*
exec dbo.spArticle_GetValidListForSitemap '00000000-0000-0000-0000-000000000000', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticle_GetValidListForSitemap
@ParentId uniqueidentifier
,@CultureName varchar(10)
as
begin
	select
		am.ArticleId, am.ArticleSubject, a.ArticleAlias, 
		a.ShowTypeId, a.LinkUrl, a.LinkTarget, 
		a.IsHideChild, a.ArticleLevelNo
	from dbo.ArticleMultiLang am
		join dbo.Article a on am.ArticleId=a.ArticleId
	where a.ParentId=@ParentId
		and am.CultureName=@CultureName
		and a.IsHideSelf=0
		and a.StartDate <= getdate() and getdate() < a.EndDate+1
		and am.IsShowInLang=1
		and a.IsShowInSitemap=1
	order by a.SortNo
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/31>
-- Description: <取得網頁的所有子網頁>
-- Test:
/*
exec dbo.spArticle_GetDescendants '00000000-0000-0000-0000-000000000000'
exec dbo.spArticle_GetDescendants 'bcea101f-0173-4d07-84fa-0d7bb9a81073'
*/
-- =============================================
create procedure dbo.spArticle_GetDescendants
@ArticleId uniqueidentifier
as
begin
	declare @CurArticleId uniqueidentifier=@ArticleId
	declare @CurLevelNo int

	-- get current info
	select
		@CurLevelNo=ArticleLevelNo
	from dbo.Article
	where ArticleId=@ArticleId

	create table #tblResult(
		ArticleId uniqueidentifier
		,ArticleLevelNo int
	)

	if @CurLevelNo is not null
	begin
		insert into #tblResult
		values (@CurArticleId, @CurLevelNo)
	end

	while 1=1
	begin
		insert into #tblResult
			select
				ArticleId, ArticleLevelNo
			from dbo.Article
			where ParentId in (
				select ArticleId from #tblResult 
				where ArticleLevelNo=@CurLevelNo
				)

		if @@rowcount=0
		begin
			break
		end

		set @CurLevelNo+=1
	end

	select * from #tblResult
	order by ArticleLevelNo desc

	drop table #tblResult
end
go

----------------------------------------------------------------------------
-- 附件檔案
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/05>
-- Description: <取得後台用附件檔案資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFile_GetDataForBackend
@AttId	uniqueidentifier
as
begin
	select
		ArticleId, FilePath, FileSavedName, 
		FileSize, SortNo, FileMIME, 
		DontDelete, PostAccount, PostDate, 
		MdfAccount, MdfDate
	from dbo.AttachFile
	where AttId=@AttId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/05>
-- Description: <取得後台用附件檔案的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFileMultiLang_GetDataForBackend
@AttId	uniqueidentifier
,@CultureName	varchar(10)
as
begin
	select
		AttSubject, ReadCount, IsShowInLang, 
		PostAccount, PostDate, MdfAccount, 
		MdfDate
	from dbo.AttachFileMultiLang
	where AttId=@AttId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/06>
-- Description: <取得附件檔案的最大排序編號>
-- Test:
/*
exec dbo.spAttachFile_GetMaxSortNo null
*/
-- =============================================
create procedure dbo.spAttachFile_GetMaxSortNo
@ArticleId	uniqueidentifier
as
begin
	select
		isnull(max(SortNo), 0) as MaxSortNo
	from dbo.AttachFile
	where ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/06>
-- Description: <新增附件檔案資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFile_InsertData
@AttId	uniqueidentifier
,@ArticleId	uniqueidentifier
,@FilePath	nvarchar(150)
,@FileSavedName	nvarchar(500)
,@FileSize	int
,@SortNo	int
,@FileMIME	varchar(255)
,@DontDelete	bit
,@PostAccount	varchar(20)
as
begin
	insert into dbo.AttachFile(
		AttId, ArticleId, FilePath, 
		FileSavedName, FileSize, SortNo, 
		FileMIME, DontDelete, PostAccount,
		PostDate
		)
	values(
		@AttId, @ArticleId, @FilePath, 
		@FileSavedName, @FileSize, @SortNo, 
		@FileMIME, @DontDelete, @PostAccount,
		getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/06>
-- Description: <新增附件檔案的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFileMultiLang_InsertData
@AttId	uniqueidentifier
,@CultureName	varchar(10)
,@AttSubject	nvarchar(200)
,@IsShowInLang	bit
,@PostAccount	varchar(20)
as
begin
	insert into dbo.AttachFileMultiLang(
		AttId, CultureName, AttSubject, 
		IsShowInLang, PostAccount, PostDate
		)
	values(
		@AttId, @CultureName, @AttSubject, 
		@IsShowInLang, @PostAccount, getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/06>
-- Description: <更新附件檔案資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFile_UpdateData
@AttId	uniqueidentifier
,@FilePath	nvarchar(150)
,@FileSavedName	nvarchar(500)
,@FileSize	int
,@SortNo	int
,@FileMIME	varchar(255)
,@DontDelete	bit
,@MdfAccount	varchar(20)
as
begin
	update dbo.AttachFile
	set FilePath=@FilePath
		,FileSavedName=@FileSavedName
		,FileSize=@FileSize
		,SortNo=@SortNo
		,FileMIME=@FileMIME
		,DontDelete=@DontDelete
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where AttId=@AttId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/06>
-- Description: <更新附件檔案的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFileMultiLang_UpdateData
@AttId	uniqueidentifier
,@CultureName	varchar(10)
,@AttSubject	nvarchar(200)
,@IsShowInLang	bit
,@MdfAccount	varchar(20)
as
begin
	update dbo.AttachFileMultiLang
	set AttSubject=@AttSubject
		,IsShowInLang=@IsShowInLang
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where AttId=@AttId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/06>
-- Description: <刪除附件檔案資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFile_DeleteData
@AttId uniqueidentifier
as
begin
	begin transaction
	begin try
		-- delete multi language data
		delete from dbo.AttachFileMultiLang
		where AttId=@AttId

		-- delete main data
		delete from dbo.AttachFile
		where AttId=@AttId

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
-- Create date: <2017/12/07>
-- Description: <取得後台用指定語系的附件檔案清單>
-- Test:
/*
declare @RowCount int
exec dbo.spAttachFileMultiLang_GetListForBackend '00000000-0000-0000-0000-000000000000', 'zh-TW', N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
create procedure dbo.spAttachFileMultiLang_GetListForBackend
@ArticleId uniqueidentifier
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
	set @conditions=N' and af.ArticleId=@ArticleId and afm.CultureName=@CultureName '

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and afm.PostAccount=@MyAccount) '
	
	if @Kw<>N''
	begin
		set @conditions += N' and afm.AttSubject like @Kw '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.AttachFileMultiLang afm
	join dbo.AttachFile af on afm.AttId=af.AttId
	left join dbo.Employee e on afm.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@ArticleId	uniqueidentifier
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
		@ArticleId
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

	if @SortField in (N'AttSubject', N'SortNo', N'PostDate', N'PostDeptName')
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
			afm.AttId, afm.AttSubject, afm.ReadCount, 
			dbo.fnAttachFile_IsShowInLang(afm.AttId, ''zh-TW'') as IsShowInLangZhTw,
			dbo.fnAttachFile_IsShowInLang(afm.AttId, ''en'') as IsShowInLangEn, 
			afm.PostAccount, afm.PostDate, afm.MdfAccount, 
			afm.MdfDate, isnull(e.DeptId, 0) as PostDeptId, d.DeptName as PostDeptName,
			af.FilePath, af.FileSavedName, af.FileSize, 
			af.SortNo, af.FileMIME, af.DontDelete
		from dbo.AttachFileMultiLang afm
			join dbo.AttachFile af on afm.AttId=af.AttId
			left join dbo.Employee e on afm.PostAccount=e.EmpAccount
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
		@ArticleId
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
-- Create date: <2017/12/07>
-- Description: <加大附件檔案的排序編號>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFile_IncreaseSortNo
@AttId uniqueidentifier
,@MdfAccount varchar(20)
as
begin
	declare @ArticleId uniqueidentifier
	declare @SortNo int

	select
		@ArticleId=ArticleId, @SortNo=SortNo
	from dbo.AttachFile
	where AttId=@AttId

	if @SortNo is null
	begin
		set @SortNo=0
	end

	-- get bigger one
	declare @BiggerSortNo int
	declare @BiggerAttId uniqueidentifier

	select top 1
		@BiggerSortNo=SortNo, @BiggerAttId=AttId
	from dbo.AttachFile
	where ArticleId=@ArticleId
		and AttId<>@AttId
		and SortNo>=@SortNo
	order by SortNo

	-- there is no bigger one, exit
	if @BiggerAttId is null
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
	update dbo.AttachFile
	set SortNo=@BiggerSortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where AttId=@AttId

	update dbo.AttachFile
	set SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where AttId=@BiggerAttId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/07>
-- Description: <減小附件檔案的排序編號>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFile_DecreaseSortNo
@AttId uniqueidentifier
,@MdfAccount varchar(20)
as
begin
	declare @ArticleId uniqueidentifier
	declare @SortNo int

	select
		@ArticleId=ArticleId, @SortNo=SortNo
	from dbo.AttachFile
	where AttId=@AttId

	if @SortNo is null
	begin
		set @SortNo=0
	end

	-- get smaller one
	declare @SmallerSortNo int
	declare @SmallerAttId uniqueidentifier

	select top 1
		@SmallerSortNo=SortNo, @SmallerAttId=AttId
	from dbo.AttachFile
	where ArticleId=@ArticleId
		and AttId<>@AttId
		and SortNo<=@SortNo
	order by SortNo desc

	-- there is no smaller one, exit
	if @SmallerAttId is null
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
	update dbo.AttachFile
	set SortNo=@SmallerSortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where AttId=@AttId

	update dbo.AttachFile
	set SortNo=@SortNo
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where AttId=@SmallerAttId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/07>
-- Description: <增加附件檔案的多國語系資料被點閱次數>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spAttachFileMultiLang_IncreaseReadCount
@AttId uniqueidentifier
,@CultureName varchar(10)
as
begin
	update dbo.AttachFileMultiLang
	set ReadCount += 1
	where AttId=@AttId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/28>
-- Description: <取得前台用附件檔案清單>
-- Test:
/*
exec dbo.spAttachFile_GetListForFrontend '00000000-0000-0000-0000-000000000000', 'zh-TW'
*/
-- =============================================
create procedure dbo.spAttachFile_GetListForFrontend
@ArticleId	uniqueidentifier
,@CultureName	varchar(10)
as
begin
	select
		afm.AttId, afm.AttSubject, afm.ReadCount, 
		af.FileSavedName, af.FileSize, af.SortNo, 
		af.PostDate, af.MdfDate
	from dbo.AttachFileMultiLang afm
		join dbo.AttachFile af on afm.AttId=af.AttId 
	where af.ArticleId=@ArticleId
		and afm.CultureName=@CultureName
		and afm.IsShowInLang=1
	order by af.SortNo
end
go

----------------------------------------------------------------------------
-- 網頁照片
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/08>
-- Description: <取得後台用網頁照片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePicture_GetDataForBackend
@PicId uniqueidentifier
as
begin
	select
		ArticleId, FileSavedName, FileSize, 
		SortNo, FileMIME, PostAccount, 
		PostDate, MdfAccount, MdfDate
	from dbo.ArticlePicture
	where PicId=@PicId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/08>
-- Description: <取得後台用網頁照片的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePictureMultiLang_GetDataForBackend
@PicId uniqueidentifier
,@CultureName varchar(10)
as
begin
	select
		PicSubject, IsShowInLang, PostAccount, 
		PostDate, MdfAccount, MdfDate
	from dbo.ArticlePictureMultiLang
	where PicId=@PicId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/08>
-- Description: <取得網頁照片的最大排序編號>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePicture_GetMaxSortNo
@ArticleId	uniqueidentifier
as
begin
	select
		isnull(max(SortNo), 0) as MaxSortNo
	from dbo.ArticlePicture
	where ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <刪除網頁照片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePicture_DeleteData
@PicId uniqueidentifier
as
begin
	begin transaction
	begin try
		-- delete multi language data
		delete from dbo.ArticlePictureMultiLang
		where PicId=@PicId

		-- delete main data
		delete from dbo.ArticlePicture
		where PicId=@PicId

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
-- Create date: <2017/12/11>
-- Description: <新增網頁照片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePicture_InsertData
@PicId	uniqueidentifier
,@ArticleId	uniqueidentifier
,@FileSavedName	nvarchar(500)
,@FileSize	int
,@SortNo	int
,@FileMIME	varchar(255)
,@PostAccount	varchar(20)
as
begin
	insert into dbo.ArticlePicture(
		PicId, ArticleId, FileSavedName, 
		FileSize, SortNo, FileMIME, 
		PostAccount, PostDate
		)
	values(
		@PicId, @ArticleId, @FileSavedName, 
		@FileSize, @SortNo, @FileMIME, 
		@PostAccount, getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <新增網頁照片的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePictureMultiLang_InsertData
@PicId	uniqueidentifier
,@CultureName	varchar(10)
,@PicSubject	nvarchar(200)
,@IsShowInLang	bit
,@PostAccount	varchar(20)
as
begin
	insert into dbo.ArticlePictureMultiLang(
		PicId, CultureName, PicSubject, 
		IsShowInLang, PostAccount, PostDate
		)
	values(
		@PicId, @CultureName, @PicSubject, 
		@IsShowInLang, @PostAccount, getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <更新網頁照片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePicture_UpdateData
@PicId	uniqueidentifier
,@FileSavedName	nvarchar(500)
,@FileSize	int
,@SortNo	int
,@FileMIME	varchar(255)
,@MdfAccount	varchar(20)
as
begin
	update dbo.ArticlePicture
	set FileSavedName=@FileSavedName
		,FileSize=@FileSize
		,SortNo=@SortNo
		,FileMIME=@FileMIME
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where PicId=@PicId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <更新網頁照片的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticlePictureMultiLang_UpdateData
@PicId	uniqueidentifier
,@CultureName	varchar(10)
,@PicSubject	nvarchar(200)
,@IsShowInLang	bit
,@MdfAccount	varchar(20)
as
begin
	update dbo.ArticlePictureMultiLang
	set PicSubject=@PicSubject
		,IsShowInLang=@IsShowInLang
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where PicId=@PicId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <取得後台用指定語系的網頁照片清單>
-- Test:
/*
declare @RowCount int
exec dbo.spArticlePictureMultiLang_GetListForBackend '00000000-0000-0000-0000-000000000000', 'zh-TW', N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
create procedure dbo.spArticlePictureMultiLang_GetListForBackend
@ArticleId uniqueidentifier
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
	set @conditions=N' and ap.ArticleId=@ArticleId and apm.CultureName=@CultureName '

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and apm.PostAccount=@MyAccount) '
	
	if @Kw<>N''
	begin
		set @conditions += N' and apm.PicSubject like @Kw '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.ArticlePictureMultiLang apm
	join dbo.ArticlePicture ap on apm.PicId=ap.PicId
	left join dbo.Employee e on apm.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@ArticleId	uniqueidentifier
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
		@ArticleId
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

	if @SortField in (N'PicSubject', N'SortNo', N'PostDate', N'PostDeptName')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by SortNo desc'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			apm.PicId, apm.PicSubject, 
			dbo.fnArticlePicture_IsShowInLang(apm.PicId, ''zh-TW'') as IsShowInLangZhTw,
			dbo.fnArticlePicture_IsShowInLang(apm.PicId, ''en'') as IsShowInLangEn, 
			apm.PostAccount, apm.PostDate, apm.MdfAccount, 
			apm.MdfDate, isnull(e.DeptId, 0) as PostDeptId, d.DeptName as PostDeptName,
			ap.FileSavedName, ap.FileSize, 
			ap.SortNo, ap.FileMIME
		from dbo.ArticlePictureMultiLang apm
			join dbo.ArticlePicture ap on apm.PicId=ap.PicId
			left join dbo.Employee e on apm.PostAccount=e.EmpAccount
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
		@ArticleId
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
-- Create date: <2017/12/29>
-- Description: <取得前台用網頁照片清單>
-- Test:
/*
exec dbo.spArticlePicture_GetListForFrontend '00000000-0000-0000-0000-000000000000', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticlePicture_GetListForFrontend
@ArticleId	uniqueidentifier
,@CultureName	varchar(10)
as
begin
	select
		apm.PicId, apm.PicSubject, ap.FileSavedName, 
		ap.SortNo
	from dbo.ArticlePictureMultiLang apm
		join dbo.ArticlePicture ap on apm.PicId=ap.PicId
	where ap.ArticleId=@ArticleId
		and apm.CultureName=@CultureName
		and apm.IsShowInLang=1
	order by ap.SortNo desc
end
go

----------------------------------------------------------------------------
-- 網頁影片
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <取得後台用網頁影片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideo_GetDataForBackend
@VidId	uniqueidentifier
as
begin
	select
		ArticleId, SortNo, VidLinkUrl, 
		SourceVideoId, PostAccount, PostDate, 
		MdfAccount, MdfDate
	from dbo.ArticleVideo
	where VidId=@VidId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <取得後台用網頁影片的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideoMultiLang_GetDataForBackend
@VidId	uniqueidentifier
,@CultureName	varchar(10)
as
begin
	select
		VidSubject, VidDesc, IsShowInLang, 
		PostAccount, PostDate, MdfAccount,
		MdfDate
	from dbo.ArticleVideoMultiLang
	where VidId=@VidId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <取得網頁影片的最大排序編號>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideo_GetMaxSortNo
@ArticleId	uniqueidentifier
as
begin
	select
		isnull(max(SortNo), 0) as MaxSortNo
	from dbo.ArticleVideo
	where ArticleId=@ArticleId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <新增網頁影片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideo_InsertData
@VidId	uniqueidentifier
,@ArticleId	uniqueidentifier
,@SortNo	int
,@VidLinkUrl	nvarchar(2048)
,@SourceVideoId	varchar(100)
,@PostAccount	varchar(20)
as
begin
	insert into dbo.ArticleVideo(
		VidId, ArticleId, SortNo, 
		VidLinkUrl, SourceVideoId, PostAccount,
		PostDate
		)
	values(
		@VidId, @ArticleId, @SortNo, 
		@VidLinkUrl, @SourceVideoId, @PostAccount,
		getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <新增網頁影片的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideoMultiLang_InsertData
@VidId	uniqueidentifier
,@CultureName	varchar(10)
,@VidSubject	nvarchar(200)
,@VidDesc	nvarchar(500)
,@IsShowInLang	bit
,@PostAccount	varchar(20)
as
begin
	insert into dbo.ArticleVideoMultiLang(
		VidId, CultureName, VidSubject, 
		VidDesc, IsShowInLang, PostAccount, 
		PostDate
		)
	values(
		@VidId, @CultureName, @VidSubject, 
		@VidDesc, @IsShowInLang, @PostAccount, 
		getdate()
		)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <更新網頁影片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideo_UpdateData
@VidId	uniqueidentifier
,@SortNo	int
,@VidLinkUrl	nvarchar(2048)
,@SourceVideoId	varchar(100)
,@MdfAccount	varchar(20)
as
begin
	update dbo.ArticleVideo
	set SortNo=@SortNo
		,VidLinkUrl=@VidLinkUrl
		,SourceVideoId=@SourceVideoId
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where VidId=@VidId
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/12>
-- Description: <更新網頁影片的多國語系資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideoMultiLang_UpdateData
@VidId	uniqueidentifier
,@CultureName	varchar(10)
,@VidSubject	nvarchar(200)
,@VidDesc	nvarchar(500)
,@IsShowInLang	bit
,@MdfAccount	varchar(20)
as
begin
	update dbo.ArticleVideoMultiLang
	set VidSubject=@VidSubject
		,VidDesc=@VidDesc
		,IsShowInLang=@IsShowInLang
		,MdfAccount=@MdfAccount
		,MdfDate=getdate()
	where VidId=@VidId
		and CultureName=@CultureName
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/13>
-- Description: <取得後台用指定語系的網頁影片清單>
-- Test:
/*
declare @RowCount int
exec dbo.spArticleVideoMultiLang_GetListForBackend '00000000-0000-0000-0000-000000000000', 'zh-TW', N'', 1, 20, '', 0, 1, 1, 1, '', 0, @RowCount output
select @RowCount
*/
-- =============================================
create procedure dbo.spArticleVideoMultiLang_GetListForBackend
@ArticleId uniqueidentifier
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
	set @conditions=N' and av.ArticleId=@ArticleId and avm.CultureName=@CultureName '

	set @conditions += N'
 and (@CanReadSubItemOfOthers=1
	or @CanReadSubItemOfCrew=1 and e.DeptId=@MyDeptId
	or @CanReadSubItemOfSelf=1 and avm.PostAccount=@MyAccount) '
	
	if @Kw<>N''
	begin
		set @conditions += N' and avm.VidSubject like @Kw '
	end
	
	--取得總筆數
	set @sql = N'
select @RowCount=count(*)
from dbo.ArticleVideoMultiLang avm
	join dbo.ArticleVideo av on avm.VidId=av.VidId
	left join dbo.Employee e on avm.PostAccount=e.EmpAccount
where 1=1 ' + @conditions

	--參數定義
	set @parmDef=N'
@ArticleId	uniqueidentifier
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
		@ArticleId
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

	if @SortField in (N'VidSubject', N'SortNo', N'PostDate', N'PostDeptName')
	begin
		--允許的欄位
		set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end
	end
	else
	begin
		--預設
		set @SortExp=N' order by SortNo desc'
	end
	
	set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			avm.VidId, avm.VidSubject, avm.VidDesc, 
			dbo.fnArticleVideo_IsShowInLang(avm.VidId, ''zh-TW'') as IsShowInLangZhTw,
			dbo.fnArticleVideo_IsShowInLang(avm.VidId, ''en'') as IsShowInLangEn, 
			avm.PostAccount, avm.PostDate, avm.MdfAccount, 
			avm.MdfDate, isnull(e.DeptId, 0) as PostDeptId, d.DeptName as PostDeptName,
			av.SortNo, av.VidLinkUrl, av.SourceVideoId
		from dbo.ArticleVideoMultiLang avm
			join dbo.ArticleVideo av on avm.VidId=av.VidId
			left join dbo.Employee e on avm.PostAccount=e.EmpAccount
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
		@ArticleId
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
-- Create date: <2017/12/13>
-- Description: <刪除網頁影片資料>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spArticleVideo_DeleteData
@VidId uniqueidentifier
as
begin
	begin transaction
	begin try
		-- delete multi language data
		delete from dbo.ArticleVideoMultiLang
		where VidId=@VidId

		-- delete main data
		delete from dbo.ArticleVideo
		where VidId=@VidId

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
-- Create date: <2018/01/02>
-- Description: <取得前台用網頁影片清單>
-- Test:
/*
exec dbo.spArticleVideo_GetListForFrontend '00000000-0000-0000-0000-000000000000', 'zh-TW'
*/
-- =============================================
create procedure dbo.spArticleVideo_GetListForFrontend
@ArticleId	uniqueidentifier
,@CultureName	varchar(10)
as
begin
	select
		avm.VidId, avm.VidSubject, avm.VidDesc,
		av.SortNo, av.SourceVideoId
	from dbo.ArticleVideoMultiLang avm
		join dbo.ArticleVideo av on avm.VidId=av.VidId
	where av.ArticleId=@ArticleId
		and avm.CultureName=@CultureName
		and avm.IsShowInLang=1
	order by av.SortNo desc
end
go

----------------------------------------------------------------------------
-- 搜尋關鍵字
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- Description: <儲存搜尋關鍵字>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spKeyword_SaveData
@CultureName	varchar(10)
,@Kw	nvarchar(100)
as
begin
	if exists(select * from dbo.Keyword 
				where CultureName=@CultureName
					and Kw=@Kw)
	begin
		-- increase used count
		update dbo.Keyword
		set UsedCount += 1
		where CultureName=@CultureName
			and Kw=@Kw
			and UsedCount>=0	-- >= 0 : enabled
	end
	else
	begin
		-- new one
		insert into dbo.Keyword(
			CultureName, Kw, UsedCount
			)
		values(
			@CultureName, @Kw, 1
			)
	end
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/13>
-- Description: <取得前台用搜尋關鍵字>
-- Test:
/*
exec dbo.spKeyword_GetListForFrontend 'zh-TW', N'測', 5
*/
-- =============================================
create procedure dbo.spKeyword_GetListForFrontend
@CultureName	varchar(10)
,@Kw	nvarchar(100)
,@TopCount int=5
as
begin
	select top(@TopCount)
		Kw, UsedCount
	from dbo.Keyword
	where CultureName=@CultureName
		and Kw like N'%'+@Kw+N'%'
		and UsedCount>0
	order by UsedCount desc
end
go

----------------------------------------------------------------------------
-- 搜尋用資料來源
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- Description: <建立搜尋用資料來源>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spSearchDataSource_Build
@MainLinkUrl	nvarchar(2048) = N''
as
begin
	if @MainLinkUrl=N''
	begin
		set @MainLinkUrl = N'~/Article.aspx'
	end

	--刪除原本資料
	truncate table dbo.SearchDataSource
	
	--建立網頁資料
	insert dbo.SearchDataSource(
		ArticleId, CultureName, ArticleSubject, 
		ArticleContext, ReadCount, 
		LinkUrl, 
		PublishDate, 
		BreadcrumbData, Lv1ArticleId, PostDate)
		select
			a.ArticleId, am.CultureName, am.ArticleSubject, 
			/*dbo.fnStripHTML(am.ArticleContext)*/ am.TextContext, am.ReadCount, 
			case a.ShowTypeId when 3/*URL*/ then a.LinkUrl else @MainLinkUrl end as LinkUrl, 
			a.PublishDate, 
			dbo.fnBuildBreadcrumbData(a.ArticleId, am.CultureName), dbo.fnGetLv1ArticleId(a.ArticleId), getdate()
		from dbo.Article a
			join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId
			join dbo.Article p on a.ParentId=p.ArticleId
		where a.IsHideSelf=0 
			and am.IsShowInLang=1 
			and a.StartDate <= getdate() and getdate() < a.EndDate+1
			and p.IsHideChild=0

	--刪除不需要的資料
	delete s
	from dbo.SearchDataSource s
	where s.ArticleSubject=''
		or (isnull(s.ArticleContext,'')='' 
			and not exists(select * from dbo.AttachFile
						where ArticleId=s.ArticleId) --留下有附件的空文章
			)
		or s.ArticleId='00000000-0000-0000-0000-000000000000'
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/09>
-- Description: <取得搜尋用資料來源清單>
/*
declare @RowCount int
exec dbo.spSearchDataSource_GetList N'測試', 'zh-TW', 1, 99999, 'MatchesTotal', 0, @RowCount output
select @RowCount

declare @RowCount int
exec dbo.spSearchDataSource_GetList N'test', 'en', 1, 99999, '', 0, @RowCount output
select @RowCount

declare @RowCount int
exec dbo.spSearchDataSource_GetList N'測試,縮圖,區塊,清單', 'zh-TW', 1, 99999, '', 1, @RowCount output
select @RowCount

*/
-- =============================================
create procedure dbo.spSearchDataSource_GetList
@Keywords nvarchar(4000)=''	-- 多項關聯字用逗號串接; Multiple related words concatenated with commas(,), e.g., one,two,three
,@CultureName varchar(10)
,@BeginNum int
,@EndNum int
,@SortField nvarchar(20)=''
,@IsSortDesc bit=0
,@RowCount int output
as
begin
	-- 建立暫存表記錄符合數量
	create table #tblMatches(
		ArticleId	uniqueidentifier
		,SubId	uniqueidentifier
		,total int
	)

	declare @sql nvarchar(4000)
	declare @parmDef nvarchar(4000)
	declare @parmDefForTotal nvarchar(4000)
	declare @conditions nvarchar(4000)
	declare @SortExp nvarchar(200)
	declare @colClause nvarchar(500) = N''
	declare @joinClause nvarchar(500) = N''

	if charindex(',', @Keywords)=0
	begin
		-- 單一關鍵字查詢

		--條件定義
		set @conditions = N' and sds.CultureName=@CultureName '
	
		if @Keywords<>N''
		begin
			set @conditions += N' and (sds.ArticleSubject like @Keywords or sds.ArticleContext like @Keywords) '
		end

		--條件定義
		set @colClause = N',1 as MatchesTotal'
	end
	else
	begin
		-- 多項關聯字查詢

		insert into #tblMatches
			select 
				ArticleId, SubId, 0
			from dbo.SearchDataSource 
			where CultureName=@CultureName

		-- 記錄符合的關鍵字數量
		declare @Token nvarchar(4000)

		declare curKeyword cursor for
			select Token 
			from dbo.fnStringToTable(N',', @Keywords)

		open curKeyword

		fetch next from curKeyword into @Token

		while @@fetch_status=0
		begin
			set @Token = N'%'+@Token+N'%'

			update t
			set total = total+1
			from #tblMatches t
				join dbo.SearchDataSource sds on t.ArticleId=sds.ArticleId and t.SubId=sds.SubId
			where sds.CultureName=@CultureName
				and (sds.ArticleSubject like @Token or sds.ArticleContext like @Token)

			fetch next from curKeyword into @Token
		end

		close curKeyword
		deallocate curKeyword

		--條件定義
		set @colClause = N',t.total as MatchesTotal'
		set @joinClause = N' join #tblMatches t on sds.ArticleId=t.ArticleId and sds.SubId=t.SubId  '
		set @conditions = N' and sds.CultureName=@CultureName and t.total>0 '
	end

	
		--取得總筆數
		set @sql = N'
select @RowCount=count(*)
from dbo.SearchDataSource sds
' + @joinClause + N'
where 1=1 ' + @conditions

		--參數定義
		set @parmDef=N'
@Keywords nvarchar(100)
,@CultureName varchar(10)
'

		set @parmDefForTotal = @parmDef + N',@RowCount int output'

		set @Keywords = N'%'+@Keywords+N'%'

		exec sp_executesql @sql, @parmDefForTotal, 
			@Keywords
			,@CultureName
			,@RowCount output

		--取得指定排序和範圍的結果

		--指定排序
		set @SortExp=N' order by '

		if @SortField in (N'ReadCount', N'ArticleSubject', N'PublishDate', N'MatchesTotal')
		begin
			--允許的欄位
			set @SortExp = @SortExp+@SortField+case @IsSortDesc when 1 then N' desc' else N' asc' end

			if @SortField=N'MatchesTotal'
			begin
				set @SortExp += N', PublishDate desc'
			end
		end
		else
		begin
			--預設
			set @SortExp=N' order by MatchesTotal desc, PublishDate desc'
		end

		set @sql=N'
select *
from (
	select row_number() over(' + @SortExp + N') as RowNum, *
	from (
		select
			sds.ArticleId, sds.SubId, 
			sds.ArticleSubject, sds.ArticleContext, sds.ReadCount, 
			sds.LinkUrl, sds.PublishDate, sds.BreadcrumbData, 
			sds.Lv1ArticleId, sds.PostDate, sds.MdfDate
			' + @colClause + N'
		from dbo.SearchDataSource sds
' + @joinClause + N'
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
			@Keywords
			,@CultureName
			,@BeginNum
			,@EndNum

	drop table #tblMatches
end
go



/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/31>
-- Description: <xxxxxxxxxxxxxxxxxx>
-- Test:

-- =============================================
create procedure xxxxx

as
begin

end
go

*/
