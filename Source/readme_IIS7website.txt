在本機開發本專案前，請先在 IIS7(含更新版本) 中照下列項目設定網站：

*. 請先在 C:\Windows\System32\drivers\etc\hosts 內容新增 127.0.0.1 SampleCMS.dev.com

1. 新增網站：(SampleCMS前端網站)
．站台名稱[SampleCMS]
．應用程式集區[.NET 4.0 整合式]
．目錄選擇此專案的資料夾[Root]
．主機名稱[SampleCMS]
．完成

2. 在網站下新增[應用程式]：(SampleCMS後端網站)
．別名[Management]
．應用程式集區[.NET 4.0 整合式]
．目錄選擇此專案的資料夾[Management]
．完成

3. 在[Root]下新增[虛擬目錄]：(SampleCMS前端和後端網站的共用圖片區)
．別名[images]
．目錄選擇此專案的資料夾[Management/images]
．完成

4. 在[Root]下新增[虛擬目錄]：(FCKeditor編輯器上傳目錄，後端用)
．別名[UserFiles]
．目錄選擇此專案的資料夾[Management/UserFiles]
．完成

5. 在[Root]下新增[虛擬目錄]：(共用附件區，後端用)
．別名[Attachments]
．目錄選擇此專案的資料夾[Management/Attachments]
．完成

p.s. 預設後端管理者帳號/密碼: admin / admin
