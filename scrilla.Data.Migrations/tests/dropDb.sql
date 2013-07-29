USE [master]
GO

IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'scrilla.js.tests')
BEGIN

	EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'scrilla.js.tests'

	ALTER DATABASE [scrilla.js.tests] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE

	USE [master]

	DROP DATABASE [scrilla.js.tests]

END
GO
