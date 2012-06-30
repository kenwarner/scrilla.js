USE [master]
GO

IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'scrilla')
BEGIN

	EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'scrilla'

	ALTER DATABASE [scrilla] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE

	USE [master]

	DROP DATABASE [scrilla]

END
GO
