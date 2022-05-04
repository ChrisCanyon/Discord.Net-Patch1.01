# Discord.Net DiscordBot
Base Project for AWS hosted Discord Bot with instructions for local development and deploy
Technologies Used:
- Discord.Net
- SQL Server
- Entity Framework
- AWS Elastic Beanstalk
- AWS RDS

# Setup
## Config
add a config file named "_config.yml" and place in src\\bin\\Debug\\net5.0
it should look like this:
```
prefix: !
tokens:
	discord: DISCORD_BOT_TOKEN
connection_string: YOUR_DB_CONNECTION_STRING
```
Replace **DISCORD_BOT_TOKEN** with your discord bot's token

See this [Dev Guide](https://discord.com/developers/docs/getting-started) for setting up a discord bot on discord and getting a bot token

## Entity Framework (Local DB Setup)
Download and install [SQLServer Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
Download and install [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

In an admin PowerShell window run:
```
Install-Module -Name "SqlServer"
```
Run 'CreateDB.ps1' from an admin PowerShell window.
If you encounter 'not digitally signed' errors' run the following first:
```
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```
You should have a DB created on your local SQL Server instance. Open SSMS and check if it exists
Now that your database is created, connect to it in Visual Studio using the server explorer and find the connection string. Add the connection string to _config.yml

### If changes to DB schema are made please rerun EF scaffolding to reflect those changes in code
From the src directory run:

```
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=DiscordBotDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -f
```
You man need to install the ef scaffold Tool (in powershell)
```
dotnet tool install --global dotnet-ef
```



## AWS

### Elastic Beanstalk (EB)

### Relational Database Service (RDS)

