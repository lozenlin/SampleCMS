--SQL Injection�ѼƹL�o�ιw�s�{�ǫإ�
-- use SampleCMS
go

----------------------------------------------------------------------------
-- SQL Injection ����
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/17>
-- Description: <���չB�⦡�O�_����,��_�Q�ΨӰ� SQL Injection>
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
	--���޸����אּ��޸�
	set @Expr = replace(@Expr, '"', '''')
	
	--���չB�⦡
	begin try
		--����޸���,�p: or 31337-31337='0
		exec('if '+@Expr+''' select cast(1 as bit) else select cast(0 as bit)')
		print 'path1'
		return
	end try
	begin catch
	end catch

	begin try
		--�S��޸���,�p: and 31337-31337=0
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
