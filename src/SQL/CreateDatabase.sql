IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DiscordBotDB')
Begin
	CREATE DATABASE DiscordBotDB
END;
GO

USE DiscordBotDB;
GO

CREATE TABLE DiscordUser (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	DiscordId varchar(50),
	NickName varchar(50)
)

CREATE TABLE WordleRecord (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	UserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES DiscordUser(Id),
	Score int,
	PostDate DATETIME
)

