﻿-- Article Publisher tables
use SampleCMS
go

----------------------------------------------------------------------------
-- dbo.Article 網頁內容	
----------------------------------------------------------------------------
create table dbo.Article(
	ArticleId	uniqueidentifier	Not Null
	,SeqnoForCluster	int	Not Null	identity
	,ParentId	uniqueidentifier		
	,ArticleLevelNo	int		
	,ArticleAlias	varchar(50)		
	,BannerPicFileName	nvarchar(255)		
	,LayoutModeId	int
	,ShowTypeId	int
	,LinkUrl	nvarchar(2048)		
	,LinkTarget	varchar(10)		
	,ControlName	varchar(100)		
	,SubItemControlName	varchar(100)		
	,IsHideSelf	bit	Not Null	Default(0)
	,IsHideChild	bit	Not Null	Default(0)
	,StartDate	datetime		
	,EndDate	datetime		
	,SortNo	int		
	,DontDelete	bit	Not Null	Default(0)
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
	,constraint PK_Article primary key nonclustered (ArticleId)
)
go
-- 為避免 GUID 造成的索引破碎帶來的效能影響，叢集索引使用自動編號並且與主鍵分開
create clustered index IX_Article on dbo.Article (SeqnoForCluster)
go
-- 預設內容
set identity_insert dbo.Article on
insert into dbo.Article(
	ArticleId, SeqnoForCluster, ArticleLevelNo, 
	ArticleAlias, LayoutModeId, ShowTypeId, 
	LinkUrl, StartDate, EndDate, 
	SortNo, PostAccount, PostDate
	)
values(
	'00000000-0000-0000-0000-000000000000', 1, 0, 
	'root', 1, 3, 
	'http://SampleCMS.dev.com/', getdate(), dateadd(year,100,getdate()),
	0, 'admin', getdate()
	)
go

set identity_insert dbo.Article off
go

----------------------------------------------------------------------------
-- dbo.ArticleMultiLang 網頁內容的多國語系資料	
----------------------------------------------------------------------------
create table dbo.ArticleMultiLang(
	ArticleId	uniqueidentifier	Not Null
	,CultureName	varchar(10)	Not Null
	,SeqnoForCluster	int	Not Null	identity
	,ArticleSubject	nvarchar(200)		
	,ArticleContext	nvarchar(max)		
	,ReadCount	int	Not Null	Default(0)
	,IsShowInLang	bit	Not Null	Default(1)
	,PostAccount	varchar(20)		
	,PostDate	datetime		
	,MdfAccount	varchar(20)		
	,MdfDate	datetime		
	,constraint PK_ArticleMultiLang primary key nonclustered (ArticleId, CultureName)
)
go
-- 為避免 GUID 造成的索引破碎帶來的效能影響，叢集索引使用自動編號並且與主鍵分開
create clustered index IX_ArticleMultiLang on dbo.ArticleMultiLang (SeqnoForCluster)
go
-- 預設內容
set identity_insert dbo.ArticleMultiLang on
insert into dbo.ArticleMultiLang(
	ArticleId, CultureName, SeqnoForCluster, 
	ArticleSubject, IsShowInLang, PostAccount,
	PostDate
	)
values(
	'00000000-0000-0000-0000-000000000000', 'zh-TW', 1, 
	N'網站架構管理', 1, 'admin',
	getdate()
	)
go

insert into dbo.ArticleMultiLang(
	ArticleId, CultureName, SeqnoForCluster, 
	ArticleSubject, IsShowInLang, PostAccount,
	PostDate
	)
values(
	'00000000-0000-0000-0000-000000000000', 'en', 2, 
	N'Site Architecture Mgmt.', 1, 'admin',
	getdate()
	)
go

set identity_insert dbo.ArticleMultiLang off
go


go
----------------------------------------------------------------------------
-- dbo.TableName 資料表名稱
----------------------------------------------------------------------------
go