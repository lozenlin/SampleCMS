"C:\Program Files\Microsoft SQL Server\110\Tools\Binn\sqlcmd" -S .\sqlexpress2012 -E -i BakcupSampleCMS_Full.sql -o D:\SqlDataBackup\SQLEXPRESS2012\SampleCMS\result_Full.txt
rem §R°£ÂÂ³Æ¥÷ÀÉ
forfiles /p D:\SqlDataBackup\SQLEXPRESS2012\SampleCMS /m SampleCMS*.bak /d -3 /c "cmd /c del @path"