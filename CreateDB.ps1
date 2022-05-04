$ServerName = $args[0]
if($ServerName){
  invoke-sqlcmd -inputfile ".\src\SQL\CreateDatabase.sql" -serverinstance $ServerName
} else {
  Write-Output "Please provide additional parameter: SQL Server local sever name"
}
