$conn = New-Object System.Data.SqlClient.SqlConnection("Server=tcp:YourAzureDB,1433;Initial Catalog=SampleCMS;Persist Security Info=False;User ID=YourAccount;Password=YourPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=UpdateSearchDataSource;")
$cmd = New-Object System.Data.SqlClient.SqlCommand("dbo.spSearchDataSource_Build", $conn)
$cmd.CommandType = [System.Data.CommandType]::StoredProcedure
$paMainLinkUrl = New-Object System.Data.SqlClient.SqlParameter("@MainLinkUrl", "")
$cmd.Parameters.Add($paMainLinkUrl)

$conn.Open()
$cmd.ExecuteNonQuery()
$conn.Close()
