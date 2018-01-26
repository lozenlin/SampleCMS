--SQL Injection參數過濾用預存程序建立
-- use SampleCMS
go

----------------------------------------------------------------------------
-- SQL Injection 相關
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/17>
-- Description: <測試運算式是否成立,能否被用來做 SQL Injection>
/*
exec dbo.spIsSQLInjectionExpr '31337-31337="0'
exec dbo.spIsSQLInjectionExpr ' 5 * 2 /3  = 3'
exec dbo.spIsSQLInjectionExpr ' 5 * 2 /3  = ''3'
exec dbo.spIsSQLInjectionExpr ' ''5'' * 2 /3  = 3'
*/
-- =============================================
create procedure dbo.spIsSQLInjectionExpr
@Expr varchar(8000)
as
begin
	--雙引號都改為單引號
	set @Expr = replace(@Expr, '"', '''')
	
	--測試運算式
	begin try
		--有單引號的,如: or 31337-31337='0
		exec('if '+@Expr+''' select cast(1 as bit) else select cast(0 as bit)')
		print 'path1'
		return
	end try
	begin catch
	end catch

	begin try
		--沒單引號的,如: and 31337-31337=0
		exec('if '+@Expr+' select cast(1 as bit) else select cast(0 as bit)')
		print 'path2'
		return
	end try
	begin catch
	end catch

	print 'path3'
	select cast(0 as bit)
end
go


/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/17>
-- Description: <xxxxxxxxxxxxxxxxxx>
-- Test:

-- =============================================
create procedure xxxxx

as
begin

end
go

*/
