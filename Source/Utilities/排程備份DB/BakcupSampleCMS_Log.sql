print convert(varchar(20), getdate(), 120)
declare @FileFullName nvarchar(500)
set @FileFullName = N'D:\SqlDataBackup\SQLEXPRESS2012\SampleCMS\SampleCMS_'+replace(replace(replace(convert(varchar(20), getdate(), 120), '-', ''), ':', ''), ' ', '_')+N'.trn'

BACKUP LOG [SampleCMS] TO  DISK = @FileFullName WITH NOFORMAT, NOINIT,  NAME = N'SampleCMS-交易記錄  備份', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
print @FileFullName
GO
