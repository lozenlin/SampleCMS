use SampleCMS
go
print convert(varchar(20), getdate(), 120)
exec dbo.spSearchDataSource_Build '~/Article.aspx'
go
